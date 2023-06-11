using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneHUDManager : MonoBehaviour
{

    [SerializeField] private Image _pokemonImage;

    [SerializeField] private TextMeshProUGUI _pokemonName;
    [SerializeField] private TextMeshProUGUI _pokemonLevelText;

    [SerializeField] private RectTransform _healthBar;
    [SerializeField] private TextMeshProUGUI _healthText;

    [SerializeField] private Image _statusImage;
    [SerializeField] private TextMeshProUGUI _statusText;

    [SerializeField] private RectTransform _expBar;

    public void UpdateHUD(Pokemon pokemon)
    {
        Sprite sprite = (pokemon.Owner == PokemonOwner.Player) ? 
            pokemon.ScriptablePokemon.BackSprite : pokemon.ScriptablePokemon.FrontSprite;
        _pokemonImage.sprite = sprite;

        _pokemonName.text = $"{pokemon.Name}";
        _pokemonLevelText.text = $"Lv {pokemon.Level}";

        float fillAmount = (float)pokemon.CurrentHP / (float)pokemon.MaxHP;
        _healthBar.localScale = new Vector3(fillAmount, 1, 1);
        _healthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHP}";

        if (pokemon.Owner == PokemonOwner.Player)
        {
            float targetExpFillAmount = 1.0f - (float)GrowthRateDB.ExpBeforeLevelUp(pokemon) / GrowthRateDB.Exp2NextLevel(pokemon.ScriptablePokemon.GrowthRate, pokemon.Level);
            _expBar.localScale = new Vector3(targetExpFillAmount, 1, 1);
        }

        UpdateStatusCondition(pokemon);
    }

    public IEnumerator UpdateHPBarSmooth(Pokemon pokemon)
    {
        float pokemonCurrentHP = (float)pokemon.CurrentHP / pokemon.MaxHP;
        float currentHpDisplayed = _healthBar.localScale.x;
        float changeAmount = currentHpDisplayed - pokemonCurrentHP;

        float hpLost = currentHpDisplayed * pokemon.MaxHP - pokemon.CurrentHP;
        int maxHPLostPerSecond = 50;
        float animationTime = Mathf.Max(hpLost / maxHPLostPerSecond, 0.5f);

        for (float t=0; t < animationTime; t += Time.deltaTime)
        {
            float normalizedTime = t / animationTime;
            currentHpDisplayed = pokemonCurrentHP + (1 - normalizedTime) * changeAmount;

            _healthBar.localScale = new Vector3(currentHpDisplayed, 1, 1);
            _healthText.text = $"{Mathf.Round(currentHpDisplayed * pokemon.MaxHP)} / {pokemon.MaxHP}";

            yield return null;
        }

        _healthBar.localScale = new Vector3(pokemonCurrentHP, 1, 1);
        _healthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHP}";
    }

    public IEnumerator UpdateExpBarSmooth(Pokemon pokemon, int exp)
    {
        float targetExpFillAmount = 1.0f - (float)GrowthRateDB.ExpBeforeLevelUp(pokemon) / GrowthRateDB.Exp2NextLevel(pokemon.ScriptablePokemon.GrowthRate, pokemon.Level);
        targetExpFillAmount = Mathf.Clamp01(targetExpFillAmount);
        float currentExpDisplayed = _expBar.localScale.x;
        float changeAmount = targetExpFillAmount - currentExpDisplayed;

        float animationTime = 0.5f;

        for (float t = 0; t < animationTime; t += Time.deltaTime)
        {
            float normalizedTime = t / animationTime;
            currentExpDisplayed = targetExpFillAmount - (1 - normalizedTime) * changeAmount;

            _expBar.localScale = new Vector3(currentExpDisplayed, 1, 1);

            yield return null;
        }

        _expBar.localScale = new Vector3(currentExpDisplayed, 1, 1);
    }

    public void UpdateStatusCondition(Pokemon pokemon)
    {
        if (pokemon.StatusCondition == StatusCondition.None)
        {
            _statusImage.gameObject.SetActive(false);
            return;
        }

        StatusConditionData conditionData = ConditionsDB.GetCondition(pokemon.StatusCondition);

        _statusImage.gameObject.SetActive(true);
        _statusImage.color = conditionData.HUDColor;
        _statusText.text = conditionData.HUDName;
    }
}
