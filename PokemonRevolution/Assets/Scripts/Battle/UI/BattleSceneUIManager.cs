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
        BattleEvents.Current.OnPokemonDamaged += OnPokemonDamaged;
        GameEvents.Current.OnEnterBattle += OnEnterBattle;
        BattleEvents.Current.OnPokemonSwitched += OnPokemonSwitched;
    }

    private void OnDestroy()
    {
        BattleEvents.Current.OnPokemonDamaged -= OnPokemonDamaged;
        GameEvents.Current.OnEnterBattle -= OnEnterBattle;
        BattleEvents.Current.OnPokemonSwitched -= OnPokemonSwitched;
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        battleBackgroundImage.sprite = defaultBattleBackground;
        SetPlayerSceneUI(playerPokemon);
        SetEnemySceneUI(enemyPokemon);
    }

    private void SetPlayerSceneUI(Pokemon playerPokemon)
    {
        playerUnitImage.sprite = playerPokemon.ScriptablePokemon.BackSprite;
        
        playerPokemonName.text = $"{playerPokemon.Name}";
        playerLevelText.text = $"Lv {playerPokemon.Level}";

        float playerFillAmount = (float)playerPokemon.CurrentHP / (float)playerPokemon.MaxHealthPoints;
        playerHealthBar.transform.localScale = new Vector3(playerFillAmount, 1, 1);
        playerHealthText.text = $"{playerPokemon.CurrentHP} / {playerPokemon.MaxHealthPoints}";
    }

    private void SetEnemySceneUI(Pokemon enemyPokemon)
    {
        enemyUnitImage.sprite = enemyPokemon.ScriptablePokemon.FrontSprite;

        enemyPokemonName.text = $"{enemyPokemon.Name}";
        enemyLevelText.text = $"Lv {enemyPokemon.Level}";

        float enemyFillAmount = (float)enemyPokemon.CurrentHP / (float)enemyPokemon.MaxHealthPoints;
        enemyHealthBar.transform.localScale = new Vector3(enemyFillAmount, 1, 1);
        enemyHealthText.text = $"{enemyPokemon.CurrentHP} / {enemyPokemon.MaxHealthPoints}";
    }
    
    private void OnPokemonDamaged(Pokemon pokemon, int damage)
    {
        float fillAmount = (float)pokemon.CurrentHP / (float)pokemon.MaxHealthPoints;
        if (pokemon.Owner == PokemonOwner.Player)
        {
            StartCoroutine(SetHPBarSmooth(playerHealthBar, fillAmount));
            playerHealthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHealthPoints}";
        }
        else
        {
            StartCoroutine(SetHPBarSmooth(enemyHealthBar, fillAmount));
            enemyHealthText.text = $"{pokemon.CurrentHP} / {pokemon.MaxHealthPoints}";
        }
    }

    private IEnumerator SetHPBarSmooth(GameObject healthBar, float pokemonCurrentHP)
    {
        float currentHpDisplayed = healthBar.transform.localScale.x;
        float changeAmount = currentHpDisplayed - pokemonCurrentHP;
        
        while (currentHpDisplayed - pokemonCurrentHP > Mathf.Epsilon)
        {
            currentHpDisplayed -= changeAmount * Time.deltaTime;
            healthBar.transform.localScale = new Vector3(currentHpDisplayed, 1, 1);
            yield return null;
        }

        healthBar.transform.localScale = new Vector3(pokemonCurrentHP, 1, 1);
    }

    private void OnPokemonSwitched(Pokemon oldPokemon, Pokemon newPokemon)
    {
        StartCoroutine(SwitchPokemon(newPokemon));
    }

    private IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        yield return new WaitForSeconds(1.81f);

        if (newPokemon.Owner == PokemonOwner.Player)
        {
            playerUnitImage.color = Color.white;
            SetPlayerSceneUI(newPokemon);
        }
        else if (newPokemon.Owner == PokemonOwner.EnemyTrainer)
        {
            SetEnemySceneUI(newPokemon);
        }
    }
}