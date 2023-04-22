using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PokemonPartyManager playerPartyManager;

    // testing
    [SerializeField] private ScriptablePokemon bulbasaurPrefab;
    [SerializeField] private ScriptablePokemon charmanderPrefab;
    [SerializeField] private ScriptablePokemon squirtlePrefab;
    // end testing

    private void Start()
    {
        // testing
        Pokemon playerPokemon1 = new Pokemon(bulbasaurPrefab, 15, PokemonOwner.Player, "Bulb");
        Pokemon playerPokemon2 = new Pokemon(charmanderPrefab, 15, PokemonOwner.Player, "Char");
        Pokemon playerPokemon3 = new Pokemon(squirtlePrefab, 15, PokemonOwner.Player, "Squirt");
        playerPartyManager.PokemonParty.Pokemons.Add(playerPokemon1);
        playerPartyManager.PokemonParty.Pokemons.Add(playerPokemon2);
        playerPartyManager.PokemonParty.Pokemons.Add(playerPokemon3);
        // end testing
    }
}
