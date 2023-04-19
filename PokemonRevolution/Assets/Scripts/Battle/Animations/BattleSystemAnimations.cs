using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemAnimations : MonoBehaviour
{
    [SerializeField] private Image playerPokemonImage;
    [SerializeField] private Image enemyPokemonImage;

    [SerializeField] private float enterBattleAnimationTime;
    [SerializeField] private float attackAnimationTime;
    [SerializeField] private float hitAnimationTime;
    [SerializeField] private float faintAnimationTime;

    private void Start()
    {
        BattleEvents.Current.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Current.OnPokemonFainted += OnPokemonFainted;
        GameEvents.Current.OnEnterBattle += OnEnterBattle;
    }

    private void OnDestroy()
    {
        BattleEvents.Current.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Current.OnPokemonFainted -= OnPokemonFainted;
        GameEvents.Current.OnEnterBattle -= OnEnterBattle;
    }
    
    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        AnimatePokemonEnterBattle(playerPokemon, enemyPokemon);
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        StartCoroutine(AnimatePokemonAttack(attacker));
        StartCoroutine(AnimatePokemonHit(defender));
    }
    
    private void OnPokemonFainted(Pokemon pokemon)
    {
        StartCoroutine(AnimatePokemonFaints(pokemon));
    }

    private void AnimatePokemonEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        Vector3 playerOffset = new Vector3(-300, 0, 0);
        Vector3 playerInitPos = playerPokemonImage.rectTransform.localPosition + playerOffset;
        Vector3 enemyInitPos = enemyPokemonImage.rectTransform.localPosition - playerOffset;
        playerPokemonImage.color = Color.white;
        enemyPokemonImage.color = Color.white;
        StartCoroutine(MoveImage(playerPokemonImage, playerInitPos, -playerOffset, enterBattleAnimationTime));
        StartCoroutine(MoveImage(enemyPokemonImage, enemyInitPos, playerOffset, enterBattleAnimationTime));
    }

    private IEnumerator AnimatePokemonAttack(Pokemon attacker)
    {
        Image image =
            attacker.Owner == PokemonOwner.Player ? playerPokemonImage : enemyPokemonImage;
        Vector3 originalPosition = image.rectTransform.localPosition;
        Vector3 offset =
            attacker.Owner == PokemonOwner.Player ? new Vector3(40, 0, 0) : new Vector3(-40, 0, 0);

        yield return MoveImage(image, originalPosition, offset, attackAnimationTime);
        yield return MoveImage(image, originalPosition + offset, -offset, attackAnimationTime);
    }

    private IEnumerator AnimatePokemonHit(Pokemon defender)
    {
        yield return new WaitForSeconds(attackAnimationTime);

        Image image =
            defender.Owner == PokemonOwner.Player ? playerPokemonImage : enemyPokemonImage;
        Color hitColor = Color.grey;

        yield return ColorImage(image, hitColor, hitAnimationTime);
        yield return ColorImage(image, Color.white, hitAnimationTime);
    }

    private IEnumerator AnimatePokemonFaints(Pokemon pokemon)
    {
        yield return new WaitForSeconds(1.0f);

        Image image =
            pokemon.Owner == PokemonOwner.Player ? playerPokemonImage : enemyPokemonImage;
        Vector3 originalPosition = image.rectTransform.localPosition;
        Vector3 offset = new Vector3(0, -150, 0);
        Color targetColor = new Color(0.5f, 0.5f, 0.5f, 0.0f);

        Coroutine moveAnimation = StartCoroutine(MoveImage(image, originalPosition, offset, faintAnimationTime));
        Coroutine fadeAnimation = StartCoroutine(ColorImage(image, targetColor, faintAnimationTime));

        yield return moveAnimation;
        yield return fadeAnimation;

        image.rectTransform.localPosition = originalPosition;
        image.color = new Color(1, 1, 1, 0);
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
