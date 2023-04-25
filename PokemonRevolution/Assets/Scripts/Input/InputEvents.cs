using System;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    public static InputEvents Instance;

    private void Awake() { if (Instance == null) Instance = this; }

    public Action<Vector2Int> OnUINavigate;
    public Action OnInteract;
    public Action OnUISubmit;
    public Action OnUICancel;

    public void NavigateUI(Vector2Int direction) => OnUINavigate?.Invoke(direction);
    public void Interact() => OnInteract?.Invoke();
    public void Submit() => OnUISubmit?.Invoke();
    public void Cancel() => OnUICancel?.Invoke();
}
