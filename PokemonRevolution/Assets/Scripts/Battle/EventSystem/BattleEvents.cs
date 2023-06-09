using System;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Instance;

    private void Awake() => Instance = this;
    
    public event Action<Pokemon, Pokemon, Move, AttackInfo> OnPokemonAttack;
    public event Action<Pokemon, int> OnPokemonDamaged;
    public event Action<Pokemon> OnPokemonFainted;

    public event Action<Stat, int, Pokemon> OnPokemonStatBoosted;
    public event Action<StatusCondition, Pokemon> OnStatusConditionApplied;
    public event Action<StatusCondition, Pokemon> OnStatusConditionRemoved;
    public event Action<string> OnStatusConditionMessage;

    public event Action<Pokemon> OnPokeballThrown;
    public event Action<Pokemon> OnPokemonEscaped;
    public event Action<Pokemon> OnPokemonCaught;
    public event Action OnAttemptCatchTrainerPokemon;
    
    public event Action<Pokemon> OnPokemonSwitchedOut;
    public event Action<Pokemon> OnPokemonSwitchedIn;
    
    public event Action<Pokemon> OnLevelUp;
    public event Action<Pokemon, ScriptableMove, ScriptableMove> OnMoveLearnt;

    public void PokemonAttacks(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo) => OnPokemonAttack?.Invoke(attacker, defender, move, attackInfo);
    public void PokemonDamaged(Pokemon pokemon, int damage) => OnPokemonDamaged?.Invoke(pokemon, damage);
    public void PokemonFaints(Pokemon pokemon) => OnPokemonFainted?.Invoke(pokemon);

    public void BoostedPokemonStat(Stat stat, int boost, Pokemon pokemon) => OnPokemonStatBoosted?.Invoke(stat, boost, pokemon);
    public void AppliedStatusCondition(StatusCondition status, Pokemon pokemon) => OnStatusConditionApplied?.Invoke(status, pokemon);
    public void RemovedStatusCondition(StatusCondition status, Pokemon pokemon) => OnStatusConditionRemoved?.Invoke(status, pokemon);
    public void StatusConditionMessage(string message) => OnStatusConditionMessage?.Invoke(message);

    public void ThrowPokeball(Pokemon pokemon) => OnPokeballThrown?.Invoke(pokemon);
    public void PokemonEscapes(Pokemon pokemon) => OnPokemonEscaped?.Invoke(pokemon);
    public void PokemonCaught(Pokemon pokemon) => OnPokemonCaught?.Invoke(pokemon);
    public void AttemptCatchTrainerPokemon() => OnAttemptCatchTrainerPokemon?.Invoke();
    
    public void PokemonSwitchedOut(Pokemon pokemon) => OnPokemonSwitchedOut?.Invoke(pokemon);
    public void PokemonSwitchedIn(Pokemon pokemon) => OnPokemonSwitchedIn?.Invoke(pokemon);
    
    public void LevelUp(Pokemon pokemon) => OnLevelUp?.Invoke(pokemon);
    public void MoveLearnt(Pokemon pokemon, ScriptableMove oldMove, ScriptableMove newMove) => OnMoveLearnt?.Invoke(pokemon, oldMove, newMove);
}
