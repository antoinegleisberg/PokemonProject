using System;
using UnityEngine;

public class NavigationItem : MonoBehaviour
{
    public event Action OnSelected;
    public event Action OnUnselected;
    public event Action OnSubmitted;

    public bool IsSelectable { get; set; } = true;

    public void Select()
    {
        OnSelected?.Invoke();
    }

    public void Unselect()
    {
        OnUnselected?.Invoke();
    }

    public void Submit()
    {
        OnSubmitted?.Invoke();
    }
}
