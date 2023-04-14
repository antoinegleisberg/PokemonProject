using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneUIManager : MonoBehaviour
{
    [SerializeField] private Sprite defaultBattleBackground;

    [SerializeField] private Image battleBackgroundImage;
    [SerializeField] private Image playerUnitImage;
    [SerializeField] private Image enemyUnitImage;

    [SerializeField] private TextMeshProUGUI playerPokemonName;
    [SerializeField] private TextMeshProUGUI enemyPokemonName;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI enemyLevelText;

    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;

    private void Start()
    {
        GameEvents.Current.OnPokemonDamaged += OnPokemonDamaged;
        GameEvents.Current.OnEnterBattle += OnEnterBattle;
    }

    private void OnDestroy()
    {
        GameEvents.Current.OnPokemonDamaged -= OnPokemonDamaged;
        GameEvents.Current.OnEnterBattle -= OnEnterBattle;
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        battleBackgroundImage.sprite = defaultBattleBackground;
        
        playerUnitImage.sprite = playerPokemon.ScriptablePokemon.BackSprite;
        enemyUnitImage.sprite = enemyPokemon.ScriptablePokemon.FrontSprite;
        
        playerPokemonName.text = $"{playerPokemon.Name}";
        enemyPokemonName.text = $"{enemyPokemon.Name}";
        playerLevelText.text = $"Lv {playerPokemon.Level}";
        enemyLevelText.text = $"Lv {enemyPokemon.Level}";

        playerHealthBar.transform.localScale = new Vector3(1, 1, 1);
        enemyHealthBar.transform.localScale = new Vector3(1, 1, 1);
        playerHealthText.text = $"{playerPokemon.CurrentHP} / {playerPokemon.MaxHealthPoints}";
        enemyHealthText.text = $"{enemyPokemon.CurrentHP} / {enemyPokemon.MaxHealthPoints}";
    }

    private void OnPokemonDamaged(Pokemon pokemon, int damage)
    {
        float fillAmount = (float)pokemon.CurrentHP / (float)pokemon.MaxHealthPoints;
        if (pokemon.Owner == PokemonOwner.Player)
        {
            playerHealthBar.transform.localScale = new Vector3(fillAmount, 1, 1);
            playerHealthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHealthPoints}";
        }
        else
        {
            enemyHealthBar.transform.localScale = new Vector3(fillAmount, 1, 1);
            enemyHealthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHealthPoints}";
        }
    }
}