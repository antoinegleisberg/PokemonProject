using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuPokemons : MonoBehaviour
{
    [SerializeField] private UINavigator _pokemonPartyNavigator;
    [SerializeField] private PartyMenuSummary _partyMenuSummary;
    [SerializeField] private PartyMenuMoves _partyMenuMoves;

    [SerializeField] private List<Image> _partyMemberImages;
    private List<NavigationItem> _partyMemberNavigationItems;

    private PokemonParty _playerParty;

    private void Awake()
    {
        _playerParty = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty;

        _partyMemberNavigationItems = new List<NavigationItem>();
        foreach (Image pokemonIcon in _partyMemberImages)
        {
            _partyMemberNavigationItems.Add(pokemonIcon.GetComponent<NavigationItem>());
        }

        _pokemonPartyNavigator.OnNavigated += _partyMenuSummary.UpdateSummary;
        _pokemonPartyNavigator.OnNavigated += _partyMenuMoves.UpdateMoves;
    }

    private void OnEnable()
    {
        UpdatePartyImages();
        _partyMenuSummary.UpdateSummary(_pokemonPartyNavigator.CurrentSelection);
        _partyMenuMoves.UpdateMoves(_pokemonPartyNavigator.CurrentSelection);
    }

    private void Start()
    {
        _pokemonPartyNavigator.OnCancelled += UIManager.Instance.OpenPauseMenu;
        _pokemonPartyNavigator.OnSubmitted += (int _) => UIManager.Instance.OpenPartyMenu();
    }
    
    private void OnDestroy()
    {
        _pokemonPartyNavigator.OnNavigated -= _partyMenuSummary.UpdateSummary;
        _pokemonPartyNavigator.OnNavigated -= _partyMenuMoves.UpdateMoves;
        _pokemonPartyNavigator.OnCancelled -= UIManager.Instance.OpenPauseMenu;
        _pokemonPartyNavigator.OnSubmitted -= (int _) => UIManager.Instance.OpenPartyMenu();
    }

    private void UpdatePartyImages()
    {
        for (int i = 0; i < _partyMemberImages.Count; i++)
        {
            if (i < _playerParty.Pokemons.Count)
            {
                _partyMemberNavigationItems[i].IsSelectable = true;
                _partyMemberImages[i].sprite = _playerParty.Pokemons[i].ScriptablePokemon.IconSprite;
            }
            else
            {
                _partyMemberNavigationItems[i].IsSelectable = false;
                _partyMemberImages[i].sprite = null;
            }
        }
    }
}
