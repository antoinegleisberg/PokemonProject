using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Instance;

    private void Awake() => Instance = this;

    public event Action OnEnterActionSelection;

    public event Action<Pokemon, Pokemon, Move, AttackInfo> OnPokemonAttack;
    public event Action<Pokemon, int> OnPokemonDamaged;
    public event Action<Pokemon> OnPokemonFainted;
    public event Action OnReplaceFaintedPokemon;
    public event Action<Pokemon> OnPokemonSwitchedOut;
    public event Action<Pokemon> OnPokemonSwitchedIn;

    public void EnterActionSelectionState() => OnEnterActionSelection?.Invoke();

    public void PokemonAttacks(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo) => OnPokemonAttack?.Invoke(attacker, defender, move, attackInfo);
    public void PokemonDamaged(Pokemon pokemon, int damage) => OnPokemonDamaged?.Invoke(pokemon, damage);
    public void PokemonFaints(Pokemon pokemon) => OnPokemonFainted?.Invoke(pokemon);
    public void ReplaceFaintedPokemon() => OnReplaceFaintedPokemon?.Invoke();
    public void PokemonSwitchedOut(Pokemon pokemon) => OnPokemonSwitchedOut?.Invoke(pokemon);
    public void PokemonSwitchedIn(Pokemon pokemon) => OnPokemonSwitchedIn?.Invoke(pokemon);
}
