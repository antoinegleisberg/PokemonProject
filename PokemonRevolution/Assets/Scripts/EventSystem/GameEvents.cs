using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Current;

    private void Awake()
    {
        Current = this;
    }

    public event Action<Pokemon, Pokemon> OnEnterBattle;
    public event Action<Pokemon, int> OnPokemonDamaged;

    public void EnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        if (OnEnterBattle != null)
        {
            OnEnterBattle(playerPokemon, enemyPokemon);
        }
    }

    public void PokemonDamaged(Pokemon pokemon, int damage)
    {
        if (OnPokemonDamaged != null)
        {
            OnPokemonDamaged(pokemon, damage);
        }
    }
}
