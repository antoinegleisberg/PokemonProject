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

        GameEvents.Instance.OnEnterBattle += ActivateUIActionMap;
        GameEvents.Instance.OnExitBattle += ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterDialogue += ActivateUIActionMap;
        GameEvents.Instance.OnExitDialogue += ActivatePlayerActionMap;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2Int newInput = Vector2Int.zero;
        newInput.x = (int)context.ReadValue<Vector2>().x;
        if (newInput.x != 0) newInput.y = 0;
        else newInput.y = (int)context.ReadValue<Vector2>().y;
        MovementInput = newInput;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputEvents.Instance.Interact();
        }
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
        InputEvents.Instance.NavigateUI(input2Int);
    }

    public void OnUISubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
            InputEvents.Instance.Submit();
    }

    public void OnUICancel(InputAction.CallbackContext context)
    {
        if (context.performed)
            InputEvents.Instance.Cancel();
    }

    private void ActivatePlayerActionMap()
    {
        playerActionMap.Enable();
        uiActionMap.Disable();
    }
    
    private void ActivateUIActionMap()
    {
        playerActionMap.Disable();
        uiActionMap.Enable();
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnEnterBattle -= ActivateUIActionMap;
        GameEvents.Instance.OnExitBattle -= ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterDialogue -= ActivateUIActionMap;
        GameEvents.Instance.OnExitDialogue -= ActivatePlayerActionMap;
    }
}
