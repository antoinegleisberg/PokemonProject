using System;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    public event Action<Vector2Int> OnNavigationInput;
    public event Action OnCancelled;
    public event Action OnSubmitted;

    public void OnNavigate(Vector2Int input)
    {
        OnNavigationInput?.Invoke(input);
    }

    public void OnSubmit()
    {
        OnSubmitted?.Invoke();
    }
    
    public void OnCancel()
    {
        OnCancelled?.Invoke();
    }
}