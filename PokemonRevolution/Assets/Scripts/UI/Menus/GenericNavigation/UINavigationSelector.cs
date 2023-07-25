using System;
using System.Collections.Generic;
using UnityEngine;

public class UINavigationSelector : MonoBehaviour
{
    [SerializeField] private UINavigator _uiNavigator;

    [SerializeField] private NavigationMode _navigationMode;
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;

    [field: SerializeField] public List<NavigationItem> NavigationItems { get; private set; }

    [SerializeField] private List<Triplet<Direction, NavigationItem, NavigationItem>> _overrideNavigationOrder;

    [SerializeField] private bool _canLoop;
    [SerializeField] private bool _resetSelectionOnEnable;

    private int _currentSelection = 0;

    public event Action<int, int> OnSelectionChanged;
    public event Action<int> OnSubmitted;

    private NavigationItem _currentNavigationItem {
        get
        {
            if (NavigationItems.Count == 0)
                return null;
            return NavigationItems[_currentSelection];
        }
    }

    private int _nSelectableNavigationItems {
        get
        {
            int count = 0;
            foreach (NavigationItem item in NavigationItems)
            {
                if (item.IsSelectable)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public int CurrentSelection => _currentSelection;

    private void OnEnable()
    {
        _uiNavigator.OnNavigationInput += OnNavigatedInput;
        _uiNavigator.OnSubmitted += OnSubmit;

        if (_resetSelectionOnEnable)
        {
            int oldSelection = _currentSelection;
            UpdateUI(0);
            OnSelectionChanged?.Invoke(oldSelection, 0);
        }
    }

    private void Start()
    {
        // Not in OnEnable function in case the
        // NavigationItems list is dynamically changed on enabling
        _currentNavigationItem?.Select();
    }

    private void OnDisable()
    {
        _uiNavigator.OnNavigationInput -= OnNavigatedInput;
        _uiNavigator.OnSubmitted -= OnSubmit;
    }

    private void OnNavigatedInput(Vector2Int input)
    {
        int oldSelection = _currentSelection;
        UpdateSelection(input);
        OnSelectionChanged?.Invoke(oldSelection, _currentSelection);
    }

    private void OnSubmit()
    {
        _currentNavigationItem?.Submit();
        OnSubmitted?.Invoke(_currentSelection);
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

        if (_canLoop)
        {
            newSelection = (newSelection + _nSelectableNavigationItems) % _nSelectableNavigationItems;
        }
        else
        {
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
        }
        

        UpdateUI(newSelection);
    }

    private void UpdateUI(int newSelection)
    {
        _currentSelection = Mathf.Clamp(_currentSelection, 0, NavigationItems.Count - 1);
        _currentNavigationItem?.Unselect();
        _currentSelection = Mathf.Clamp(newSelection, 0, NavigationItems.Count - 1);
        _currentNavigationItem?.Select();
    }
}

public enum NavigationMode
{
    Horizontal,
    Vertical,
    GridHorizontal,
    GridVertical
}