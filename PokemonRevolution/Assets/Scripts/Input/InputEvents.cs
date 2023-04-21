using System;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    public static InputEvents Current;

    private void Awake() { if (Current == null) Current = this; }

    public Action<Vector2Int> OnUINavigate;

    public void NavigateUI(Vector2Int direction)
    {
        OnUINavigate?.Invoke(direction);
    }
}
