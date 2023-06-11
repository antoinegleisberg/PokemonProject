using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private List<MenuItem> _items;
    [SerializeField] private RectTransform _selector;

    private int _currentSelection;


    private void Awake()
    {
        _currentSelection = 0;
        UpdateSelection(0);
    }

    private void OnEnable()
    {
        _selector.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _selector.gameObject.SetActive(false);
    }

    public void HandleUINavigate(Vector2Int input)
    {
        UpdateSelection(_currentSelection - input.y);
    }

    public void HandleUISubmit()
    {
        _items[_currentSelection].Click();
    }

    public void HandleUICancel()
    {
        _items[_items.Count - 1].Click();
    }

    private void UpdateSelection(int newSelection)
    {
        _items[_currentSelection].Unselect();
        _currentSelection = Mathf.Clamp(newSelection, 0, _items.Count - 1);
        _items[_currentSelection].Select();
        RectTransform selectedItem = _items[_currentSelection].GetComponent<RectTransform>();
        _selector.position = selectedItem.position;
        _selector.sizeDelta = selectedItem.sizeDelta;
    }
}
