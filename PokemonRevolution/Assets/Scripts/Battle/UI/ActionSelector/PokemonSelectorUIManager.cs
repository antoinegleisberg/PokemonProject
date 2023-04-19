using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonSelectorUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform pokemonButtonsContainer;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color faintedColor;

    private List<Button> pokemonButtons;

    private void InitButtonsList()
    {
        pokemonButtons = new List<Button>();
        foreach (Button button in pokemonButtonsContainer.GetComponentsInChildren<Button>())
        {
            pokemonButtons.Add(button);
        }
    }

    public void UpdatePokemonButtons(PokemonParty playerParty)
    {
        if (pokemonButtons == null)
            InitButtonsList();

        for (int i = 0; i < pokemonButtons.Count; i++)
        {
            if (i < playerParty.Pokemons.Count)
            {
                pokemonButtons[i].gameObject.SetActive(true);
                pokemonButtons[i].transform.Find("PokemonName").GetComponent<TextMeshProUGUI>().text = playerParty.Pokemons[i].Name;
                pokemonButtons[i].transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv. {playerParty.Pokemons[i].Level}";
                pokemonButtons[i].transform.Find("PokemonIcon").GetComponent<Image>().sprite = playerParty.Pokemons[i].ScriptablePokemon.IconSprite;
                pokemonButtons[i].transform.Find("HealthBar").Find("HealthText").GetComponent<TextMeshProUGUI>().text = $"{playerParty.Pokemons[i].CurrentHP}/{playerParty.Pokemons[i].MaxHealthPoints}";
                float healthPercentage = (float)playerParty.Pokemons[i].CurrentHP / (float)playerParty.Pokemons[i].MaxHealthPoints;
                pokemonButtons[i].transform.Find("HealthBar").Find("HealthBarForeground").localScale = new Vector3(healthPercentage, 1, 1);
            
                if (playerParty.Pokemons[i].IsFainted)
                {
                    pokemonButtons[i].interactable = false;
                    pokemonButtons[i].GetComponent<Image>().color = faintedColor;
                }
                else
                {
                    pokemonButtons[i].interactable = true;
                    pokemonButtons[i].GetComponent<Image>().color = defaultColor;
                }
            }
            else
            {
                pokemonButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
