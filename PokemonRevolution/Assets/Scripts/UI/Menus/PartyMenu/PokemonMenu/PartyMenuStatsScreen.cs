using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuStatsScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _hpBarForeground;
    [SerializeField] private Image _statusImage;
    [SerializeField] private TextMeshProUGUI _statusText;

    [SerializeField] private List<TextMeshProUGUI> _baseStatsTexts;
    [SerializeField] private List<TextMeshProUGUI> _ivStatsTexts;
    [SerializeField] private List<TextMeshProUGUI> _evStatsTexts;
    [SerializeField] private List<TextMeshProUGUI> _totalStatsTexts;

    [SerializeField] private TextMeshProUGUI _abilityName;
    [SerializeField] private TextMeshProUGUI _abilityDescription;

    public void UpdateUI(int pokemonIdx)
    {
        Pokemon pokemon = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[pokemonIdx];

        float fillAmount = (float)pokemon.CurrentHP / pokemon.MaxHP;
        _hpBarForeground.localScale = new Vector3(fillAmount, 1);

        if (pokemon.StatusCondition != StatusCondition.None)
        {
            _statusImage.color = ConditionsDB.GetCondition(pokemon.StatusCondition).HUDColor;
            _statusText.text = ConditionsDB.GetCondition(pokemon.StatusCondition).HUDName;
        }
        else
        {
            _statusImage.color = new Color(1, 1, 1, 0);
            _statusText.text = "";
        }

        List<int> baseStats = new List<int>()
        {
            pokemon.ScriptablePokemon.BaseHP,
            pokemon.ScriptablePokemon.BaseAttack,
            pokemon.ScriptablePokemon.BaseDefense,
            pokemon.ScriptablePokemon.BaseSpecialAttack,
            pokemon.ScriptablePokemon.BaseSpecialDefense,
            pokemon.ScriptablePokemon.BaseSpeed,
        };
        
        for (int i=0; i<_baseStatsTexts.Count; i++)
        {
            _baseStatsTexts[i].text = baseStats[i].ToString();
        }
        for (int i = 0; i < _ivStatsTexts.Count; i++)
        {
            _ivStatsTexts[i].text = pokemon.IVs[(Stat)i].ToString();
        }
        for (int i = 0; i < _evStatsTexts.Count; i++)
        {
            _evStatsTexts[i].text = pokemon.EVs[(Stat)i].ToString();
        }
        for (int i = 0; i < _totalStatsTexts.Count; i++)
        {
            _totalStatsTexts[i].text = pokemon.Stats[(Stat)i].ToString();
        }
    }
}
