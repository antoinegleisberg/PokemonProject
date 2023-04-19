using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Current;

    private void Awake() { Current = this; }

    public event Action OnEnterPlayerTurn;
    public event Action OnEnterEnemyTurn;

    public event Action<Pokemon, int> OnPokemonDamaged;
    public event Action<Pokemon, Pokemon, Move, AttackInfo> OnPokemonAttack;
    public event Action<Pokemon> OnPokemonFainted;
    public event Action<Pokemon, Pokemon> OnPokemonSwitched;

    public void EnterPlayerTurn() => OnEnterPlayerTurn?.Invoke();
    public void EnterEnemyTurn() => OnEnterEnemyTurn?.Invoke();
    
    public void PokemonDamaged(Pokemon pokemon, int damage) => OnPokemonDamaged?.Invoke(pokemon, damage);
    public void PokemonAttacks(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo) => OnPokemonAttack?.Invoke(attacker, defender, move, attackInfo);
    public void PokemonFaints(Pokemon pokemon) => OnPokemonFainted?.Invoke(pokemon);
    public void PokemonSwitched(Pokemon oldPokemon, Pokemon newPokemon) => OnPokemonSwitched?.Invoke(oldPokemon, newPokemon);
}
