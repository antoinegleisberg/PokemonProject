using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    public List<Pokemon> Pokemons;

    public Pokemon GetFirstPokemon()
    {
        foreach (Pokemon pokemon in Pokemons)
        {
            if (!pokemon.IsFainted)
                return pokemon;
        }
        return null;
    }

    private void Awake()
    {
        Pokemons = new List<Pokemon>();
    }
}
