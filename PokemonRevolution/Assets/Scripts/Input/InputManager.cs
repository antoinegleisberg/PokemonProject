using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    [SerializeField] private InputActionAsset _inputActions;
    private InputActionMap _playerActionMap;
    private InputActionMap _uiActionMap;
    
    public Vector2Int MovementInput { get; private set; }
    public bool IsRunning { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _playerActionMap = _inputActions.FindActionMap("Player");
        _uiActionMap = _inputActions.FindActionMap("UI");
        _playerActionMap.Enable();
        _uiActionMap.Disable();
    }

    private void Start()
    {
        MovementInput = Vector2Int.zero;
        IsRunning = false;

        GameEvents.Instance.OnEnterBattle += ActivateUIActionMap;
        GameEvents.Instance.OnExitBattle += ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterDialogue += ActivateUIActionMap;
        GameEvents.Instance.OnExitDialogue += ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterMenu += ActivateUIActionMap;
        GameEvents.Instance.OnExitMenu += ActivatePlayerActionMap;
    }
    
    private void OnDestroy()
    {
        GameEvents.Instance.OnEnterBattle -= ActivateUIActionMap;
        GameEvents.Instance.OnExitBattle -= ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterDialogue -= ActivateUIActionMap;
        GameEvents.Instance.OnExitDialogue -= ActivatePlayerActionMap;
        GameEvents.Instance.OnEnterMenu -= ActivateUIActionMap;
        GameEvents.Instance.OnExitMenu -= ActivatePlayerActionMap;
    }

    public void ActivatePlayerActionMap()
    {
        _playerActionMap.Enable();
        _uiActionMap.Disable();
    }

    public void ActivateUIActionMap()
    {
        MovementInput = Vector2Int.zero;
        IsRunning = false;
        _playerActionMap.Disable();
        _uiActionMap.Enable();
    }

    #region PlayerActionMap

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

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.OpenMenu();
        }
    }

    public void OnShortcut(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

    #endregion

    #region UIActionMap

    public void OnUINavigate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector2Int input2Int = new Vector2Int((int)input.x, (int)input.y);
            GameManager.Instance.HandleUINavigation(input2Int);
        }
    }

    public void OnUISubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.HandleUISubmit();
        }
    }

    public void OnUICancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.HandleUICancel();
        }
    }

    #endregion
}
