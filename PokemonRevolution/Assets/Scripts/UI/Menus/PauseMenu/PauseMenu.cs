using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _pauseMenuNavigator;
    [SerializeField] private UINavigationSelector _pauseMenuNavigationSelector;
    [SerializeField] private RectTransform _selectionIndicator;
    
    private void OnEnable()
    {
        UpdateUI();

        _pauseMenuNavigationSelector.OnSelectionChanged += UpdateUI;
        _pauseMenuNavigator.OnCancelled += GameManager.Instance.ClosePauseMenu;
    }

    private void OnDisable()
    {
        _selectionIndicator.gameObject.SetActive(false);
        
        _pauseMenuNavigationSelector.OnSelectionChanged -= UpdateUI;
        _pauseMenuNavigator.OnCancelled -= GameManager.Instance.ClosePauseMenu;
    }

    private void UpdateUI(int oldSelection, int newSelection)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _selectionIndicator.gameObject.SetActive(true);

        RectTransform currentChildTransform = _pauseMenuNavigationSelector.NavigationItems[_pauseMenuNavigationSelector.CurrentSelection].GetComponent<RectTransform>();
        _selectionIndicator.position = currentChildTransform.TransformPoint(currentChildTransform.rect.center);
        _selectionIndicator.sizeDelta = new Vector2(Mathf.Abs(currentChildTransform.sizeDelta.x), Mathf.Abs(currentChildTransform.sizeDelta.y));
    }
}
