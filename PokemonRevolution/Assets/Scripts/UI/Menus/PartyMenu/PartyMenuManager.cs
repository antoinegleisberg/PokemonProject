using System;
using UnityEngine;

public class PartyMenuManager : MonoBehaviour
{
    [SerializeField] private UINavigator _partyMenuNavigator;
    
    [SerializeField] private UINavigator _partyNavigator;
    [SerializeField] private UINavigator _pokemonNavigator;
    private UINavigator _currentNavigator;
    
    [SerializeField] private PokemonMenu _pokemonMenu;
    [SerializeField] private PartyMenu _partyMenu;
    
    
    private void OnEnable()
    {
        _partyMenuNavigator.OnNavigationInput += OnNavigationInput;
        _partyMenuNavigator.OnSubmitted += OnSubmitted;
        _partyMenuNavigator.OnCancelled += OnCancelled;
        
        OpenPartyScreen();
    }

    private void OnDisable()
    {
        _partyMenuNavigator.OnNavigationInput -= OnNavigationInput;
        _partyMenuNavigator.OnSubmitted -= OnSubmitted;
        _partyMenuNavigator.OnCancelled -= OnCancelled;
    }

    public void OverridePartyScreenCallbacks(Action<int> onSelected, Action onCancelled)
    {
        _partyMenu.OverrideCallbacks(onSelected, onCancelled);
    }

    public void OpenPartyScreen()
    {
        CloseAllScreens();
        _partyNavigator.gameObject.SetActive(true);
        _currentNavigator = _partyNavigator;
    }

    public void OpenPokemonScreen(int pokemonIdx)
    {
        CloseAllScreens();
        _pokemonNavigator.gameObject.SetActive(true);
        _currentNavigator = _pokemonNavigator;
        
        _pokemonMenu.OpenPokemonSummaryScreen(pokemonIdx);
    }

    private void OnNavigationInput(Vector2Int input)
    {
        _currentNavigator.OnNavigate(input);
    }

    private void OnSubmitted()
    {
        _currentNavigator.OnSubmit();
    }

    private void OnCancelled()
    {
        _currentNavigator.OnCancel();
    }

    private void CloseAllScreens()
    {
        _pokemonNavigator.gameObject.SetActive(false);
        _partyNavigator.gameObject.SetActive(false);
    }
}
