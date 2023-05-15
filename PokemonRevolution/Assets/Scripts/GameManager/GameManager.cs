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
                battleManager.EnemyTrainer = null;
                battleManager.IsTrainerBattle = false;
                StartBattle(enemyParty);
            }
        }
    }
    
    public void CheckForNPCs()
    {
        if (Physics2D.OverlapCircle(PlayerTransform.position, 0.2f, GameLayers.Instance.FovLayer) != null)
        {
            Fov fov = Physics2D.OverlapCircle(PlayerTransform.position, 0.2f, GameLayers.Instance.FovLayer).GetComponentInParent<Transform>().parent.GetComponentInChildren<Fov>();
            fov.OnEnterFOV(PlayerTransform);
        }
    }

    public void TriggerTrainerBattle(Trainer enemyTrainer)
    {
        PokemonParty enemyParty = enemyTrainer.PokemonPartyManager.PokemonParty;
        battleManager.EnemyTrainer = enemyTrainer;
        battleManager.IsTrainerBattle = true;
        StartBattle(enemyParty);
    }

    private void StartBattle(PokemonParty enemyParty)
    {
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