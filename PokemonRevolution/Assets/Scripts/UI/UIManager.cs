using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private MenuSelector menuSelector;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OpenMenu()
    {
        menuSelector.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        menuSelector.gameObject.SetActive(false);
    }

    public void HandleUINavigation(Vector2Int input)
    {
        menuSelector.HandleUINavigate(input);
    }

    public void HandleUISubmit()
    {
        menuSelector.HandleUISubmit();
    }

    public void HandleUICancel()
    {
        menuSelector.HandleUICancel();
    }
}
