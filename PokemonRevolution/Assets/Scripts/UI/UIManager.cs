using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private MenuSelector _menuSelector;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OpenMenu()
    {
        _menuSelector.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        _menuSelector.gameObject.SetActive(false);
    }

    public void HandleUINavigation(Vector2Int input)
    {
        _menuSelector.HandleUINavigate(input);
    }

    public void HandleUISubmit()
    {
        _menuSelector.HandleUISubmit();
    }

    public void HandleUICancel()
    {
        _menuSelector.HandleUICancel();
    }
}
