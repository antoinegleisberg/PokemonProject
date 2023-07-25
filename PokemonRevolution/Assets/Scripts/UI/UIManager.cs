using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private UINavigator _pauseMenu;
    [SerializeField] private UINavigator _partyMenu;
    [SerializeField] private UINavigator _bagMenu;
    [SerializeField] private UINavigator _settingsMenu;

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
    }

    public void ClosePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
    }

    public void OpenPartyMenu()
    {
        OpenPartyMenu(null, null);
    }

    public void OpenPartyMenu(Action<int> onSelected = null, Action onCancelled = null)
    {
        CloseAllMenus();
        _partyMenu.gameObject.SetActive(true);
        _currentMenu = _partyMenu;
        _partyMenu.GetComponent<PartyMenuManager>().OverridePartyScreenCallbacks(onSelected, onCancelled);
    }

    public void ClosePartyMenu()
    {
        CloseAllMenus();
        _partyMenu.gameObject.SetActive(false);
        _partyMenu.GetComponent<PartyMenuManager>().OverridePartyScreenCallbacks(null, null);
    }

    public void OpenMoveSelectionMenu(Action<int> onSelected = null, Action onCancelled = null)
    {
        CloseAllMenus();
        _partyMenu.gameObject.SetActive(true);
        _currentMenu = _partyMenu;
    }

    public void CloseMoveSelectionMenu()
    {
        CloseAllMenus();
        _partyMenu.gameObject.SetActive(false);
    }

    public void OpenBagMenu()
    {
        OpenBagMenu(null, null);
    }

    public void OpenBagMenu(Action<BagCategory, int> onSelected = null, Action onCancelled = null)
    {
        CloseAllMenus();
        _bagMenu.gameObject.SetActive(true);
        _currentMenu = _bagMenu;
        _bagMenu.GetComponent<BagMenu>().OverrideCallbacks(onSelected, onCancelled);
    }
    
    public void CloseBagMenu()
    {
        CloseAllMenus();
        _bagMenu.gameObject.SetActive(false);
        _bagMenu.GetComponent<BagMenu>().OverrideCallbacks(null, null);
    }

    public void OpenSettingsMenu()
    {
        CloseAllMenus();
        _settingsMenu.gameObject.SetActive(true);
        _currentMenu = _settingsMenu;
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
        _settingsMenu.gameObject.SetActive(false);
    }
}
