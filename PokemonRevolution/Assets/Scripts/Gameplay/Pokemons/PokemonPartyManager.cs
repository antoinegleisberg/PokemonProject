using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonPartyManager : MonoBehaviour
{
    [SerializeField] private PokemonParty pokemonParty;

    public PokemonParty PokemonParty { get => pokemonParty; }

    private void Awake()
    {
        pokemonParty = new PokemonParty();
    }
}
