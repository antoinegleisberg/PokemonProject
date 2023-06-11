using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PokemonBuilder
{
    public int ScriptablePokemonId;
    public int Level;
    public string Name;
    public PokemonOwner Owner;

    public Pokemon BuildPokemon()
    {
        ScriptablePokemon scriptablePokemon = PokemonsDB.GetPokemonById(ScriptablePokemonId);
        return new Pokemon(scriptablePokemon, Level, Owner, Name);
    }
}
