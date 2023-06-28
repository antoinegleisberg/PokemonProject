using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> _screens;
    [SerializeField] private List<RectTransform> _categoryBarIcons;
    private int _currentSelection;
    private int _currentPokemon;

    [SerializeField] private PartyMenu _partyMenu;

    [SerializeField] private PartyMenuOverview _pokemonOverview;
    [SerializeField] private PartyMenuMovesDetailsScreen _movesDetailsScreen;

    [SerializeField] private PartyMenuSummaryScreen _summaryScreen;
    [SerializeField] private PartyMenuStatsScreen _statsScreen;
    [SerializeField] private PartyMenuMovesScreen _movesScreen;

    private GameObject _currentScreen {
        get => _screens[_currentSelection];
    }

    private int _nScreens {
        get => _screens.Count;
    }

    public void OpenPokemonSummaryScreen(int pokemonIdx)
    {
        ChangeSelection(0);
        ChangePokemon(pokemonIdx);
    }

    public void OnNavigated(Vector2Int input)
    {
        if (input.x != 0)
        {
            if (_pokemonOverview.gameObject.activeSelf)
            {
                ChangeSelection(_currentSelection + input.x);
            }
        }
        if (input.y != 0)
        {
            if (_pokemonOverview.gameObject.activeSelf)
            {
                ChangePokemon(_currentPokemon - input.y);
            }
            else
            {
                _movesScreen.UnselectMoveUI(_movesDetailsScreen.DisplayedMove);
                _movesDetailsScreen.UpdateUI(input, GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[_currentPokemon].Moves);
                _movesScreen.SelectMoveUI(_movesDetailsScreen.DisplayedMove);
            }
        }
    }

    public void OnSubmitted(int selection)
    {
        // Moves screen
        if (_currentSelection == 2 && _pokemonOverview.gameObject.activeSelf)
        {
            _pokemonOverview.gameObject.SetActive(false);
            _movesDetailsScreen.gameObject.SetActive(true);
            _movesDetailsScreen.UpdateUI(Vector2Int.zero, GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[_currentPokemon].Moves);
            _movesScreen.SelectMoveUI(_movesDetailsScreen.DisplayedMove);
        }
    }

    public void OnCancelled()
    {
        if (_movesDetailsScreen.gameObject.activeSelf)
        {
            _pokemonOverview.gameObject.SetActive(true);
            _movesDetailsScreen.gameObject.SetActive(false);
            _movesScreen.UnselectMoveUI(_movesDetailsScreen.DisplayedMove);
        }
        else
        {
            _partyMenu.CloseSummaryScreen();
            _partyMenu.OpenPartyScreen();
        }
    }

    public void CloseScreens()
    {
        _currentScreen.SetActive(false);
    }

    private void ChangeSelection(int newSelection)
    {
        newSelection = (newSelection + _nScreens) % _nScreens;

        Vector2 selectedSize = _categoryBarIcons[_currentSelection].sizeDelta;
        Vector2 unselectedSize = _categoryBarIcons[(_currentSelection + 1) % _nScreens].sizeDelta;

        _currentScreen?.SetActive(false);
        _categoryBarIcons[_currentSelection].sizeDelta = unselectedSize;
        _categoryBarIcons[_currentSelection].GetComponent<Image>().color = Color.white;
        _currentSelection = newSelection;
        _categoryBarIcons[_currentSelection].sizeDelta = selectedSize;
        _categoryBarIcons[_currentSelection].GetComponent<Image>().color = Color.black;
        _currentScreen?.SetActive(true);
    }

    private void ChangePokemon(int newPokemonIdx)
    {
        int nPokemons = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons.Count;
        _currentPokemon = (newPokemonIdx + nPokemons) % nPokemons;

        _pokemonOverview.UpdateUI(_currentPokemon);

        _movesScreen.UpdateUI(_currentPokemon);
        _statsScreen.UpdateUI(_currentPokemon);
        _summaryScreen.UpdateUI(_currentPokemon);
    }
}
