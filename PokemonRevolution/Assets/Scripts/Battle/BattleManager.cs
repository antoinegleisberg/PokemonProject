using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public PokemonParty PlayerParty { get; private set; }
    public PokemonParty EnemyParty { get; private set; }
    public Pokemon PlayerPokemon { get; private set; }
    public Pokemon EnemyPokemon { get; private set; }

    public BattleActionInfo NextPlayerAction { get; set; }
    public BattleActionInfo NextEnemyAction { get; set; }

    private BattleManagerBaseState currentState;
    public BattleManagerOutOfBattleState OutOfBattleState;
    public BattleManagerStartBattleState StartBattleState;
    public BattleManagerStartTurnState StartTurnState;
    public BattleManagerActionSelectionState ActionSelectionState;
    public BattleManagerPerformMovesState PerformMovesState;
    public BattleManagerEndTurnState EndTurnState;
    public BattleManagerEndBattleState EndBattleState;

    public void SwitchState(BattleManagerBaseState newState)
    {
        // Debug.Log("Entering state: " + newState.GetType().Name);
        StartCoroutine(SwitchStateCoroutine(newState));
    }

    public void SwitchPokemon(Pokemon oldPokemon, Pokemon newPokemon)
    {
        oldPokemon.OnPokemonSwitchedOut();
        if (!oldPokemon.IsFainted)
            BattleEvents.Instance.PokemonSwitchedOut(oldPokemon);
        if (newPokemon.Owner == PokemonOwner.Player)
            PlayerPokemon = newPokemon;
        else if (newPokemon.Owner == PokemonOwner.EnemyTrainer)
            EnemyPokemon = newPokemon;
        BattleEvents.Instance.PokemonSwitchedIn(newPokemon);
    }

    public void PerformMove(Pokemon attackingPokemon, Pokemon targetPokemon, Move move)
    {
        // Apply PP loss
        attackingPokemon.LoseMovePP(move);
        
        // Apply damage
        AttackInfo attackInfo = CalculateMoveDamage(attackingPokemon, targetPokemon, move);
        targetPokemon.TakeDamage(attackInfo.damage);
        if (targetPokemon.IsFainted)
            attackInfo.fainted = true;
        BattleEvents.Instance.PokemonAttacks(attackingPokemon, targetPokemon, move, attackInfo);
        if (attackInfo.damage > 0) BattleEvents.Instance.PokemonDamaged(targetPokemon, attackInfo.damage);

        // Apply effects
        if (move.ScriptableMove.MoveEffects != null)
            move.ScriptableMove.MoveEffects.ApplyEffects(attackingPokemon, targetPokemon);

        if (targetPokemon.IsFainted) BattleEvents.Instance.PokemonFaints(targetPokemon);
    }

    private void Awake()
    {
        OutOfBattleState = new BattleManagerOutOfBattleState();
        StartBattleState = new BattleManagerStartBattleState();
        StartTurnState = new BattleManagerStartTurnState();
        ActionSelectionState = new BattleManagerActionSelectionState();
        PerformMovesState = new BattleManagerPerformMovesState();
        EndTurnState = new BattleManagerEndTurnState();
        EndBattleState = new BattleManagerEndBattleState();
    }

    private void Start()
    {
        OutOfBattleState.InitState(this);
        StartBattleState.InitState(this);
        StartTurnState.InitState(this);
        ActionSelectionState.InitState(this);
        PerformMovesState.InitState(this);
        EndTurnState.InitState(this);
        EndBattleState.InitState(this);

        currentState = OutOfBattleState;

        GameEvents.Instance.OnPokemonEncounter += OnPokemonEncounter;
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    private void OnDestroy()
    {
        OutOfBattleState.OnDestroy();
        StartBattleState.OnDestroy();
        StartTurnState.OnDestroy();
        ActionSelectionState.OnDestroy();
        PerformMovesState.OnDestroy();
        EndTurnState.OnDestroy();
        EndBattleState.OnDestroy();

        GameEvents.Instance.OnPokemonEncounter -= OnPokemonEncounter;
    }

    private IEnumerator SwitchStateCoroutine(BattleManagerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;

        yield return UIManager.Instance.WaitWhileBusy();

        currentState.EnterState();
    }

    private AttackInfo CalculateMoveDamage(Pokemon attackingPokemon, Pokemon targetPokemon, Move move)
    {
        if (move.ScriptableMove.Category == MoveCategory.Status) 
            return new AttackInfo(false, 0, 1.0f, false);

        int level = attackingPokemon.Level;
        int attack = (move.ScriptableMove.Category == MoveCategory.Special) ? attackingPokemon.SpecialAttack : attackingPokemon.Attack;
        int defense = (move.ScriptableMove.Category == MoveCategory.Special) ? targetPokemon.SpecialDefense : targetPokemon.Defense;
        
        int power = move.ScriptableMove.Power;
        bool criticalHit = Random.Range(1, 16) == 1;
        int criticalHitModifier = criticalHit ? 2 : 1;
        bool stab = attackingPokemon.ScriptablePokemon.Type1 == move.ScriptableMove.Type || 
            attackingPokemon.ScriptablePokemon.Type2 == move.ScriptableMove.Type;
        float stabModifier = stab ? 1.5f : 1.0f;
        float typeModifier = TypeUtils.TypeModifier(move.ScriptableMove, targetPokemon.ScriptablePokemon);
        float randomModifier = Random.Range(0.85f, 1.0f);

        float baseDamage = (((2 * level / 5 + 2) * attack * power / defense / 50) + 2);
        float damage = baseDamage * criticalHitModifier * stabModifier * typeModifier * randomModifier;
        int roundedDamage = Mathf.Max(1, Mathf.RoundToInt(damage));

        AttackInfo attackInfo = new AttackInfo(criticalHit, roundedDamage, typeModifier, false);
        
        return attackInfo;
    }

    private void OnPokemonEncounter(PokemonParty playerParty, PokemonParty enemyParty)
    {
        PlayerParty = playerParty;
        EnemyParty = enemyParty;
        PlayerPokemon = PlayerParty.GetFirstPokemon();
        EnemyPokemon = EnemyParty.GetFirstPokemon();
        SwitchState(StartBattleState);
    }
}