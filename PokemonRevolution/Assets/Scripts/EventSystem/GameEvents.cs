using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    private void Awake() => Instance = this;

    public event Action<PokemonParty, PokemonParty> OnPokemonEncounter;
    public event Action<Pokemon, Pokemon> OnEnterBattle;
    public event Action OnExitBattle;

    public void EnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon) => OnEnterBattle?.Invoke(playerPokemon, enemyPokemon);
    public void ExitBattle() => OnExitBattle?.Invoke();
    public void EncounterPokemon(PokemonParty playerParty, PokemonParty enemyParty) => OnPokemonEncounter?.Invoke(playerParty, enemyParty);
}
