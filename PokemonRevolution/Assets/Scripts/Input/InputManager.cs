using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    
    public Vector2Int MovementInput { get; private set; }
    public bool IsRunning { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
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
        GameEvents.Instance.OnEnterNpcFov += ActivateUIActionMap;
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
            GameManager.Instance.PlayerController.CheckForInteraction();
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
        UIManager.Instance.HandleUINavigation(input2Int);
    }

    public void OnUISubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
            UIManager.Instance.HandleUISubmit();
    }

    public void OnUICancel(InputAction.CallbackContext context)
    {
        if (context.performed)
            UIManager.Instance.HandleUICancel();
    }

    private void ActivatePlayerActionMap()
    {
        playerActionMap.Enable();
        uiActionMap.Disable();
    }
    
    private void ActivateUIActionMap()
    {
        MovementInput = Vector2Int.zero;
        IsRunning = false;
        playerActionMap.Disable();
        uiActionMap.Enable();
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnEnterBattle -= ActivateUIActionMap;
        GameEvents.Instance.OnExitBattle -= ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterDialogue -= ActivateUIActionMap;
        GameEvents.Instance.OnExitDialogue -= ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterNpcFov -= ActivateUIActionMap;
    }
}
