using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private Pokemon playerPokemon;
    private Pokemon enemyPokemon;

    // for testing only
    [SerializeField] private ScriptablePokemon playerPokemonScriptable;
    [SerializeField] private ScriptablePokemon enemyPokemonScriptable;

    private bool initialized = false;

    private void Update()
    {
        if (!initialized)
        {
            initialized = true;
            InitializeBattle();
        }
        playerPokemon.CurrentHP -= 5;

        GameEvents.Current.PokemonDamaged(playerPokemon, 5);
    }

    private void InitializeBattle()
    {
        // testing only
        playerPokemon = new Pokemon(playerPokemonScriptable, 7, PokemonOwner.Player, "Bla");
        enemyPokemon = new Pokemon(enemyPokemonScriptable, 6, PokemonOwner.Wild);
        // end testing

        GameEvents.Current.EnterBattle(playerPokemon, enemyPokemon);
    }
}