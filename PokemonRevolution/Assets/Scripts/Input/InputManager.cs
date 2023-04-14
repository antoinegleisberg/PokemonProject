using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    private Vector2Int movementInput;
    public Vector2Int MovementInput { get { return movementInput; } }

    private bool isRunning;
    public bool IsRunning { get { return isRunning; } }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        movementInput = Vector2Int.zero;
        isRunning = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput.x = (int)context.ReadValue<Vector2>().x;
        if (movementInput.x != 0) movementInput.y = 0;
        else movementInput.y = (int)context.ReadValue<Vector2>().y;
    }

    public void OnRunOrCancel(InputAction.CallbackContext context)
    {
        if (context.started) isRunning = true;
        else if (context.canceled) isRunning = false;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("MouseClick");
    }
}
