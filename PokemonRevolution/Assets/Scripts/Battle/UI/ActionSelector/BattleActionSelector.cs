using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionSelector : MonoBehaviour
{
    [SerializeField] private UINavigationSelector _selector;

    [SerializeField] private RectTransform _selectionIndicator;

    private void OnEnable()
    {
        _selector.OnSelectionChanged += OnSelectionChanged;

        _selectionIndicator.gameObject.SetActive(true);
        UpdateNavigationArrow();
    }

    private void OnDisable()
    {
        _selector.OnSelectionChanged -= OnSelectionChanged;

        _selectionIndicator.gameObject.SetActive(false);
    }

    private void OnSelectionChanged(int oldSelection, int newSelection)
    {
        UpdateNavigationArrow();
    }

    private void UpdateNavigationArrow()
    {
        float targetX = _selector.NavigationItems[_selector.CurrentSelection].GetComponent<RectTransform>().rect.x;
        float targetY = _selector.NavigationItems[_selector.CurrentSelection].GetComponent<RectTransform>().rect.center.y;

        Vector3 targetPos = _selector.NavigationItems[_selector.CurrentSelection].GetComponent<RectTransform>().TransformPoint(new Vector3(targetX, targetY, 0));

        _selectionIndicator.position = new Vector3(targetPos.x, targetPos.y, 0);
        _selectionIndicator.localPosition = new Vector3(_selectionIndicator.localPosition.x, _selectionIndicator.localPosition.y, 0);
    }
}
