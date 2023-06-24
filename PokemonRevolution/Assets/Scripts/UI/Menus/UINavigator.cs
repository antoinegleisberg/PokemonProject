using System;
using System.Collections.Generic;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    [SerializeField] private RectTransform _selectionIndicator;

    [field: SerializeField] public List<NavigationItem> NavigationItems { get; private set; }
    
    [Header("Navigation Order Overrides")]
    [SerializeField] private List<Pair<UINavigator, UINavigator>> _overrideNavigationOrderRight;
    [SerializeField] private List<Pair<UINavigator, UINavigator>> _overrideNavigationOrderLeft;
    [SerializeField] private List<Pair<UINavigator, UINavigator>> _overrideNavigationOrderUp;
    [SerializeField] private List<Pair<UINavigator, UINavigator>> _overrideNavigationOrderDown;

    
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

    public event Action OnStarted;
    public event Action<Vector2Int> OnNavigationInput;
    public event Action<int> OnNavigated;
    public event Action OnCancelled;
    public event Action<int> OnSubmitted;

    private void Awake()
    {
        // _currentSelection = 0;
    }

    private void OnDisable()
    {
        _selectionIndicator.gameObject.SetActive(false);
    }

    private void Start()
    {
        OnStarted?.Invoke();
    }

    public void OnNavigate(Vector2Int input)
    {
        OnNavigationInput?.Invoke(input);
        HandleNavigation(input);
        OnNavigated?.Invoke(_currentSelection);
    }

    public void OnCancel()
    {
        OnCancelled?.Invoke();
    }
    
    public void OnSubmit()
    {
        OnSubmitted?.Invoke(_currentSelection);
        _currentNavigationItem?.Submit();
    }

    public void UpdateUI()
    {
        UpdateUI(_currentSelection);
    }

    private void HandleNavigation(Vector2Int input)
    {
        int newSelection = _currentSelection + input.x;
        UpdateUI(newSelection);
    }

    private void UpdateUI(int newSelection)
    {
        if (NavigationItems.Count == 0)
        {
            return;
        }

        _selectionIndicator.gameObject.SetActive(true);

        _currentNavigationItem.Unselect();
        _currentSelection = Mathf.Clamp(newSelection, 0, NavigationItems.Count - 1);
        while (_currentSelection >= 0 && !NavigationItems[_currentSelection].IsSelectable)
        {
            _currentSelection -= 1;
        }
        _currentSelection = Mathf.Clamp(_currentSelection, 0, NavigationItems.Count - 1);
        while (_currentSelection < NavigationItems.Count && !NavigationItems[_currentSelection].IsSelectable)
        {
            _currentSelection += 1;
        }
        _currentSelection = Mathf.Clamp(_currentSelection, 0, NavigationItems.Count - 1);
        _currentNavigationItem.Select();

        RectTransform currentChildTransform = _currentNavigationItem.GetComponent<RectTransform>();
        _selectionIndicator.position = currentChildTransform.TransformPoint(currentChildTransform.rect.center);
        _selectionIndicator.sizeDelta = new Vector2(Mathf.Abs(currentChildTransform.sizeDelta.x), Mathf.Abs(currentChildTransform.sizeDelta.y));
    }
}
