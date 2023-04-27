using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField] private PokemonPartyManager pokemonPartyManager;

    public PokemonPartyManager PokemonPartyManager { get => pokemonPartyManager; }
}
