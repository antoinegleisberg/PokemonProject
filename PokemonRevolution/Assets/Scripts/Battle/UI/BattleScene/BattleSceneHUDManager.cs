using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneHUDManager : MonoBehaviour
{

    [SerializeField] private Image pokemonImage;

    [SerializeField] private TextMeshProUGUI pokemonName;
    [SerializeField] private TextMeshProUGUI pokemonLevelText;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    
    public void UpdateHUD(Pokemon pokemon)
    {
        Sprite sprite = (pokemon.Owner == PokemonOwner.Player) ? 
            pokemon.ScriptablePokemon.BackSprite : pokemon.ScriptablePokemon.FrontSprite;
        pokemonImage.sprite = sprite;

        pokemonName.text = $"{pokemon.Name}";
        pokemonLevelText.text = $"Lv {pokemon.Level}";


        float fillAmount = (float)pokemon.CurrentHP / (float)pokemon.MaxHP;
        healthBar.transform.localScale = new Vector3(fillAmount, 1, 1);
        healthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHP}";
    }

    public IEnumerator UpdateHPBarSmooth(Pokemon pokemon)
    {
        float pokemonCurrentHP = (float)pokemon.CurrentHP / pokemon.MaxHP;
        float currentHpDisplayed = healthBar.transform.localScale.x;
        float changeAmount = currentHpDisplayed - pokemonCurrentHP;

        float hpLost = currentHpDisplayed * pokemon.MaxHP - pokemon.CurrentHP;
        int maxHPLostPerSecond = 50;
        float animationTime = Mathf.Max(hpLost / maxHPLostPerSecond, 0.5f);

        for (float t=0; t < animationTime; t += Time.deltaTime)
        {
            float normalizedTime = t / animationTime;
            currentHpDisplayed = pokemonCurrentHP + (1 - normalizedTime) * changeAmount;

            healthBar.transform.localScale = new Vector3(currentHpDisplayed, 1, 1);
            healthText.text = $"{Mathf.Round(currentHpDisplayed * pokemon.MaxHP)} / {pokemon.MaxHP}";

            yield return null;
        }

        healthBar.transform.localScale = new Vector3(pokemonCurrentHP, 1, 1);
        healthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHP}";
    }
}
