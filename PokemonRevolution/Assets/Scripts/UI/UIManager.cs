using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private UINavigator _pauseMenu;
    [SerializeField] private UINavigator _partyMenu;
    [SerializeField] private UINavigator _partyMenuPokemonSelector;
    [SerializeField] private UINavigator _bagMenu;
    [SerializeField] private UINavigator _partyActionSelectionMenu;

    private UINavigator _currentMenu;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OpenPauseMenu()
    {
        CloseAllMenus();
        _pauseMenu.gameObject.SetActive(true);
        _currentMenu = _pauseMenu;
        _currentMenu.UpdateUI();
    }

    public void ClosePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
    }

    public void OpenPartyMenu()
    {
        CloseAllMenus();
        _partyMenu.gameObject.SetActive(true);
        _currentMenu = _partyMenu;
        _currentMenu.UpdateUI();
    }

    public void OpenBagMenu()
    {
        CloseAllMenus();
        _bagMenu.gameObject.SetActive(true);
        _currentMenu = _bagMenu;
        _currentMenu.UpdateUI();
    }

    public void OpenPartyMenuPokemonSelector()
    {
        CloseAllMenus();
        _partyMenu.gameObject.SetActive(true);
        _currentMenu = _partyMenuPokemonSelector;
        _currentMenu.UpdateUI();
    }

    public void OpenPartyMenuActionSelector(Action<int> onSelected, Action onCancelled)
    {
        CloseAllMenus();
        _partyActionSelectionMenu.gameObject.SetActive(true);
        _currentMenu = _partyActionSelectionMenu;
        _currentMenu.GetComponent<PartyActionSelectionUI>().SetCallbacks(onSelected, onCancelled);
        _currentMenu.UpdateUI();
    }

    public void HandleUINavigation(Vector2Int input)
    {
        _currentMenu.OnNavigate(input);
    }

    public void HandleUISubmit()
    {
        _currentMenu.OnSubmit();
    }

    public void HandleUICancel()
    {
        _currentMenu.OnCancel();
    }

    private void CloseAllMenus()
    {
        _pauseMenu.gameObject.SetActive(false);
        _partyMenu.gameObject.SetActive(false);
        _bagMenu.gameObject.SetActive(false);
        _partyActionSelectionMenu.gameObject.SetActive(false);
    }
}
