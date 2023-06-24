using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameManagerBaseState _currentState;
    public GameManagerFreeRoamState FreeRoamState;
    public GameManagerBattleState BattleState;
    public GameManagerDialogueState DialogueState;
    public GameManagerCutsceneState CutsceneState;
    public GameManagerUINavigationState UINavigationState;

    [field: SerializeField] public BattleManager BattleManager { get; private set; }
    
    [field: SerializeField] public PlayerController PlayerController { get; private set; }

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

        _currentState = FreeRoamState;
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.UpdateState();
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
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }
    
    public void CheckForNPCs()
    {
        Transform playerTransform = PlayerController.PlayerTransform;

        if (Physics2D.OverlapCircle(playerTransform.position, 0.2f, GameLayers.Instance.FovLayer) != null)
        {
            Fov fov = Physics2D.OverlapCircle(playerTransform.position, 0.2f, GameLayers.Instance.FovLayer).GetComponent<Fov>();
            fov.OnEnterFOV(playerTransform);
        }
    }

    public void StartBattle(PokemonParty enemyParty, Trainer enemyTrainer)
    {
        BattleManager.EnemyTrainer = enemyTrainer;
        BattleManager.IsTrainerBattle = enemyTrainer != null;

        SwitchState(BattleState);
        BattleManager.StartBattle(PlayerController.PokemonPartyManager.PokemonParty, enemyParty);
    }

    public void SetCurrentScene(SceneDetails currentScene)
    {
        PreviousScene = CurrentScene;
        CurrentScene = currentScene;
        Debug.Log($"Setting current scene to {CurrentScene.gameObject.name}");
    }

    public void OpenMenu()
    {
        if (_currentState == FreeRoamState)
        {
            SwitchState(UINavigationState);
            UIManager.Instance.OpenPauseMenu();
        }
    }

    public void CloseMenu()
    {
        if (_currentState == UINavigationState)
        {
            SwitchState(FreeRoamState);
            UIManager.Instance.ClosePauseMenu();
        }
    }

    public void HandleUINavigation(Vector2Int input)
    {
        if (_currentState == BattleState)
        {
            BattleManager.BattleActionSelectorsUIManager.HandleUINavigation(input);
        }
        else if (_currentState == UINavigationState)
        {
            UIManager.Instance.HandleUINavigation(input);
        }
    }

    public void HandleUISubmit()
    {
        if (_currentState == BattleState)
        {
            BattleManager.BattleActionSelectorsUIManager.HandleUISubmit();
        }
        else if (_currentState == DialogueState)
        {
            DialogueManager.Instance.HandleUISubmit();
        }
        else if (_currentState == UINavigationState)
        {
            UIManager.Instance.HandleUISubmit();
        }
    }

    public void HandleUICancel()
    {
        if (_currentState == BattleState)
        {
            BattleManager.BattleActionSelectorsUIManager.HandleUICancel();
        }
        else if (_currentState == DialogueState)
        {
            DialogueManager.Instance.HandleUICancel();
        }
        else if (_currentState == UINavigationState)
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
                CurrentArea = mapArea;
                break;
            }
        }
    }
}