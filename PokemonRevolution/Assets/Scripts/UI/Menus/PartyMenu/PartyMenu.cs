using System;
using UnityEngine;

public class PartyMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _partyMenuNavigator;
    [SerializeField] private PokemonMenu _pokemonMenu;

    [SerializeField] private GameObject _upArrow;
    [SerializeField] private GameObject _downArrow;

    [SerializeField] private GameObject _categoryBar;
    [SerializeField] private PartyMenuPartyScreen _partyScreen;
    [SerializeField] private PartyMenuOverview _pokemonOverview;

    
    private Action<int> _onSelectedOverride;
    private Action _onCancelledOverride;
    

    private void Awake()
    {
        _partyMenuNavigator.OnNavigationInput += OnNavigationInput;
        _partyMenuNavigator.OnNavigated += OnNavigated;
        _partyMenuNavigator.OnSubmitted += OnSubmitted;
        _partyMenuNavigator.OnCancelled += OnCancelled;
    }

    private void OnEnable()
    {
        OpenPartyScreen();
    }

    private void OnDestroy()
    {
        _partyMenuNavigator.OnNavigationInput -= OnNavigationInput;
        _partyMenuNavigator.OnNavigated -= OnNavigated;
        _partyMenuNavigator.OnSubmitted -= OnSubmitted;
        _partyMenuNavigator.OnCancelled -= OnCancelled;
    }

    public void OverrideCallbacks(Action<int> onSelected, Action onCancelled)
    {
        _onSelectedOverride = onSelected;
        _onCancelledOverride = onCancelled;
    }

    public void OpenPartyScreen()
    {
        _partyScreen.gameObject.SetActive(true);
        _categoryBar.SetActive(false);
        _pokemonOverview.gameObject.SetActive(true);
        _pokemonMenu.CloseScreens();
        ShowPokemonNavigationArrows(false);

        UpdatePartyScreenUI();
    }
    
    public void ClosePartyScreen()
    {
        _partyScreen.gameObject.SetActive(false);
        _categoryBar.SetActive(false);
    }

    public void OpenSummaryScreen(int pokemonIdx)
    {
        _categoryBar.SetActive(true);
        _pokemonMenu.OpenPokemonSummaryScreen(pokemonIdx);
        ShowPokemonNavigationArrows(true);
    }

    public void CloseSummaryScreen()
    {
        _categoryBar.SetActive(false);
        _pokemonMenu.CloseScreens();
    }

    private void OnNavigationInput(Vector2Int input)
    {
        if (!_partyScreen.gameObject.activeSelf)
        {
            _pokemonMenu.OnNavigated(input);
        }
    }

    private void OnNavigated(int selection)
    {
        if (_partyScreen.gameObject.activeSelf)
        {
            _partyScreen.UpdateSelection(selection);
            _pokemonOverview.UpdateUI(_partyMenuNavigator.CurrentSelection);
        }
    }
    
    private void OnSubmitted(int selection)
    {
        if (_partyScreen.gameObject.activeSelf)
        {
            if (_onSelectedOverride != null)
            {
                _onSelectedOverride.Invoke(selection);
                UpdatePartyScreenUI();
            }
            else
            {
                ClosePartyScreen();
                OpenSummaryScreen(selection);
            }
        }
        else
        {
            _pokemonMenu.OnSubmitted(selection);
        }
    }

    private void OnCancelled()
    {
        if (_partyScreen.gameObject.activeSelf)
        {
            if (_onCancelledOverride != null)
            {
                _onCancelledOverride.Invoke();
            }
            else
            {
                ClosePartyScreen();
                UIManager.Instance.OpenPauseMenu();
            }
        }
        else
        {
            _pokemonMenu.OnCancelled();
        }
    }

    private void UpdatePartyScreenUI()
    {
        _partyScreen.UpdateSelection(_partyMenuNavigator.CurrentSelection);
        _pokemonOverview.UpdateUI(_partyMenuNavigator.CurrentSelection);
        _partyMenuNavigator.UpdateUI();
    }

    private void ShowPokemonNavigationArrows(bool show)
    {
        _upArrow.SetActive(show);
        _downArrow.SetActive(show);
    }
}
