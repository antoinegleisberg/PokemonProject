using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PokemonParty playerParty;

    // testing
    [SerializeField] private ScriptablePokemon bulbasaurPrefab;
    [SerializeField] private ScriptablePokemon charmanderPrefab;
    [SerializeField] private ScriptablePokemon squirtlePrefab;
    // end testing

    private void Start()
    {
        // testing
        Pokemon playerPokemon1 = new Pokemon(bulbasaurPrefab, 1, PokemonOwner.Player, "Bulb");
        Pokemon playerPokemon2 = new Pokemon(charmanderPrefab, 1, PokemonOwner.Player, "Char");
        Pokemon playerPokemon3 = new Pokemon(squirtlePrefab, 1, PokemonOwner.Player, "Squirt");
        Debug.Log($"Created player party with pokemons" +
            $"\n{playerPokemon1.Name} (level {playerPokemon1.Level})" +
            $"\n{playerPokemon2.Name} (level {playerPokemon2.Level})" +
            $"\n{playerPokemon3.Name} (level {playerPokemon3.Level})");
        playerParty.Pokemons.Add(playerPokemon1);
        playerParty.Pokemons.Add(playerPokemon2);
        playerParty.Pokemons.Add(playerPokemon3);
        Debug.Log($"Player party : {playerParty}" +
            $"\n{playerParty.Pokemons[0].Name} (level {playerParty.Pokemons[0].Level})" +
            $"\n{playerParty.Pokemons[1].Name} (level {playerParty.Pokemons[1].Level})" +
            $"\n{playerParty.Pokemons[2].Name} (level {playerParty.Pokemons[2].Level})");
        // end testing
    }
}
