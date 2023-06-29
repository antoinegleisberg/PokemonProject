using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuSummaryScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pokedexNumber;
    [SerializeField] private TextMeshProUGUI _pokemonName;
    [SerializeField] private Image _type1Image;
    [SerializeField] private Image _type2Image;
    [SerializeField] private TextMeshProUGUI _totalExpPoints;
    [SerializeField] private TextMeshProUGUI _nextLevelExpPoints;
    [SerializeField] private RectTransform _expBarForeground;
    [SerializeField] private TextMeshProUGUI _heldItemName;
    [SerializeField] private TextMeshProUGUI _heldItemDescription;
    
    private PokemonParty PlayerParty => GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty;
    
    public void UpdateUI(int pokemonIdx)
    {
        Pokemon pokemon = PlayerParty.Pokemons[pokemonIdx];
        ScriptablePokemon scriptablePokemon = pokemon.ScriptablePokemon;

        _pokedexNumber.text = scriptablePokemon.Id.ToString();
        _pokemonName.text = scriptablePokemon.Name;
        _type1Image.sprite = TypeUtils.TypeInfo(scriptablePokemon.Type1).TypeIcon;
        if (scriptablePokemon.Type2 != PokemonType.None)
        {
            _type2Image.sprite = TypeUtils.TypeInfo(scriptablePokemon.Type2).TypeIcon;
        }
        else
        {
            _type2Image.sprite = null;
        }
        _totalExpPoints.text = pokemon.TotalExperiencePoints.ToString();
        int expBeforeLvUp = GrowthRateDB.ExpBeforeLevelUp(pokemon);
        int expBetweenLevels = GrowthRateDB.Exp2NextLevel(scriptablePokemon.GrowthRate, pokemon.Level);
        _nextLevelExpPoints.text = expBeforeLvUp.ToString();
        float fillAmount = (float)(expBetweenLevels - expBeforeLvUp) / expBetweenLevels;
        _expBarForeground.localScale = new Vector3(fillAmount, 1);
    }
}
