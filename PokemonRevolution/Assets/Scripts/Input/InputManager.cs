using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }
    
    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    
    public Vector2Int MovementInput { get; private set; }
    public bool IsRunning { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        playerActionMap = inputActions.FindActionMap("Player");
        uiActionMap = inputActions.FindActionMap("UI");
        playerActionMap.Enable();
        uiActionMap.Disable();
    }

    private void Start()
    {
        MovementInput = Vector2Int.zero;
        IsRunning = false;

        GameEvents.Current.OnEnterBattle += OnEnterBattle;
        GameEvents.Current.OnExitBattle += OnExitBattle;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2Int newInput = Vector2Int.zero;
        newInput.x = (int)context.ReadValue<Vector2>().x;
        if (newInput.x != 0) newInput.y = 0;
        else newInput.y = (int)context.ReadValue<Vector2>().y;
        MovementInput = newInput;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started) IsRunning = true;
        else if (context.canceled) IsRunning = false;
    }

    public void OnUINavigate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector2Int input2Int = new Vector2Int((int)input.x, (int)input.y);
        InputEvents.Current.NavigateUI(input2Int);
    }

    private void OnEnterBattle(Pokemon p1, Pokemon p2)
    {
        playerActionMap.Disable();
        uiActionMap.Enable();
    }
    
    private void OnExitBattle()
    {
        playerActionMap.Enable();
        uiActionMap.Disable();
    }

    private void OnDestroy()
    {
        GameEvents.Current.OnEnterBattle -= OnEnterBattle;
        GameEvents.Current.OnExitBattle -= OnExitBattle;
    }
}
