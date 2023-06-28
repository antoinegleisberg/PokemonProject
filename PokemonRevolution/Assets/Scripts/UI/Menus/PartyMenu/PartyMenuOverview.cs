using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuOverview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private Image _genderIcon;
    [SerializeField] private Image _pokemonIcon;

    public void UpdateUI(int pokemonIdx)
    {
        Pokemon pokemon = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[pokemonIdx];

        _name.text = pokemon.Name;
        _level.text = $"Lv. {pokemon.Level}";
        // _genderIcon.sprite = 
        _pokemonIcon.sprite = pokemon.ScriptablePokemon.FrontSprite;
    }
}
