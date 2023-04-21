using System;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Instance;

    private void Awake() => Instance = this;

    public event Action OnEnterActionSelection;
    public event Action OnExitActionSelection;
    
    public event Action<Pokemon, Pokemon, Move, AttackInfo> OnPokemonAttack;
    public event Action<Pokemon, int> OnPokemonDamaged;
    public event Action<Pokemon> OnPokemonFainted;

    public event Action<Stat, int, Pokemon> OnPokemonStatBoosted;
    public event Action<NonVolatileStatus, Pokemon> OnPokemonNonVolatileStatusAdded;
    public event Action<NonVolatileStatus, Pokemon> OnPokemonNonVolatileStatusRemoved;
    public event Action<VolatileStatus, Pokemon> OnPokemonVolatileStatusAdded;
    public event Action<VolatileStatus, Pokemon> OnPokemonVolatileStatusRemoved;
    
    public event Action OnReplaceFaintedPokemon;
    public event Action<Pokemon> OnPokemonSwitchedOut;
    public event Action<Pokemon> OnPokemonSwitchedIn;

    public void EnterActionSelectionState() => OnEnterActionSelection?.Invoke();
    public void ExitActionSelectionState() => OnExitActionSelection?.Invoke();

    public void PokemonAttacks(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo) => OnPokemonAttack?.Invoke(attacker, defender, move, attackInfo);
    public void PokemonDamaged(Pokemon pokemon, int damage) => OnPokemonDamaged?.Invoke(pokemon, damage);
    public void PokemonFaints(Pokemon pokemon) => OnPokemonFainted?.Invoke(pokemon);

    public void BoostedPokemonStat(Stat stat, int boost, Pokemon pokemon) => OnPokemonStatBoosted?.Invoke(stat, boost, pokemon);
    public void AddedPokemonNonVolatileStatus(NonVolatileStatus status, Pokemon pokemon) => OnPokemonNonVolatileStatusAdded?.Invoke(status, pokemon);
    public void RemovedPokemonNonVolatileStatus(NonVolatileStatus status, Pokemon pokemon) => OnPokemonNonVolatileStatusRemoved?.Invoke(status, pokemon);
    public void AddedPokemonVolatileStatus(VolatileStatus status, Pokemon pokemon) => OnPokemonVolatileStatusAdded?.Invoke(status, pokemon);
    public void RemovedPokemonVolatileStatus(VolatileStatus status, Pokemon pokemon) => OnPokemonVolatileStatusRemoved?.Invoke(status, pokemon);

    public void ReplaceFaintedPokemon() => OnReplaceFaintedPokemon?.Invoke();
    public void PokemonSwitchedOut(Pokemon pokemon) => OnPokemonSwitchedOut?.Invoke(pokemon);
    public void PokemonSwitchedIn(Pokemon pokemon) => OnPokemonSwitchedIn?.Invoke(pokemon);
}
