using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneUIManager : MonoBehaviour
{
    [SerializeField] private GameObject battleScene;

    [SerializeField] private Sprite defaultBattleBackground;

    [SerializeField] private Image battleBackgroundImage;

    [SerializeField] private BattleSceneHUDManager playerHUD;
    [SerializeField] private BattleSceneHUDManager enemyHUD;

    [SerializeField] private Image playerPokemonImage;
    [SerializeField] private Image enemyPokemonImage;

    [SerializeField] private float switchInAnimationDuration = 1.0f;
    [SerializeField] private float attackAnimationDuration = 0.1f;
    [SerializeField] private float hitAnimationDuration = 0.08f;
    [SerializeField] private float faintAnimationDuration = 0.8f;

    // TODO: move this out of here to player inventory
    [SerializeField] private GameObject pokeballPrefab;
    private GameObject pokeball;
    
    private void Start()
    {
        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonDamaged += OnPokemonDamaged;
        BattleEvents.Instance.OnPokemonSwitchedOut += OnPokemonFainted;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
        BattleEvents.Instance.OnStatusConditionApplied += OnStatusConditionApplied;
        BattleEvents.Instance.OnStatusConditionRemoved += OnStatusConditionRemoved;

        BattleEvents.Instance.OnPokeballThrown += OnPokeballThrown;
        BattleEvents.Instance.OnPokemonCaught += OnPokemonCaught;
        BattleEvents.Instance.OnPokemonEscaped += OnPokemonEscaped;
        
        BattleEvents.Instance.OnLevelUp += OnLevelUp;
    }

    private void OnDestroy()
    {
        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonDamaged -= OnPokemonDamaged;
        BattleEvents.Instance.OnPokemonSwitchedOut -= OnPokemonFainted;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
        BattleEvents.Instance.OnStatusConditionApplied -= OnStatusConditionApplied;
        BattleEvents.Instance.OnStatusConditionRemoved -= OnStatusConditionRemoved;

        BattleEvents.Instance.OnPokeballThrown -= OnPokeballThrown;
        BattleEvents.Instance.OnPokemonCaught -= OnPokemonCaught;
        BattleEvents.Instance.OnPokemonEscaped -= OnPokemonEscaped;
        
        BattleEvents.Instance.OnLevelUp -= OnLevelUp;
    }

    public void OnBattleStart(PokemonParty playerParty, PokemonParty enemyParty)
    {
        battleBackgroundImage.sprite = defaultBattleBackground;
        
        BattleUIManager.Instance.EnqueueAnimation(AnimatePokemonEnterBattle(playerParty.GetFirstPokemon(), enemyParty.GetFirstPokemon()));
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        if (attackInfo.moveHits && move.ScriptableMove.Category != MoveCategory.Status)
            BattleUIManager.Instance.EnqueueAnimation(AnimatePokemonAttackAndHit(attacker, defender));
        else
            BattleUIManager.Instance.EnqueueAnimation(AnimatePokemonAttack(attacker));
    }

    private void OnPokemonDamaged(Pokemon pokemon, int damage)
    {
        if (pokemon.Owner == PokemonOwner.Player)
            BattleUIManager.Instance.EnqueueAnimation(playerHUD.UpdateHPBarSmooth(pokemon));
        else
            BattleUIManager.Instance.EnqueueAnimation(enemyHUD.UpdateHPBarSmooth(pokemon));
    }

    public void OnPokemonFainted(Pokemon pokemon)
    {
        BattleUIManager.Instance.EnqueueAnimation(AnimatePokemonFaints(pokemon));
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        BattleUIManager.Instance.EnqueueAnimation(SwitchPokemonIn(newPokemon));
    }

    private void OnStatusConditionApplied(StatusCondition status, Pokemon pokemon)
    {
        if (ConditionsDB.GetCondition(status).IsVolatile)
            return;

        if (pokemon.Owner == PokemonOwner.Player)
            playerHUD.UpdateStatusCondition(pokemon);
        else
            enemyHUD.UpdateStatusCondition(pokemon);
    }

    private void OnStatusConditionRemoved(StatusCondition status, Pokemon pokemon)
    {
        if (ConditionsDB.GetCondition(status).IsVolatile)
            return;

        if (pokemon.Owner == PokemonOwner.Player)
            playerHUD.UpdateStatusCondition(pokemon);
        else
            enemyHUD.UpdateStatusCondition(pokemon);
    }

    private void OnPokeballThrown(Pokemon pokemon)
    {
        BattleUIManager.Instance.EnqueueAnimation(AnimateThrowPokeball(pokemon));
    }

    private void OnPokemonCaught(Pokemon pokemon)
    {
        BattleUIManager.Instance.EnqueueAnimation(AnimatePokemonCaught(pokemon));
    }

    private void OnPokemonEscaped(Pokemon pokemon)
    {
        BattleUIManager.Instance.EnqueueAnimation(AnimatePokemonEscaped(pokemon));
    }

    public void OnExpGained(Pokemon pokemon, int exp)
    {
        if (pokemon.Owner == PokemonOwner.Player)
            BattleUIManager.Instance.EnqueueAnimation(playerHUD.UpdateExpBarSmooth(pokemon, exp));
    }

    private void OnLevelUp(Pokemon pokemon)
    {
        if (pokemon.Owner == PokemonOwner.Player)
            playerHUD.UpdateHUD(pokemon);
    }

    private IEnumerator AnimatePokemonEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        Coroutine playerAnim = StartCoroutine(SwitchPokemonIn(playerPokemon));
        Coroutine enemyAnim = StartCoroutine(SwitchPokemonIn(enemyPokemon));
        yield return playerAnim;
        yield return enemyAnim;
    }

    private IEnumerator AnimatePokemonAttackAndHit(Pokemon attacker, Pokemon defender)
    {
        Coroutine attackAnim = StartCoroutine(AnimatePokemonAttack(attacker));
        Coroutine hitAnim = StartCoroutine(AnimatePokemonHit(defender));
        yield return attackAnim;
        yield return hitAnim;
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
        Vector3 playerOffset = new Vector3(-300, 0, 0);

        if (newPokemon.Owner == PokemonOwner.Player || newPokemon.Owner == PokemonOwner.AllyTrainer)
        {
            Vector3 playerInitPos = playerPokemonImage.rectTransform.localPosition + playerOffset;
            playerPokemonImage.color = Color.white;
            playerHUD.UpdateHUD(newPokemon);
            Coroutine playerAnim = StartCoroutine(MoveImage(playerPokemonImage, playerInitPos, -playerOffset, switchInAnimationDuration));
            yield return playerAnim;
        }
        else if (newPokemon.Owner == PokemonOwner.EnemyTrainer || newPokemon.Owner == PokemonOwner.Wild)
        {
            Vector3 enemyInitPos = enemyPokemonImage.rectTransform.localPosition - playerOffset;
            enemyPokemonImage.color = Color.white;
            enemyHUD.UpdateHUD(newPokemon);
            Coroutine enemyAnim = StartCoroutine(MoveImage(enemyPokemonImage, enemyInitPos, playerOffset, switchInAnimationDuration));
            yield return enemyAnim;
        }
    }

    private IEnumerator AnimateThrowPokeball(Pokemon pokemon)
    {
        pokeball = Instantiate(pokeballPrefab, battleScene.transform);
        Vector3 startPos = playerPokemonImage.rectTransform.localPosition + new Vector3(0, 100, 0);
        Vector3 targetPos = enemyPokemonImage.rectTransform.localPosition;
        Vector3 offset = targetPos - startPos;
        yield return MoveImage(pokeball.GetComponent<Image>(), startPos, offset, 0.3f);
        yield return new WaitForSeconds(0.2f);
        enemyPokemonImage.color = Color.clear;
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator AnimatePokemonCaught(Pokemon pokemon)
    {
        pokeball.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSeconds(0.5f);
        Destroy(pokeball);
    }
    
    private IEnumerator AnimatePokemonEscaped(Pokemon pokemon)
    {
        Destroy(pokeball);
        enemyPokemonImage.color = Color.white;
        yield return null;
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