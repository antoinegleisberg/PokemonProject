using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private List<MenuItem> items;
    [SerializeField] private RectTransform selector;

    private int currentSelection;


    private void Awake()
    {
        currentSelection = 0;
        UpdateSelection(0);
    }

    private void OnEnable()
    {
        selector.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        selector.gameObject.SetActive(false);
    }

    public void HandleUINavigate(Vector2Int input)
    {
        UpdateSelection(currentSelection - input.y);
    }

    public void HandleUISubmit()
    {
        items[currentSelection].Click();
    }

    public void HandleUICancel()
    {
        items[items.Count - 1].Click();
    }

    private void UpdateSelection(int newSelection)
    {
        items[currentSelection].Unselect();
        currentSelection = Mathf.Clamp(newSelection, 0, items.Count - 1);
        items[currentSelection].Select();
        RectTransform selectedItem = items[currentSelection].GetComponent<RectTransform>();
        selector.position = selectedItem.position;
        selector.sizeDelta = selectedItem.sizeDelta;
    }
}
