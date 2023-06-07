using System.Collections.Generic;
using UnityEngine;

public class PokemonPartyManager : MonoBehaviour
{
    [SerializeField] private List<PokemonBuilder> pokemonBuilders;

    private PokemonParty pokemonParty;
    
    public PokemonParty PokemonParty { get => pokemonParty; }

    private void Awake()
    {
        pokemonParty = new PokemonParty();
    }

    private void Start()
    {
        foreach (PokemonBuilder pokemonBuilder in pokemonBuilders)
        {
            pokemonParty.Pokemons.Add(pokemonBuilder.BuildPokemon());
        }
    }
}
