using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectorNavigationManager : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;

    [SerializeField] private List<Pair<Button, Button>> _overrideNavigationOrderRight;
    [SerializeField] private List<Pair<Button, Button>> _overrideNavigationOrderLeft;
    [SerializeField] private List<Pair<Button, Button>> _overrideNavigationOrderUp;
    [SerializeField] private List<Pair<Button, Button>> _overrideNavigationOrderDown;

    private Dictionary<Button, Button> _overrideNavigationOrderRightMapping;
    private Dictionary<Button, Button> _overrideNavigationOrderLeftMapping;
    private Dictionary<Button, Button> _overrideNavigationOrderUpMapping;
    private Dictionary<Button, Button> _overrideNavigationOrderDownMapping;

    [SerializeField] private bool _canCancel;

    private int _currentSelection;
    private RectTransform _selectionIndicator;

    private void Awake()
    {
        _currentSelection = -1;

        _overrideNavigationOrderRightMapping = BuildNavigationMapping(_overrideNavigationOrderRight);
        _overrideNavigationOrderLeftMapping = BuildNavigationMapping(_overrideNavigationOrderLeft);
        _overrideNavigationOrderUpMapping = BuildNavigationMapping(_overrideNavigationOrderUp);
        _overrideNavigationOrderDownMapping = BuildNavigationMapping(_overrideNavigationOrderDown);
    }

    private void OnEnable()
    {
        if (_currentSelection != -1)
        {
            _selectionIndicator.gameObject.SetActive(true);
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        _selectionIndicator.gameObject.SetActive(false);
    }

    public void SetSelectionIndicator(RectTransform selectionIndicator)
    {
        _selectionIndicator = selectionIndicator;
    }

    public void OnFirstSelected()
    {
        
    }

    public void OnNavigate(Vector2Int input)
    {
        if (_currentSelection == -1)
        {
            UpdateSelection(0);
            return;
        }

        Dictionary<Button, Button> activeOverriding = GetActiveOverriding(input);

        if (activeOverriding != null && activeOverriding.ContainsKey(_buttons[_currentSelection]))
        {
            int newSelection = _buttons.IndexOf(activeOverriding[_buttons[_currentSelection]]);
            UpdateSelection(newSelection);
            return;
        }

        int offset = InputToOffset(input);
        UpdateSelection(_currentSelection + offset);
    }

    public void OnSubmit()
    {
        if (_currentSelection == -1)
        {
            UpdateSelection(0);
        }
        else if (_buttons[_currentSelection].interactable)
        {
            _buttons[_currentSelection].onClick.Invoke();
        }
    }

    public void OnCancel()
    {
        if (_canCancel)
        {
            _buttons[_buttons.Count - 1].onClick.Invoke();
        }
    }

    private void UpdateSelection(int newSelection)
    {
        newSelection = Mathf.Clamp(newSelection, 0, _buttons.Count - 1);

        bool selectionIncreased = Mathf.Sign(newSelection - _currentSelection) > 0;
        int offset = selectionIncreased ? 1 : -1;
        while (0 <= newSelection && newSelection < _buttons.Count && !_buttons[newSelection].isActiveAndEnabled)
        {
            newSelection += offset;
        }
        if (newSelection < 0 || newSelection >= _buttons.Count)
        {
            offset = -offset;
            while (0 <= newSelection && newSelection < _buttons.Count && !_buttons[newSelection].isActiveAndEnabled)
            {
                newSelection += offset;
            }
        }

        _currentSelection = newSelection;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!_selectionIndicator.gameObject.activeSelf)
        {
            _selectionIndicator.gameObject.SetActive(true);
        }
        RectTransform currentButtonTranform = _buttons[_currentSelection].GetComponent<RectTransform>();
        _selectionIndicator.position = currentButtonTranform.position;
        _selectionIndicator.sizeDelta = currentButtonTranform.sizeDelta + new Vector2(15, 15);
    }

    private Dictionary<Button, Button> BuildNavigationMapping(List<Pair<Button, Button>> pairs)
    {
        Dictionary<Button, Button> dict = new Dictionary<Button, Button>();
        foreach (Pair<Button, Button> pair in pairs)
        {
            dict.Add(pair.First, pair.Second);
        }
        return dict;
    }

    private int InputToOffset(Vector2Int input)
    {
        switch (input)
        {
            case Vector2Int value when value == Vector2Int.left:
                return -1;
            case Vector2Int value when value == Vector2Int.right:
                return 1;
            case Vector2Int value when value == Vector2Int.up:
                return -2;
            case Vector2Int value when value == Vector2Int.down:
                return 2;
            default:
                return 0;
        }
    }

    private Dictionary<Button, Button> GetActiveOverriding(Vector2Int input)
    {
        switch (input)
        {
            case Vector2Int value when value == Vector2Int.left:
                return _overrideNavigationOrderLeftMapping;
            case Vector2Int value when value == Vector2Int.right:
                return _overrideNavigationOrderRightMapping;
            case Vector2Int value when value == Vector2Int.up:
                return _overrideNavigationOrderUpMapping;
            case Vector2Int value when value == Vector2Int.down:
                return _overrideNavigationOrderDownMapping;
            default:
                return null;
        }
    }
}