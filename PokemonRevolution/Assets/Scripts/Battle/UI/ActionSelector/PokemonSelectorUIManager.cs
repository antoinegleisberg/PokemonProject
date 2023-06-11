using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonSelectorUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _pokemonButtonsContainer;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _faintedColor;

    private List<Button> _pokemonButtons;
    private List<TextMeshProUGUI> _pokemonNames;
    private List<TextMeshProUGUI> _pokemonLevelTexts;
    private List<Image> _pokemonIcons;
    private List<TextMeshProUGUI> _pokemonHealthTexts;
    private List<Transform> _pokemonHealthBars;
    
    private void InitButtonsList()
    {
        _pokemonButtons = new List<Button>();
        _pokemonNames = new List<TextMeshProUGUI>();
        _pokemonLevelTexts = new List<TextMeshProUGUI>();
        _pokemonIcons = new List<Image>();
        _pokemonHealthTexts = new List<TextMeshProUGUI>();
        _pokemonHealthBars = new List<Transform>();
        foreach (Button button in _pokemonButtonsContainer.GetComponentsInChildren<Button>())
        {
            _pokemonButtons.Add(button);
            _pokemonNames.Add(button.transform.Find("PokemonName").GetComponent<TextMeshProUGUI>());
            _pokemonLevelTexts.Add(button.transform.Find("LevelText").GetComponent<TextMeshProUGUI>());
            _pokemonIcons.Add(button.transform.Find("PokemonIcon").GetComponent<Image>());
            _pokemonHealthTexts.Add(button.transform.Find("HealthBar").Find("HealthText").GetComponent<TextMeshProUGUI>());
            _pokemonHealthBars.Add(button.transform.Find("HealthBar").Find("HealthBarForeground"));
        }
    }

    public void UpdatePokemonButtons(PokemonParty playerParty)
    {
        if (_pokemonButtons == null)
            InitButtonsList();

        for (int i = 0; i < _pokemonButtons.Count; i++)
        {
            if (i < playerParty.Pokemons.Count)
            {
                _pokemonButtons[i].gameObject.SetActive(true);
                _pokemonNames[i].text = playerParty.Pokemons[i].Name;
                _pokemonLevelTexts[i].text = $"Lv. {playerParty.Pokemons[i].Level}";
                _pokemonIcons[i].sprite = playerParty.Pokemons[i].ScriptablePokemon.IconSprite;
                _pokemonHealthTexts[i].text = $"{playerParty.Pokemons[i].CurrentHP}/{playerParty.Pokemons[i].MaxHP}";
                float healthPercentage = (float)playerParty.Pokemons[i].CurrentHP / (float)playerParty.Pokemons[i].MaxHP;
                _pokemonHealthBars[i].localScale = new Vector3(healthPercentage, 1, 1);

                if (playerParty.Pokemons[i].IsFainted)
                {
                    _pokemonButtons[i].interactable = false;
                    _pokemonButtons[i].GetComponent<Image>().color = _faintedColor;
                }
                else
                {
                    _pokemonButtons[i].interactable = true;
                    _pokemonButtons[i].GetComponent<Image>().color = _defaultColor;
                }
            }
            else
            {
                _pokemonButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
