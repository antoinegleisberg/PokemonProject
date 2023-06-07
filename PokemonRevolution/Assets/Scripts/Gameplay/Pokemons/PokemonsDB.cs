using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PokemonsDB
{
    private static Dictionary<int, ScriptablePokemon> pokemons;

    public static void Init()
    {
        pokemons = new Dictionary<int, ScriptablePokemon>();

        string basePath = "Pokemons";
        ScriptablePokemon[] scriptablePokemons = Resources.LoadAll<ScriptablePokemon>(basePath);
        foreach (ScriptablePokemon scriptablePokemon in scriptablePokemons)
        {
            pokemons.Add(scriptablePokemon.Id, scriptablePokemon);
        }
    }

    public static ScriptablePokemon GetPokemonById(int id)
    {
        if (!pokemons.ContainsKey(id))
        {
            Debug.LogError($"Didn't find pokemon {id} in resources, or error while loading.");
        }

        ScriptablePokemon scriptablePokemon = pokemons[id];
        return scriptablePokemon;
    }
}
