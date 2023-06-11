using System.Collections.Generic;
using UnityEngine;

public class PokemonPartyManager : MonoBehaviour
{
    [SerializeField] private List<PokemonBuilder> _pokemonBuilders;

    private PokemonParty _pokemonParty;
    
    public PokemonParty PokemonParty { get => _pokemonParty; }

    private void Awake()
    {
        _pokemonParty = new PokemonParty();
    }

    private void Start()
    {
        foreach (PokemonBuilder pokemonBuilder in _pokemonBuilders)
        {
            _pokemonParty.Pokemons.Add(pokemonBuilder.BuildPokemon());
        }
    }
}
