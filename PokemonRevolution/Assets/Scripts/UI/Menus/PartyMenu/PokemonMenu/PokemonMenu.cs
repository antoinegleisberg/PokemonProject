using System.Collections.Generic;
using UnityEngine;

public class PokemonMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _navigator;

    [SerializeField] private List<GameObject> _screens;
    private int _currentPokemon;

    [SerializeField] private UINavigator _categoryBarNavigator;
    [SerializeField] private UINavigationSelector _categoryBarSelector;
    [SerializeField] private UINavigator _moveSelectionNavigator;

    [SerializeField] private PartyMenuManager _partyMenuManager;

    [SerializeField] private PartyMenuPokemonOverview _pokemonOverview;
    [SerializeField] private PartyMenuMovesDetailsScreen _movesDetailsScreen;

    [SerializeField] private PartyMenuSummaryScreen _summaryScreen;
    [SerializeField] private PartyMenuStatsScreen _statsScreen;
    [SerializeField] private PartyMenuMovesScreen _movesScreen;

    private GameObject _currentScreen => _screens[_categoryBarSelector.CurrentSelection];
    

    private void OnEnable()
    {
        _navigator.OnNavigationInput += OnNavigated;
        _navigator.OnSubmitted += OnSubmitted;
        _navigator.OnCancelled += OnCancelled;
        
        _categoryBarSelector.OnSelectionChanged += ChangeScreen;

        _pokemonOverview.gameObject.SetActive(true);
        _pokemonOverview.ShowPokemonNavigationArrows(true);

        _categoryBarNavigator.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _navigator.OnNavigationInput -= OnNavigated;
        _navigator.OnSubmitted -= OnSubmitted;
        _navigator.OnCancelled -= OnCancelled;
        _categoryBarSelector.OnSelectionChanged -= ChangeScreen;

        _pokemonOverview.gameObject.SetActive(false);

        _categoryBarNavigator.gameObject.SetActive(false);
    }

    public void OpenPokemonSummaryScreen(int pokemonIdx)
    {
        ChangeScreen(0, 0);
        ChangePokemon(pokemonIdx);
    }

    public void OnNavigated(Vector2Int input)
    {
        if (_pokemonOverview.gameObject.activeSelf)
        {
            _categoryBarNavigator.OnNavigate(input);

            ChangePokemon(_currentPokemon - input.y);
        }
        else
        {
            _moveSelectionNavigator.OnNavigate(input);
            
            _movesDetailsScreen.UpdateUI(input, GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[_currentPokemon].Moves);
        }
        
    }

    private void OnSubmitted()
    {
        if (_currentScreen == _movesScreen.gameObject && _pokemonOverview.gameObject.activeSelf)
        {
            _pokemonOverview.gameObject.SetActive(false);
            _movesDetailsScreen.gameObject.SetActive(true);
            _movesDetailsScreen.UpdateUI(Vector2Int.zero, GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[_currentPokemon].Moves);
            _movesScreen.EnableMoveSelection();
        }
    }

    private void OnCancelled()
    {
        if (_movesDetailsScreen.gameObject.activeSelf)
        {
            _pokemonOverview.gameObject.SetActive(true);
            _movesDetailsScreen.gameObject.SetActive(false);
            _movesScreen.DisableMoveSelection();
        }
        else
        {
            _partyMenuManager.OpenPartyScreen();
        }
    }

    private void ChangeScreen(int oldSelection, int newSelection)
    {
        _screens[oldSelection].SetActive(false);
        _screens[newSelection].SetActive(true);
    }

    private void ChangePokemon(int newPokemonIdx)
    {
        int nPokemons = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons.Count;

        _currentPokemon = Mathf.Clamp(newPokemonIdx, 0, nPokemons - 1);

        _pokemonOverview.UpdateUI(_currentPokemon);

        _summaryScreen.UpdateUI(_currentPokemon);
        _statsScreen.UpdateUI(_currentPokemon);
        _movesScreen.UpdateUI(_currentPokemon);
    }
}
