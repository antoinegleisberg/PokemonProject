using System;
using System.Collections.Generic;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    [SerializeField] private NavigationMode _navigationMode;
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;

    [field: SerializeField] public List<NavigationItem> NavigationItems { get; private set; }
    
    [SerializeField] private List<Triplet<Direction, UINavigator, UINavigator>> _overrideNavigationOrder;

    private int _currentSelection = 0;
    
    private NavigationItem _currentNavigationItem {
        get
        {
            if (NavigationItems.Count == 0)
                return null;
            return NavigationItems[_currentSelection]; 
        }
    }

    public int CurrentSelection => _currentSelection;

    public event Action<Vector2Int> OnNavigationInput;
    public event Action<int> OnNavigated;
    public event Action OnCancelled;
    public event Action<int> OnSubmitted;

    private void Start()
    {
        _currentNavigationItem?.Select();
    }

    public void OnNavigate(Vector2Int input)
    {
        OnNavigationInput?.Invoke(input);
        UpdateSelection(input);
        OnNavigated?.Invoke(_currentSelection);
    }

    public void OnCancel()
    {
        OnCancelled?.Invoke();
    }
    
    public void OnSubmit()
    {
        // Technically redundant, but sometimes one is more practical than the other
        OnSubmitted?.Invoke(_currentSelection);
        _currentNavigationItem?.Submit();
    }

    public void UpdateUI()
    {
        UpdateUI(_currentSelection);
    }
    
    private void UpdateSelection(Vector2Int input)
    {
        if (NavigationItems.Count == 0)
        {
            return;
        }

        int newSelection = 0;
        if (_navigationMode == NavigationMode.Horizontal)
        {
            newSelection = _currentSelection + input.x;
        }
        else if (_navigationMode == NavigationMode.Vertical)
        {
            newSelection = _currentSelection - input.y;
        }
        else if (_navigationMode == NavigationMode.GridHorizontal)
        {
            newSelection = _currentSelection + input.x - _gridWidth * input.y;
        }
        else if (_navigationMode == NavigationMode.GridVertical)
        {
            newSelection = _currentSelection + _gridHeight * input.x - input.y;
        }
        
        newSelection = Mathf.Clamp(newSelection, 0, NavigationItems.Count - 1);
        while (newSelection >= 0 && !NavigationItems[newSelection].IsSelectable)
        {
            newSelection -= 1;
        }
        newSelection = Mathf.Clamp(newSelection, 0, NavigationItems.Count - 1);
        while (newSelection < NavigationItems.Count && !NavigationItems[newSelection].IsSelectable)
        {
            newSelection += 1;
        }
        newSelection = Mathf.Clamp(newSelection, 0, NavigationItems.Count - 1);

        UpdateUI(newSelection);
    }

    private void UpdateUI(int newSelection)
    {
        _currentNavigationItem.Unselect();
        _currentSelection = newSelection;
        _currentNavigationItem.Select();
    }
}

public enum NavigationMode {
    Horizontal,
    Vertical,
    GridHorizontal,
    GridVertical
}