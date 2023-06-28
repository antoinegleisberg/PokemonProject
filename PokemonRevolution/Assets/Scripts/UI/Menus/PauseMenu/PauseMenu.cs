using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _pauseMenuNavigator;
    [SerializeField] private RectTransform _selectionIndicator;

    private void Start()
    {
        _pauseMenuNavigator.OnNavigated += UpdateUI;
        _pauseMenuNavigator.OnCancelled += GameManager.Instance.CloseMenu;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnDisable()
    {
        _selectionIndicator.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _pauseMenuNavigator.OnNavigated -= UpdateUI;
        _pauseMenuNavigator.OnCancelled -= GameManager.Instance.CloseMenu;
    }

    public void UpdateUI(int selection)
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _selectionIndicator.gameObject.SetActive(true);

        RectTransform currentChildTransform = _pauseMenuNavigator.NavigationItems[_pauseMenuNavigator.CurrentSelection].GetComponent<RectTransform>();
        _selectionIndicator.position = currentChildTransform.TransformPoint(currentChildTransform.rect.center);
        _selectionIndicator.sizeDelta = new Vector2(Mathf.Abs(currentChildTransform.sizeDelta.x), Mathf.Abs(currentChildTransform.sizeDelta.y));
    }
}
