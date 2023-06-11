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

    private BattleManagerBaseState _currentState;
    public BattleManagerOutOfBattleState OutOfBattleState;
    public BattleManagerStartBattleState StartBattleState;
    public BattleManagerStartTurnState StartTurnState;
    public BattleManagerActionSelectionState ActionSelectionState;
    public BattleManagerPerformMovesState PerformMovesState;
    public BattleManagerEndTurnState EndTurnState;
    public BattleManagerEndBattleState EndBattleState;

    [SerializeField] private BattleSceneUIManager _battleSceneUIManager;
    [SerializeField] private BattleDialogueUIManager _battleDialogueUIManager;
    [SerializeField] private BattleActionSelectorsUIManager _battleActionSelectorsUIManager;

    public BattleSceneUIManager BattleSceneUIManager { get => _battleSceneUIManager; }
    public BattleDialogueUIManager BattleDialogueUIManager { get => _battleDialogueUIManager; }
    public BattleActionSelectorsUIManager BattleActionSelectorsUIManager { get => _battleActionSelectorsUIManager; }

    
    private ScriptableMove _moveToLearn;


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

        _currentState = OutOfBattleState;

        BattleEvents.Instance.OnPokemonFainted += OnPokemonFainted;
        BattleUIEvents.Instance.OnSelectMoveToForget += SelectedMoveToForget;
    }

    private void Update()
    {
        _currentState.UpdateState();
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
        ConditionAttackModifier attackModifier = attackingPokemon.GetStatusAttackModifier(move);
        if (!attackModifier.CanPerformMove)
            return;
        
        attackingPokemon.LoseMovePP(move);
        
        AttackInfo attackInfo = CalculateMoveInfo(attackingPokemon, targetPokemon, move, attackModifier);
        
        BattleEvents.Instance.PokemonAttacks(attackingPokemon, targetPokemon, move, attackInfo);
        
        if (!attackInfo.MoveHits)
            return;
        
        if (move.ScriptableMove.Category != MoveCategory.Status)
        {
            DamagePokemon(targetPokemon, attackInfo.Damage);
        }
        
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
        _currentState.ExitState();
        _currentState = newState;

        yield return BattleUIManager.Instance.WaitWhileBusy();

        _currentState.EnterState();
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
        _battleDialogueUIManager.OnExpGained(pokemon, exp);

        while (exp > 0 && pokemon.Level < Pokemon.MaxLevel)
        {
            int expBeforeLvUp = GrowthRateDB.ExpBeforeLevelUp(pokemon);
            if (exp >= expBeforeLvUp)
            {
                exp -= expBeforeLvUp;

                pokemon.GainExp(expBeforeLvUp);
                
                _battleSceneUIManager.OnExpGained(pokemon, expBeforeLvUp);

                yield return BattleUIManager.Instance.WaitWhileBusy();

                if (pokemon.ShouldLevelUp())
                {
                    yield return LevelUpCoroutine(pokemon);
                }
            }
            else
            {
                pokemon.GainExp(exp);
                _battleSceneUIManager.OnExpGained(pokemon, exp);
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
                _moveToLearn = scriptableMove;
                BattleDialogueUIManager.OnChooseMoveToForget(pokemon, _moveToLearn);
                BattleActionSelectorsUIManager.OnChooseMoveToForget(pokemon, _moveToLearn);
                BattleUIManager.Instance.Pause();
            }
            
            yield return new WaitUntil(() => !BattleUIManager.Instance.IsPaused);
        }
        yield return new WaitForSeconds(0.5f);
    }

    private void SelectedMoveToForget(int index)
    {
        ScriptableMove oldMove = PlayerPokemon.Moves[index].ScriptableMove;
        PlayerPokemon.ReplaceMove(index, _moveToLearn);
        BattleEvents.Instance.MoveLearnt(PlayerPokemon, oldMove, _moveToLearn);
        _moveToLearn = null;
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
        attackInfo.MoveHits = MoveHits(attackingPokemon, targetPokemon, move);
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