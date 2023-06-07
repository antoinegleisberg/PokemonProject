using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PokemonBuilder
{
    public int scriptablePokemonId;
    public int level;
    public string name;
    public PokemonOwner owner;

    public Pokemon BuildPokemon()
    {
        ScriptablePokemon scriptablePokemon = PokemonsDB.GetPokemonById(scriptablePokemonId);
        return new Pokemon(scriptablePokemon, level, owner, name);
    }
}
