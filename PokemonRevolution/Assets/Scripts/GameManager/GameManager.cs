using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    
    private GameManagerBaseState currentState;
    public GameManagerFreeRoamState FreeRoamState;
    public GameManagerBattleState BattleState;
    public GameManagerDialogueState DialogueState;
    public GameManagerCutsceneState CutsceneState;

    [SerializeField] private BattleManager battleManager;
    [SerializeField] private PokemonPartyManager playerPartyManager;
    [SerializeField] private MapArea currentArea;
    [SerializeField] private Transform player;

    public Transform PlayerTransform { get => player; }
    public PlayerController PlayerController { get; private set; }
    public MapArea CurrentArea { get => currentArea; }


    public void SwitchState(GameManagerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    
    public void CheckForNPCs()
    {
        if (Physics2D.OverlapCircle(PlayerTransform.position, 0.2f, GameLayers.Instance.FovLayer) != null)
        {
            Fov fov = Physics2D.OverlapCircle(PlayerTransform.position, 0.2f, GameLayers.Instance.FovLayer).GetComponent<Fov>();
            fov.OnEnterFOV(PlayerTransform);
        }
    }

    public void StartBattle(PokemonParty enemyParty, Trainer enemyTrainer)
    {
        battleManager.EnemyTrainer = enemyTrainer;
        battleManager.IsTrainerBattle = enemyTrainer != null;

        SwitchState(BattleState);
        battleManager.StartBattle(playerPartyManager.PokemonParty, enemyParty);
    }

    private void Awake()
    {
        Instance = this;

        FreeRoamState = new GameManagerFreeRoamState();
        BattleState = new GameManagerBattleState();
        DialogueState = new GameManagerDialogueState();
        CutsceneState = new GameManagerCutsceneState();

        PlayerController = player.GetComponentInChildren<PlayerController>();
    }

    private void Start()
    {
        FreeRoamState.InitState(this);
        BattleState.InitState(this);
        DialogueState.InitState(this);
        CutsceneState.InitState(this);

        currentState = FreeRoamState;
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    private void OnDestroy()
    {
        FreeRoamState.OnDestroy();
        BattleState.OnDestroy();
        DialogueState.OnDestroy();
        CutsceneState.OnDestroy();
    }
}