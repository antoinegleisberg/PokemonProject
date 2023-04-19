using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneUIManager : MonoBehaviour
{
    [SerializeField] private Sprite defaultBattleBackground;

    [SerializeField] private Image battleBackgroundImage;

    [SerializeField] private BattleSceneHUDManager playerHUD;
    [SerializeField] private BattleSceneHUDManager enemyHUD;

    [SerializeField] private Image playerPokemonImage;
    [SerializeField] private Image enemyPokemonImage;

    [SerializeField] private float enterBattleAnimationDuration = 1.0f;
    [SerializeField] private float attackAnimationDuration = 0.1f;
    [SerializeField] private float hitAnimationDuration = 0.08f;
    [SerializeField] private float faintAnimationDuration = 0.8f;
    [SerializeField] private float waitingTimeBeforeFaintAnimation = 1.0f;
    
    private void Start()
    {
        GameEvents.Instance.OnEnterBattle += OnEnterBattle;

        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonDamaged += OnPokemonDamaged;
        BattleEvents.Instance.OnPokemonFainted += OnPokemonFainted;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnEnterBattle -= OnEnterBattle;

        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonDamaged -= OnPokemonDamaged;
        BattleEvents.Instance.OnPokemonFainted -= OnPokemonFainted;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        battleBackgroundImage.sprite = defaultBattleBackground;
        
        playerHUD.UpdateHUD(playerPokemon);
        enemyHUD.UpdateHUD(enemyPokemon);
        
        AnimatePokemonEnterBattle(playerPokemon, enemyPokemon);
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        StartCoroutine(AnimatePokemonAttack(attacker));
        StartCoroutine(AnimatePokemonHit(defender));
    }

    private void OnPokemonDamaged(Pokemon pokemon, int damage)
    {
        if (pokemon.Owner == PokemonOwner.Player)
            playerHUD.UpdateHealthBar(pokemon);
        else
            enemyHUD.UpdateHealthBar(pokemon);
    }

    private void OnPokemonFainted(Pokemon pokemon)
    {
        StartCoroutine(AnimatePokemonFaints(pokemon));
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        StartCoroutine(SwitchPokemonIn(newPokemon));
    }

    private void AnimatePokemonEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        Vector3 playerOffset = new Vector3(-300, 0, 0);
        Vector3 playerInitPos = playerPokemonImage.rectTransform.localPosition + playerOffset;
        Vector3 enemyInitPos = enemyPokemonImage.rectTransform.localPosition - playerOffset;
        playerPokemonImage.color = Color.white;
        enemyPokemonImage.color = Color.white;
        StartCoroutine(MoveImage(playerPokemonImage, playerInitPos, -playerOffset, enterBattleAnimationDuration));
        StartCoroutine(MoveImage(enemyPokemonImage, enemyInitPos, playerOffset, enterBattleAnimationDuration));
    }

    private IEnumerator AnimatePokemonAttack(Pokemon attacker)
    {
        Image image =
            attacker.Owner == PokemonOwner.Player ? playerPokemonImage : enemyPokemonImage;
        Vector3 originalPosition = image.rectTransform.localPosition;
        Vector3 offset =
            attacker.Owner == PokemonOwner.Player ? new Vector3(40, 0, 0) : new Vector3(-40, 0, 0);

        yield return MoveImage(image, originalPosition, offset, attackAnimationDuration);
        yield return MoveImage(image, originalPosition + offset, -offset, attackAnimationDuration);
    }

    private IEnumerator AnimatePokemonHit(Pokemon defender)
    {
        yield return new WaitForSeconds(attackAnimationDuration);

        Image image =
            defender.Owner == PokemonOwner.Player ? playerPokemonImage : enemyPokemonImage;
        Color hitColor = Color.grey;

        yield return ColorImage(image, hitColor, hitAnimationDuration);
        yield return ColorImage(image, Color.white, hitAnimationDuration);
    }

    private IEnumerator AnimatePokemonFaints(Pokemon pokemon)
    {
        // Wait for the HP Bar decreasing animation to finish
        yield return new WaitForSeconds(waitingTimeBeforeFaintAnimation);

        Image image =
            pokemon.Owner == PokemonOwner.Player ? playerPokemonImage : enemyPokemonImage;
        Vector3 originalPosition = image.rectTransform.localPosition;
        Vector3 offset = new Vector3(0, -150, 0);
        Color targetColor = new Color(0.5f, 0.5f, 0.5f, 0.0f);

        Coroutine moveAnimation = StartCoroutine(MoveImage(image, originalPosition, offset, faintAnimationDuration));
        Coroutine fadeAnimation = StartCoroutine(ColorImage(image, targetColor, faintAnimationDuration));

        yield return moveAnimation;
        yield return fadeAnimation;

        image.rectTransform.localPosition = originalPosition;
        image.color = new Color(1, 1, 1, 0);
    }

    private IEnumerator SwitchPokemonIn(Pokemon newPokemon)
    {
        yield return new WaitForSeconds(waitingTimeBeforeFaintAnimation + faintAnimationDuration + 0.1f);

        if (newPokemon.Owner == PokemonOwner.Player)
        {
            playerPokemonImage.color = Color.white;
            playerHUD.UpdateHUD(newPokemon);
        }
        else if (newPokemon.Owner == PokemonOwner.EnemyTrainer)
        {
            enemyHUD.UpdateHUD(newPokemon);
        }
    }

    private IEnumerator MoveImage(Image image, Vector3 initialPos, Vector3 offset, float animationTime)
    {
        image.rectTransform.localPosition = initialPos;

        Vector3 targetPos = initialPos + offset;
        float speed = offset.magnitude / animationTime;

        while ((image.rectTransform.localPosition - targetPos).sqrMagnitude > Mathf.Epsilon)
        {
            image.rectTransform.localPosition = Vector3.MoveTowards(image.rectTransform.localPosition, targetPos, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        image.rectTransform.localPosition = targetPos;
    }

    private IEnumerator ColorImage(Image image, Color targetColor, float animationTime)
    {
        Color currentColor = image.color;
        for (float t = 0; t < animationTime; t += Time.deltaTime)
        {
            float normalizedTime = t / animationTime;
            image.color = Color.Lerp(currentColor, targetColor, normalizedTime);
            yield return null;
        }
        image.color = targetColor;
    }
}