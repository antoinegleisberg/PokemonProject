using System.Collections.Generic;
using UnityEngine;

public class BagMenuCategoryBar : MonoBehaviour
{
    [SerializeField] UINavigationSelector _navigationSelector;

    [SerializeField] private List<RectTransform> _categoryBarIcons;

    private void OnEnable()
    {
        _navigationSelector.OnSelectionChanged += UpdateSelection;
    }

    private void OnDisable()
    {
        _navigationSelector.OnSelectionChanged -= UpdateSelection;
    }

    private void UpdateSelection(int oldSelection, int newSelection)
    {
        UpdateIconsUI(oldSelection, newSelection);
    }

    private void UpdateIconsUI(int oldSelection, int newSelection)
    {
        Vector2 selectedSize = _categoryBarIcons[oldSelection].sizeDelta;
        Vector2 unselectedSize = _categoryBarIcons[newSelection].sizeDelta;

        _categoryBarIcons[oldSelection].sizeDelta = unselectedSize;

        _categoryBarIcons[newSelection].sizeDelta = selectedSize;
    }
}
