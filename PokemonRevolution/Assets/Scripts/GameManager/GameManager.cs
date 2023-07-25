using System;
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

    private Stack<GameManagerBaseState> _stateStack { get; set; }

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

        _stateStack = new Stack<GameManagerBaseState>();
    }

    private void Start()
    {
        FreeRoamState.InitState(this);
        BattleState.InitState(this);
        DialogueState.InitState(this);
        CutsceneState.InitState(this);
        UINavigationState.InitState(this);

        SceneEvents.Instance.OnCurrentSceneLoaded += UpdateMapArea;

        PushState(FreeRoamState);
    }

    private void Update()
    {
        _currentState.UpdateState();

        DebugCanvas.Instance.Clear();
        DebugCanvas.Instance.WriteText("GM State Stack");
        foreach (GameManagerBaseState state in _stateStack)
        {
            DebugCanvas.Instance.WriteText(state.GetType().ToString());
        }
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

    public void PushState(GameManagerBaseState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _stateStack.Push(_currentState);
        _currentState.EnterState();
    }

    public void PopState()
    {
        _currentState.ExitState();
        _stateStack.Pop();
        _currentState = _stateStack.Peek();
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

        PushState(BattleState);
        BattleManager.StartBattle(PlayerController.PokemonPartyManager.PokemonParty, enemyParty);
    }

    public void SetCurrentScene(SceneDetails currentScene)
    {
        PreviousScene = CurrentScene;
        CurrentScene = currentScene;
        Debug.Log($"Setting current scene to {CurrentScene.gameObject.name}");
    }

    public void OpenPauseMenu()
    {
        if (_currentState == FreeRoamState)
        {
            PushState(UINavigationState);
            UIManager.Instance.OpenPauseMenu();
        }
    }

    public void ClosePauseMenu()
    {
        if (_currentState == UINavigationState)
        {
            PopState();
            UIManager.Instance.ClosePauseMenu();
        }
    }

    public void OpenBagMenu(Action<BagCategory, int> onSelected, Action onCancelled)
    {
        if (_currentState == BattleState)
        {
            PushState(UINavigationState);
            UIManager.Instance.OpenBagMenu(onSelected, onCancelled);
        }
    }
    
    public void CloseBagMenu()
    {
        if (_currentState == UINavigationState)
        {
            PopState();
            UIManager.Instance.CloseBagMenu();
        }
    }

    public void OpenPartyMenu(Action<int> onSelected, Action onCancelled)
    {
        if (_currentState == BattleState)
        {
            PushState(UINavigationState);
            UIManager.Instance.OpenPartyMenu(onSelected, onCancelled);
        }
    }

    public void ClosePartyMenu()
    {
        if (_currentState == UINavigationState)
        {
            PopState();
            UIManager.Instance.ClosePartyMenu();
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