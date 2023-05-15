using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField] private PokemonPartyManager pokemonPartyManager;

    public bool CanBattle { get; set; } = true;

    public PokemonPartyManager PokemonPartyManager { get => pokemonPartyManager; }
}
