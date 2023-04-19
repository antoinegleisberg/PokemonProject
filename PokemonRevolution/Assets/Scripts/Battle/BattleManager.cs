using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public PokemonParty PlayerParty;
    public PokemonParty EnemyParty;
    public Pokemon PlayerPokemon;
    public Pokemon EnemyPokemon;

    private BattleManagerBaseState currentState;
    public BattleManagerOutOfBattleState OutOfBattleState;
    public BattleManagerStartState StartState;
    public BattleManagerPlayerTurnState PlayerTurnState;
    public BattleManagerEnemyMoveState EnemyMoveState;
    public BattleManagerEndBattleState EndBattleState;

    public void SwitchState(BattleManagerBaseState newState)
    {
        Debug.Log("Entering state: " + newState.GetType().Name);
        StartCoroutine(SwitchStateCoroutine(newState));
    }

    public void PerformMove(Pokemon attackingPokemon, Pokemon targetPokemon, Move move)
    {
        AttackInfo attackInfo = CalculateMoveDamage(attackingPokemon, targetPokemon, move);
        targetPokemon.TakeDamage(attackInfo.damage);
        if (targetPokemon.IsFainted) attackInfo.fainted = true;

        attackingPokemon.LoseMovePP(move);

        BattleEvents.Current.PokemonAttacks(attackingPokemon, targetPokemon, move, attackInfo);
        if (targetPokemon.IsFainted) BattleEvents.Current.PokemonFaints(targetPokemon);
    }

    private void Awake()
    {
        OutOfBattleState = new BattleManagerOutOfBattleState();
        StartState = new BattleManagerStartState();
        PlayerTurnState = new BattleManagerPlayerTurnState();
        EnemyMoveState = new BattleManagerEnemyMoveState();
        EndBattleState = new BattleManagerEndBattleState();
    }

    private void Start()
    {
        OutOfBattleState.InitState(this);
        StartState.InitState(this);
        PlayerTurnState.InitState(this);
        EnemyMoveState.InitState(this);
        EndBattleState.InitState(this);

        currentState = OutOfBattleState;

        GameEvents.Current.OnPokemonEncounter += OnPokemonEncounter;
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    private void OnDestroy()
    {
        OutOfBattleState.OnDestroy();
        StartState.OnDestroy();
        PlayerTurnState.OnDestroy();
        EnemyMoveState.OnDestroy();
        EndBattleState.OnDestroy();

        GameEvents.Current.OnPokemonEncounter -= OnPokemonEncounter;
    }

    private IEnumerator SwitchStateCoroutine(BattleManagerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        while (UIManager.Current.IsBusy)
        {
            yield return null;
        }
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

        float damage = (((2 * level / 5 + 2) * attack * power / defense / 50) + 2) * criticalHitModifier * stabModifier * typeModifier * randomModifier;
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
        SwitchState(StartState);
    }
}