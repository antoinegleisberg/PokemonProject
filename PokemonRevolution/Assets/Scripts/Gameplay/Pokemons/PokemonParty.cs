using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty
{
    private List<Pokemon> pokemons;

    public List<Pokemon> Pokemons { get => pokemons; }

    public PokemonParty() { pokemons = new List<Pokemon>();}

    public PokemonParty(List<Pokemon> pokemons) { this.pokemons = pokemons; }

    public Pokemon GetFirstPokemon()
    {
        foreach (Pokemon pokemon in pokemons)
        {
            if (!pokemon.IsFainted)
                return pokemon;
        }
        return null;
    }
}