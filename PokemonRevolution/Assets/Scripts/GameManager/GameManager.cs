using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    
    private GameManagerBaseState currentState;
    public GameManagerFreeRoamState FreeRoamState;
    public GameManagerBattleState BattleState;
    public GameManagerDialogueState DialogueState;
    public GameManagerCutsceneState CutsceneState;
    public GameManagerUINavigationState UINavigationState;

    [SerializeField] private BattleManager battleManager;
    [SerializeField] private PokemonPartyManager playerPartyManager;
    [SerializeField] private Transform player;

    public BattleManager BattleManager { get => battleManager; }
    public Transform PlayerTransform { get => player; }
    public PlayerController PlayerController { get; private set; }
    public MapArea CurrentArea { get; private set; }
    public SceneDetails PreviousScene { get; private set; }
    public SceneDetails CurrentScene { get; private set; }


    private void Awake()
    {
        Instance = this;

        FreeRoamState = new GameManagerFreeRoamState();
        BattleState = new GameManagerBattleState();
        DialogueState = new GameManagerDialogueState();
        CutsceneState = new GameManagerCutsceneState();
        UINavigationState = new GameManagerUINavigationState();

        PlayerController = player.GetComponentInChildren<PlayerController>();

        PokemonsDB.Init();
        MovesDB.Init();
        ConditionsDB.Init();
    }

    private void Start()
    {
        FreeRoamState.InitState(this);
        BattleState.InitState(this);
        DialogueState.InitState(this);
        CutsceneState.InitState(this);
        UINavigationState.InitState(this);

        SceneEvents.Instance.OnCurrentSceneLoaded += UpdateMapArea;

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
        UINavigationState.OnDestroy();

        SceneEvents.Instance.OnCurrentSceneLoaded -= UpdateMapArea;
    }

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



    public void SetCurrentScene(SceneDetails currentScene)
    {
        PreviousScene = CurrentScene;
        CurrentScene = currentScene;
        Debug.Log($"Setting current scene to {CurrentScene.gameObject.name}");
    }

    public void OpenMenu()
    {
        if (currentState == FreeRoamState)
        {
            SwitchState(UINavigationState);
            UIManager.Instance.OpenMenu();
        }
    }

    public void CloseMenu()
    {
        if (currentState == UINavigationState)
        {
            SwitchState(FreeRoamState);
            UIManager.Instance.CloseMenu();
        }
    }

    public void HandleUINavigation(Vector2Int input)
    {
        if (currentState == BattleState)
        {
            BattleManager.BattleActionSelectorsUIManager.HandleUINavigation(input);
        }
        else if (currentState == UINavigationState)
        {
            UIManager.Instance.HandleUINavigation(input);
        }
    }

    public void HandleUISubmit()
    {
        if (currentState == BattleState)
        {
            BattleManager.BattleActionSelectorsUIManager.HandleUISubmit();
        }
        else if (currentState == DialogueState)
        {
            DialogueManager.Instance.HandleUISubmit();
        }
        else if (currentState == UINavigationState)
        {
            UIManager.Instance.HandleUISubmit();
        }
    }

    public void HandleUICancel()
    {
        if (currentState == BattleState)
        {
            BattleManager.BattleActionSelectorsUIManager.HandleUICancel();
        }
        else if (currentState == DialogueState)
        {
            DialogueManager.Instance.HandleUICancel();
        }
        else if (currentState == UINavigationState)
        {
            UIManager.Instance.HandleUICancel();
        }
    }

    private void UpdateMapArea(SceneDetails sceneDetails)
    {
        List<MapArea> loadedMapAreas = FindObjectsOfType<MapArea>().ToList();
        foreach (MapArea mapArea in loadedMapAreas)
        {
            if (mapArea.gameObject.scene.name == CurrentScene.gameObject.name)
            {
                Debug.Log($"Setting current map area to {CurrentScene.gameObject.name}");
                CurrentArea = mapArea;
                break;
            }
        }
    }
}