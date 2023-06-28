using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuPartyScreen : MonoBehaviour
{
    [SerializeField] private List<Image> _pokemonUIContainers;

    private List<Image> _icons;
    private List<TextMeshProUGUI> _names;
    private List<Image> _genderIcons;
    private List<RectTransform> _hpBars;
    private List<TextMeshProUGUI> _hpTexts;
    private List<TextMeshProUGUI> _levelTexts;

    private PokemonParty PlayerParty {
        get
        {
            return GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty;
        }
    }
    
    public void UpdateSelection(int selection)
    {
        UpdateUI();

        Pokemon pokemon = PlayerParty.Pokemons[selection];

        _pokemonUIContainers[selection].color = Color.black;
        _names[selection].color = Color.white;
        _hpTexts[selection].color = Color.white;
        _levelTexts[selection].color = Color.white;
    }

    private void UpdateUI()
    {
        if (_icons == null)
        {
            InitLists();
        } 

        int numberOfPokemons = PlayerParty.Pokemons.Count;
        for (int i = 0; i < _pokemonUIContainers.Count; i++)
        {
            if (i >= numberOfPokemons)
            {
                _pokemonUIContainers[i].gameObject.SetActive(false);
                _pokemonUIContainers[i].GetComponent<NavigationItem>().IsSelectable = false;
                continue;
            }

            Pokemon pokemon = PlayerParty.Pokemons[i];

            _pokemonUIContainers[i].gameObject.SetActive(true);
            _pokemonUIContainers[i].GetComponent<NavigationItem>().IsSelectable = true;

            _pokemonUIContainers[i].color = Color.white;
            _names[i].color = Color.black;
            _hpTexts[i].color = Color.black;
            _levelTexts[i].color = Color.black;

            _icons[i].sprite = pokemon.ScriptablePokemon.IconSprite;
            _names[i].text = pokemon.ScriptablePokemon.Name;
            // _genderIcons[i].sprite = 
            _hpBars[i].localScale = new Vector3((float)pokemon.CurrentHP / pokemon.MaxHP, 1);
            _hpTexts[i].text = $"{pokemon.CurrentHP} / {pokemon.MaxHP}";
            _levelTexts[i].text = $"Lv. {pokemon.Level}";
        }
    }

    private void InitLists()
    {
        _icons = new List<Image>();
        _names = new List<TextMeshProUGUI>();
        _genderIcons = new List<Image>();
        _hpBars = new List<RectTransform>();
        _hpTexts = new List<TextMeshProUGUI>();
        _levelTexts = new List<TextMeshProUGUI>();

        for (int i=0; i < _pokemonUIContainers.Count; i++)
        {
            Transform container = _pokemonUIContainers[i].transform;
            _icons.Add(container.Find("Icon").GetComponent<Image>());
            container = container.Find("PokemonSummary");
            _names.Add(container.Find("Name").GetComponent<TextMeshProUGUI>());
            _genderIcons.Add(container.Find("Gender").GetComponent<Image>());
            _hpTexts.Add(container.Find("HPText").GetComponent<TextMeshProUGUI>());
            _levelTexts.Add(container.Find("LevelText").GetComponent<TextMeshProUGUI>());
            container = container.Find("HPBar");
            _hpBars.Add(container.Find("Foreground").GetComponent<RectTransform>());
        }
    }
}
