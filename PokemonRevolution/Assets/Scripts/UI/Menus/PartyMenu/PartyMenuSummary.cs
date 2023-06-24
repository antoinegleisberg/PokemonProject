using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuSummary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _summaryPokemonName;
    [SerializeField] private TextMeshProUGUI _summaryLevelText;
    [SerializeField] private Image _summaryPokemonImage;
    [SerializeField] private TextMeshProUGUI _pokemonHeldItemText;

    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _defenseText;
    [SerializeField] private TextMeshProUGUI _specialAttackText;
    [SerializeField] private TextMeshProUGUI _specialDefenseText;
    [SerializeField] private TextMeshProUGUI _speedText;

    private PokemonParty _playerParty;

    private PokemonParty PlayerParty {
        get
        {
            if (_playerParty == null)
            {
                _playerParty = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty;
            }
            return _playerParty;
        }
    }

    public void UpdateSummary(int pokemonIdx)
    {
        Pokemon pokemon = PlayerParty.Pokemons[pokemonIdx];
        _summaryPokemonName.text = pokemon.Name;
        _summaryLevelText.text = $"Lv. {pokemon.Level}";
        _summaryPokemonImage.sprite = pokemon.ScriptablePokemon.FrontSprite;
        _pokemonHeldItemText.text = "Leftovers";  // TODO

        _healthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHP}";
        _attackText.text = pokemon.Attack.ToString();
        _defenseText.text = pokemon.Defense.ToString();
        _specialAttackText.text = pokemon.SpecialAttack.ToString();
        _specialDefenseText.text = pokemon.SpecialDefense.ToString();
        _speedText.text = pokemon.Speed.ToString();
    }
}
