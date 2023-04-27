using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PokemonBuilder
{
    public ScriptablePokemon scriptablePokemon;
    public int level;
    public string name;
    public PokemonOwner owner;

    public Pokemon BuildPokemon()
    {
        return new Pokemon(scriptablePokemon, level, owner, name);
    }
}
