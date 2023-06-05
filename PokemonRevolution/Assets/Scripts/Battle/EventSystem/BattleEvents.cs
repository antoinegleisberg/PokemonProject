using System;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Instance;

    private void Awake() => Instance = this;

    public event Action<PokemonParty, PokemonParty> OnBattleStart;
    public event Action OnBattleEnd;

    public event Action OnEnterActionSelection;
    public event Action OnExitActionSelection;
    
    public event Action<Pokemon, Pokemon, Move, AttackInfo> OnPokemonAttack;
    public event Action<Pokemon, int> OnPokemonDamaged;
    public event Action<Pokemon> OnPokemonFainted;
    public event Action<Pokemon> OnAfterPokemonFainted;

    public event Action<Stat, int, Pokemon> OnPokemonStatBoosted;
    public event Action<StatusCondition, Pokemon> OnStatusConditionApplied;
    public event Action<StatusCondition, Pokemon> OnStatusConditionRemoved;
    public event Action<string> OnStatusConditionMessage;

    public event Action<Pokemon> OnPokeballThrown;
    public event Action<Pokemon> OnPokemonEscaped;
    public event Action<Pokemon> OnPokemonCaught;
    public event Action OnAttemptCatchTrainerPokemon;

    public event Action OnReplaceFaintedPokemon;
    public event Action<Pokemon> OnPokemonSwitchedOut;
    public event Action<Pokemon> OnPokemonSwitchedIn;

    public event Action OnTryToRunAway;
    public event Action OnRunAwaySuccess;
    public event Action OnRunAwayFail;
    public event Action OnAttemptRunFromTrainer;

    public event Action<Pokemon, int> OnExpGained;
    public event Action<Pokemon> OnLevelUp;
    public event Action<Pokemon, ScriptableMove> OnChooseMoveToForget;
    public event Action<Pokemon, ScriptableMove, ScriptableMove> OnMoveLearnt;

    public void StartBattle(PokemonParty playerParty, PokemonParty enemyParty) => OnBattleStart?.Invoke(playerParty, enemyParty);
    public void EndBattle() => OnBattleEnd?.Invoke();

    public void EnterActionSelectionState() => OnEnterActionSelection?.Invoke();
    public void ExitActionSelectionState() => OnExitActionSelection?.Invoke();

    public void PokemonAttacks(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo) => OnPokemonAttack?.Invoke(attacker, defender, move, attackInfo);
    public void PokemonDamaged(Pokemon pokemon, int damage) => OnPokemonDamaged?.Invoke(pokemon, damage);
    public void PokemonFaints(Pokemon pokemon) => OnPokemonFainted?.Invoke(pokemon);
    public void AfterPokemonFainted(Pokemon pokemon) => OnAfterPokemonFainted?.Invoke(pokemon);

    public void BoostedPokemonStat(Stat stat, int boost, Pokemon pokemon) => OnPokemonStatBoosted?.Invoke(stat, boost, pokemon);
    public void AppliedStatusCondition(StatusCondition status, Pokemon pokemon) => OnStatusConditionApplied?.Invoke(status, pokemon);
    public void RemovedStatusCondition(StatusCondition status, Pokemon pokemon) => OnStatusConditionRemoved?.Invoke(status, pokemon);
    public void StatusConditionMessage(string message) => OnStatusConditionMessage?.Invoke(message);

    public void ThrowPokeball(Pokemon pokemon) => OnPokeballThrown?.Invoke(pokemon);
    public void PokemonEscapes(Pokemon pokemon) => OnPokemonEscaped?.Invoke(pokemon);
    public void PokemonCaught(Pokemon pokemon) => OnPokemonCaught?.Invoke(pokemon);
    public void AttemptCatchTrainerPokemon() => OnAttemptCatchTrainerPokemon?.Invoke();

    public void ReplaceFaintedPokemon() => OnReplaceFaintedPokemon?.Invoke();
    public void PokemonSwitchedOut(Pokemon pokemon) => OnPokemonSwitchedOut?.Invoke(pokemon);
    public void PokemonSwitchedIn(Pokemon pokemon) => OnPokemonSwitchedIn?.Invoke(pokemon);

    public void TryToRunAway() => OnTryToRunAway?.Invoke();
    public void RunAwaySuccess() => OnRunAwaySuccess?.Invoke();
    public void RunAwayFail() => OnRunAwayFail?.Invoke();
    public void AttemptRunFromTrainer() => OnAttemptRunFromTrainer?.Invoke();

    public void ExpGained(Pokemon pokemon, int exp) => OnExpGained?.Invoke(pokemon, exp);
    public void LevelUp(Pokemon pokemon) => OnLevelUp?.Invoke(pokemon);
    public void ChooseMoveToForget(Pokemon pokemon, ScriptableMove newMove) => OnChooseMoveToForget?.Invoke(pokemon, newMove);
    public void MoveLearnt(Pokemon pokemon, ScriptableMove oldMove, ScriptableMove newMove) => OnMoveLearnt?.Invoke(pokemon, oldMove, newMove);
}
