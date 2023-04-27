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
    [SerializeField] private MapArea currentArea;
    [SerializeField] private Transform player;

    public Transform Player { get => player; }

    public void SwitchState(GameManagerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void CheckForEncounters(Vector3 position)
    {
        if (Physics2D.OverlapCircle(position, 0.2f, GameLayers.Instance.TallGrassLayer) != null)
        {
            if (Random.Range(0, 100) < currentArea.EncounterRate)
            {
                Pokemon enemyPokemon = currentArea.GetRandomWildPokemon();
                PokemonParty enemyParty = new PokemonParty(new List<Pokemon>() { enemyPokemon });
                StartBattle(enemyParty);
            }
        }
    }
    
    public void CheckForNPCs()
    {
        if (Physics2D.OverlapCircle(Player.position, 0.2f, GameLayers.Instance.FovLayer) != null)
        {
            Fov fov = Physics2D.OverlapCircle(Player.position, 0.2f, GameLayers.Instance.FovLayer).GetComponentInParent<Transform>().parent.GetComponentInChildren<Fov>();
            fov.OnEnterFOV(Player);
        }
    }

    public void TriggerTrainerBattle(PokemonPartyManager enemyPartyManager)
    {
        PokemonParty enemyParty = enemyPartyManager.PokemonParty;
        StartBattle(enemyParty);
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
