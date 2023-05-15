using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public PokemonParty PlayerParty { get; private set; }
    public PokemonParty EnemyParty { get; private set; }
    public Pokemon PlayerPokemon { get; private set; }
    public Pokemon EnemyPokemon { get; private set; }
    public int NumberPokemonsPerTeam { get; private set; } // TODO
    public List<Pokemon> PlayerPokemons { get; private set; } // TODO
    public List<Pokemon> EnemyPokemons { get; private set; } // TODO

    public Trainer EnemyTrainer { get; set; }
    public bool IsTrainerBattle { get; set; }

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

    public void StartBattle(PokemonParty playerParty, PokemonParty enemyParty)
    {
        PlayerParty = playerParty;
        EnemyParty = enemyParty;
        PlayerPokemon = PlayerParty.GetFirstPokemon();
        EnemyPokemon = EnemyParty.GetFirstPokemon();
        SwitchState(StartBattleState);
    }

    public void SwitchState(BattleManagerBaseState newState)
    {
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
        // Check if the move can be performed, depending on status conditions
        ConditionAttackModifier attackModifier = attackingPokemon.OnBeforeMove(move);
        if (!attackModifier.CanPerformMove)
            return;

        // Apply PP loss
        attackingPokemon.LoseMovePP(move);

        // Calculate attack info
        AttackInfo attackInfo = CalculateMoveInfo(attackingPokemon, targetPokemon, move, attackModifier);

        // Raise attack event
        BattleEvents.Instance.PokemonAttacks(attackingPokemon, targetPokemon, move, attackInfo);

        // If the move misses, don't apply damage or effects
        if (!attackInfo.moveHits)
            return;

        // Apply damage
        targetPokemon.TakeDamage(attackInfo.damage);

        // Apply effects
        if (move.ScriptableMove.MoveEffects != null)
            move.ScriptableMove.MoveEffects.ApplyEffects(attackingPokemon, targetPokemon);
    }
    
    public bool CanRunFromBattle()
    {
        if (IsTrainerBattle)
        {
            BattleEvents.Instance.AttemptRunFromTrainer();
            return false;
        }

        BattleEvents.Instance.TryToRunAway();
        if (PlayerPokemon.Speed >= EnemyPokemon.Speed)
        {
            BattleEvents.Instance.RunAwaySuccess();
            return true;
        }

        int odds = (Mathf.FloorToInt(PlayerPokemon.Speed * 128 / EnemyPokemon.Speed) + 30) % 256;
        if (Random.Range(0, 256) < odds)
        {
            BattleEvents.Instance.RunAwaySuccess();
            return true;
        }
        BattleEvents.Instance.RunAwayFail();
        return false;
    }

    public bool CanCatchPokemon()
    {
        if (IsTrainerBattle)
        {
            BattleEvents.Instance.AttemptCatchTrainerPokemon();
            return false;
        }

        float ballBonus = 1;
        float statusBonus = ConditionsDB.Conditions[EnemyPokemon.StatusCondition].CatchRateModifier;
        float baseCatchRate = 1 - 2.0f * EnemyPokemon.CurrentHP / (3.0f * EnemyPokemon.MaxHP);
        int catchRate = Mathf.FloorToInt(baseCatchRate * EnemyPokemon.ScriptablePokemon.CatchRate * ballBonus * statusBonus);
        bool caught = Random.Range(0, 256) <= catchRate;
        StartCoroutine(CanCatchPokemonCoroutine(caught));
        if (caught)
        {
            EnemyPokemon.Owner = PokemonOwner.Player;
            if (!PlayerParty.IsFull)
            {
                PlayerParty.AddPokemon(EnemyPokemon);
            }
            else
                Debug.Log("Party is full ! TODO : add it to the box");
        }
        return caught;
    }

    private IEnumerator CanCatchPokemonCoroutine(bool caught)
    {
        BattleEvents.Instance.ThrowPokeball(EnemyPokemon);
        yield return BattleUIManager.Instance.WaitWhileBusy();
        if (caught)
            BattleEvents.Instance.PokemonCaught(EnemyPokemon);
        else
            BattleEvents.Instance.PokemonEscapes(EnemyPokemon);
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

        BattleEvents.Instance.OnAfterPokemonFainted += AfterPokemonFainted;
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

        BattleEvents.Instance.OnAfterPokemonFainted -= AfterPokemonFainted;
    }

    private void AfterPokemonFainted(Pokemon faintedPokemon)
    {
        if (faintedPokemon.Owner == PokemonOwner.Player || faintedPokemon.Owner == PokemonOwner.AllyTrainer)
            return;
        
        StartCoroutine(PlayerPokemon.GainExp(CalculateExpGained(faintedPokemon)));
    }

    private int CalculateExpGained(Pokemon faintedPokemon)
    {
        int exp = Mathf.FloorToInt(faintedPokemon.ScriptablePokemon.ExperienceYield * faintedPokemon.Level / 7);
        float trainerBonus = (IsTrainerBattle) ? 1.5f : 1.0f;
        return Mathf.FloorToInt(exp * trainerBonus);
    }

    private IEnumerator SwitchStateCoroutine(BattleManagerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;

        yield return BattleUIManager.Instance.WaitWhileBusy();

        currentState.EnterState();
    }

    private AttackInfo CalculateMoveInfo(Pokemon attackingPokemon, Pokemon targetPokemon, Move move, ConditionAttackModifier attackModifier)
    {
        AttackInfo attackInfo = CalculateMoveDamage(attackingPokemon, targetPokemon, move, attackModifier);
        attackInfo.moveHits = MoveHits(attackingPokemon, targetPokemon, move);
        return attackInfo;
    }

    private bool MoveHits(Pokemon attackingPokemon, Pokemon defendingPokemon, Move move)
    {
        if (move.ScriptableMove.AlwaysHits)
            return true;

        float accuracy = move.ScriptableMove.Accuracy * attackingPokemon.Accuracy / defendingPokemon.Evasion;
        int roundedAccuracy = Mathf.RoundToInt(accuracy);

        return Random.Range(0, 100) < roundedAccuracy;
    }
    
    private AttackInfo CalculateMoveDamage(Pokemon attackingPokemon, Pokemon targetPokemon, Move move, ConditionAttackModifier attackModifier)
    {
        if (move.ScriptableMove.Category == MoveCategory.Status) 
            return new AttackInfo(false, 0, 1.0f, true);

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
        damage *= attackModifier.DamageMultiplier;
        int roundedDamage = Mathf.Max(1, Mathf.RoundToInt(damage));

        AttackInfo attackInfo = new AttackInfo(criticalHit, roundedDamage, typeModifier, true);
        
        return attackInfo;
    }
}