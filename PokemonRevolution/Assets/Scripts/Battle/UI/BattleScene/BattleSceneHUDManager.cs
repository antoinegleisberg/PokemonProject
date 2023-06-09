using System.Collections;
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

    [SerializeField] private Image statusImage;
    [SerializeField] private TextMeshProUGUI statusText;

    [SerializeField] private GameObject expBar;

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

        if (pokemon.Owner == PokemonOwner.Player)
        {
            float targetExpFillAmount = 1.0f - (float)GrowthRateDB.ExpBeforeLevelUp(pokemon) / GrowthRateDB.Exp2NextLevel(pokemon.ScriptablePokemon.GrowthRate, pokemon.Level);
            expBar.transform.localScale = new Vector3(targetExpFillAmount, 1, 1);
        }

        UpdateStatusCondition(pokemon);
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

    public IEnumerator UpdateExpBarSmooth(Pokemon pokemon, int exp)
    {
        float targetExpFillAmount = 1.0f - (float)GrowthRateDB.ExpBeforeLevelUp(pokemon) / GrowthRateDB.Exp2NextLevel(pokemon.ScriptablePokemon.GrowthRate, pokemon.Level);
        targetExpFillAmount = Mathf.Clamp01(targetExpFillAmount);
        float currentExpDisplayed = expBar.transform.localScale.x;
        float changeAmount = targetExpFillAmount - currentExpDisplayed;

        float animationTime = 0.5f;

        for (float t = 0; t < animationTime; t += Time.deltaTime)
        {
            float normalizedTime = t / animationTime;
            currentExpDisplayed = targetExpFillAmount - (1 - normalizedTime) * changeAmount;

            expBar.transform.localScale = new Vector3(currentExpDisplayed, 1, 1);

            yield return null;
        }

        expBar.transform.localScale = new Vector3(currentExpDisplayed, 1, 1);
    }

    public void UpdateStatusCondition(Pokemon pokemon)
    {
        if (pokemon.StatusCondition == StatusCondition.None)
        {
            statusImage.gameObject.SetActive(false);
            return;
        }

        StatusConditionData conditionData = ConditionsDB.GetCondition(pokemon.StatusCondition);

        statusImage.gameObject.SetActive(true);
        statusImage.color = conditionData.HUDColor;
        statusText.text = conditionData.HUDName;
    }
}
