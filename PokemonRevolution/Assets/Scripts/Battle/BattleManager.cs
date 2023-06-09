using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private BattleSceneUIManager battleSceneUIManager;
    [SerializeField] private BattleDialogueUIManager battleDialogueUIManager;
    [SerializeField] private BattleActionSelectorsUIManager battleActionSelectorsUIManager;

    public BattleSceneUIManager BattleSceneUIManager { get => battleSceneUIManager; }
    public BattleDialogueUIManager BattleDialogueUIManager { get => battleDialogueUIManager; }
    public BattleActionSelectorsUIManager BattleActionSelectorsUIManager { get => battleActionSelectorsUIManager; }

    
    private ScriptableMove moveToLearn;


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

        BattleEvents.Instance.OnPokemonFainted += OnPokemonFainted;
        BattleUIEvents.Instance.OnSelectMoveToForget += SelectedMoveToForget;
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

        BattleEvents.Instance.OnPokemonFainted -= OnPokemonFainted;
        BattleUIEvents.Instance.OnSelectMoveToForget -= SelectedMoveToForget;
    }

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

    public void DamagePokemon(Pokemon target, float damage)
    {
        int roundedDamage = Mathf.Max(1, Mathf.RoundToInt(damage));

        target.TakeDamage(roundedDamage);

        BattleEvents.Instance.PokemonDamaged(target, roundedDamage);
        if (target.IsFainted)
        {
            BattleEvents.Instance.PokemonFaints(target);
        }
    }

    public void SwitchPokemon(Pokemon oldPokemon, Pokemon newPokemon)
    {
        oldPokemon.OnPokemonSwitchedOut();
        if (!oldPokemon.IsFainted)
        {
            BattleEvents.Instance.PokemonSwitchedOut(oldPokemon);
        }
        if (newPokemon.Owner == PokemonOwner.Player)
        {
            PlayerPokemon = newPokemon;
        }
        else if (newPokemon.Owner == PokemonOwner.EnemyTrainer)
        {
            EnemyPokemon = newPokemon;
        }
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
        if (move.ScriptableMove.Category != MoveCategory.Status)
        {
            DamagePokemon(targetPokemon, attackInfo.damage);
        }

        // Apply effects
        if (move.ScriptableMove.MoveEffects != null)
            move.ScriptableMove.MoveEffects.ApplyEffects(attackingPokemon, targetPokemon);
    }
    
    public bool CanRunFromBattle()
    {
        if (IsTrainerBattle)
        {
            BattleDialogueUIManager.OnAttemptRunFromTrainer();
            return false;
        }

        BattleDialogueUIManager.OnTryToRunAway();
        if (PlayerPokemon.Speed >= EnemyPokemon.Speed)
        {
            BattleDialogueUIManager.OnRunAwaySuccess();
            return true;
        }

        int odds = (Mathf.FloorToInt(PlayerPokemon.Speed * 128 / EnemyPokemon.Speed) + 30) % 256;
        if (Random.Range(0, 256) < odds)
        {
            BattleDialogueUIManager.OnRunAwaySuccess();
            return true;
        }
        BattleDialogueUIManager.OnRunAwayFail();
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
        float statusBonus = ConditionsDB.GetCondition(EnemyPokemon.StatusCondition).CatchRateModifier;
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

    private IEnumerator SwitchStateCoroutine(BattleManagerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;

        yield return BattleUIManager.Instance.WaitWhileBusy();

        currentState.EnterState();
    }

    private void OnPokemonFainted(Pokemon faintedPokemon)
    {
        BattleDialogueUIManager.OnPokemonFainted(faintedPokemon);
        BattleSceneUIManager.OnPokemonFainted(faintedPokemon);

        if (faintedPokemon.Owner == PokemonOwner.Player || faintedPokemon.Owner == PokemonOwner.AllyTrainer)
            return;
        
        PlayerPokemon.GainEVs(faintedPokemon.ScriptablePokemon);
        StartCoroutine(GainExpCoroutine(PlayerPokemon, CalculateExpGained(faintedPokemon)));
    }

    private IEnumerator GainExpCoroutine(Pokemon pokemon, int exp)
    {
        battleDialogueUIManager.OnExpGained(pokemon, exp);

        while (exp > 0 && pokemon.Level < Pokemon.MaxLevel)
        {
            int expBeforeLvUp = GrowthRateDB.ExpBeforeLevelUp(pokemon);
            if (exp >= expBeforeLvUp)
            {
                exp -= expBeforeLvUp;

                pokemon.GainExp(expBeforeLvUp);
                
                battleSceneUIManager.OnExpGained(pokemon, expBeforeLvUp);

                yield return BattleUIManager.Instance.WaitWhileBusy();

                if (pokemon.ShouldLevelUp())
                {
                    yield return LevelUpCoroutine(pokemon);
                }
            }
            else
            {
                pokemon.GainExp(exp);
                battleSceneUIManager.OnExpGained(pokemon, exp);
                exp = 0;
            }
        }
    }

    public IEnumerator LevelUpCoroutine(Pokemon pokemon)
    {
        pokemon.LevelUp();

        BattleEvents.Instance.LevelUp(pokemon);

        // yield return BattleUIManager.Instance.WaitWhileBusy();

        yield return CheckForNewMovesCoroutine(pokemon);

        // TODO
        // Check for evolution
    }

    public IEnumerator CheckForNewMovesCoroutine(Pokemon pokemon)
    {
        foreach (ScriptableMove scriptableMove in pokemon.GetNewMovesAtCurrentLevel())
        {
            if (pokemon.Moves.Count < Pokemon.MaxNumberMoves)
            {
                pokemon.LearnNewMove(scriptableMove);
                BattleEvents.Instance.MoveLearnt(pokemon, null, scriptableMove);
            }
            else
            {
                moveToLearn = scriptableMove;
                BattleDialogueUIManager.OnChooseMoveToForget(pokemon, moveToLearn);
                BattleActionSelectorsUIManager.OnChooseMoveToForget(pokemon, moveToLearn);
                BattleUIManager.Instance.Pause();
            }
            // This is no use ??
            yield return new WaitUntil(() => !BattleUIManager.Instance.IsPaused);
        }
    }

    private void SelectedMoveToForget(int index)
    {
        ScriptableMove oldMove = PlayerPokemon.Moves[index].ScriptableMove;
        PlayerPokemon.ReplaceMove(index, moveToLearn);
        BattleEvents.Instance.MoveLearnt(PlayerPokemon, oldMove, moveToLearn);
        moveToLearn = null;
        BattleUIManager.Instance.Unpause();
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

    private int CalculateExpGained(Pokemon faintedPokemon)
    {
        int exp = Mathf.FloorToInt(faintedPokemon.ScriptablePokemon.ExperienceYield * faintedPokemon.Level / 7);
        float trainerBonus = (IsTrainerBattle) ? 1.5f : 1.0f;
        return Mathf.FloorToInt(exp * trainerBonus);
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
        float criticalHitModifier = criticalHit ? 1.5f : 1;
        bool stab = attackingPokemon.ScriptablePokemon.Type1 == move.ScriptableMove.Type || 
            attackingPokemon.ScriptablePokemon.Type2 == move.ScriptableMove.Type;
        float stabModifier = stab ? 1.5f : 1.0f;
        float typeModifier = TypeUtils.TypeModifier(move.ScriptableMove, targetPokemon.ScriptablePokemon);
        float randomModifier = Random.Range(0.85f, 1.0f);

        float baseDamage = (((2 * (float)level / 5 + 2) * attack * power / defense / 50) + 2);
        float damage = baseDamage * criticalHitModifier * stabModifier * typeModifier * randomModifier;
        damage *= attackModifier.DamageMultiplier;

        AttackInfo attackInfo = new AttackInfo(criticalHit, damage, typeModifier, true);
        
        return attackInfo;
    }
}