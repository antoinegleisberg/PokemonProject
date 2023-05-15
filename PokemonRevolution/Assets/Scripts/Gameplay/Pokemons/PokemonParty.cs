using System;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty
{    
    public List<Pokemon> Pokemons { get; private set; }
    public bool IsFull { get { return Pokemons.Count >= 6; } }

    public PokemonParty() : this(new List<Pokemon>()) { }
    public PokemonParty(List<Pokemon> pokemons) { Pokemons = pokemons; }

    public Pokemon GetFirstPokemon()
    {
        foreach (Pokemon pokemon in Pokemons)
        {
            if (!pokemon.IsFainted)
                return pokemon;
        }
        return null;
    }
    
    public void HealAll()
    {
        foreach (Pokemon pokemon in Pokemons)
        {
            pokemon.HealFull();
        }
    }

    public void AddPokemon(Pokemon pokemon)
    {
        Pokemons.Add(pokemon);
    }
}