using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private GameManagerBaseState currentState;
    public GameManagerFreeRoamState FreeRoamState;
    public GameManagerBattleState BattleState;
    public GameManagerDialogueState DialogueState;

    [SerializeField] private BattleManager battleManager;

    [SerializeField] private PokemonPartyManager playerPartyManager;

    [SerializeField] private LayerMask solidObjectsCollidersLayer;
    [SerializeField] private LayerMask tallGrassLayer;
    [SerializeField] private LayerMask interactableLayer;

    [SerializeField] private MapArea currentArea;

    // testing
    [SerializeField] private ScriptablePokemon bulbasaurPrefab;
    [SerializeField] private ScriptablePokemon charmanderPrefab;
    [SerializeField] private ScriptablePokemon squirtlePrefab;
    // end testing

    public void SwitchState(GameManagerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void CheckForEncounters(Vector3 position)
    {
        if (Physics2D.OverlapCircle(position, 0.2f, tallGrassLayer) != null)
        {
            if (Random.Range(0, 100) < currentArea.EncounterRate)
            {
                Pokemon enemyPokemon = currentArea.GetRandomWildPokemon();
                PokemonParty enemyParty = new PokemonParty(new List<Pokemon>() { enemyPokemon });
                StartBattle(enemyParty);
            }
        }
        
    }

    private void Awake()
    {
        Instance = this;

        FreeRoamState = new GameManagerFreeRoamState();
        BattleState = new GameManagerBattleState();
        DialogueState = new GameManagerDialogueState();
    }

    private void Start()
    {
        FreeRoamState.InitState(this);
        BattleState.InitState(this);
        DialogueState.InitState(this);

        // testing
        Pokemon playerPokemon1 = new Pokemon(bulbasaurPrefab, 15, PokemonOwner.Player, "Bulb");
        Pokemon playerPokemon2 = new Pokemon(charmanderPrefab, 15, PokemonOwner.Player, "Char");
        Pokemon playerPokemon3 = new Pokemon(squirtlePrefab, 15, PokemonOwner.Player, "Squirt");
        playerPartyManager.PokemonParty.Pokemons.Add(playerPokemon1);
        playerPartyManager.PokemonParty.Pokemons.Add(playerPokemon2);
        playerPartyManager.PokemonParty.Pokemons.Add(playerPokemon3);
        // end testing

        currentState = FreeRoamState;
        currentState.EnterState();
    }

    private void OnDestroy()
    {
        FreeRoamState.OnDestroy();
        BattleState.OnDestroy();
        DialogueState.OnDestroy();
    }

    private void StartBattle(PokemonParty enemyParty)
    {
        SwitchState(BattleState);
        battleManager.StartBattle(playerPartyManager.PokemonParty, enemyParty);
    }
}
