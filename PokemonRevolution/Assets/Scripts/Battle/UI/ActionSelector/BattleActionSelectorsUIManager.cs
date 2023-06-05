using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionSelectorsUIManager : MonoBehaviour
{
    [SerializeField] private GameObject actionSelector;
    [SerializeField] private GameObject moveSelector;
    [SerializeField] private GameObject switchPokemonSelector;
    [SerializeField] private GameObject replacePokemonSelector;
    [SerializeField] private GameObject bagSelector;
    [SerializeField] private GameObject forgetMoveSelector;
    private List<GameObject> selectors;

    [SerializeField] private MoveSelectorUIManager moveSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager pokemonSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager pokemonReplacementUIManager;
    [SerializeField] private ForgetMoveSelectorUIManager forgetMoveSelectorUIManager;

    private PokemonParty playerPokemonParty;

    private void Start()
    {
        selectors = new List<GameObject> { actionSelector, moveSelector, switchPokemonSelector, replacePokemonSelector, bagSelector, forgetMoveSelector };

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        BattleEvents.Instance.OnBattleStart += OnBattleStart;

        BattleUIEvents.Instance.OnAttackButtonPressed += SetActiveSelectorToMoveSelector;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed += OnBagButtonPressed;
        
        BattleUIEvents.Instance.OnCancelMoveSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnEnterActionSelection += SetActiveSelectorToActionSelector;
        BattleEvents.Instance.OnExitActionSelection += DeactivateAllSelectors;

        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
        BattleEvents.Instance.OnReplaceFaintedPokemon += OnReplaceFaintedPokemon;

        BattleEvents.Instance.OnChooseMoveToForget += OnChooseMoveToForget;
    }

    private void UnsubscribeToEvents()
    {
        BattleEvents.Instance.OnBattleStart -= OnBattleStart;

        BattleUIEvents.Instance.OnAttackButtonPressed -= SetActiveSelectorToMoveSelector;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed -= OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed -= OnBagButtonPressed;
        
        BattleUIEvents.Instance.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnEnterActionSelection -= SetActiveSelectorToActionSelector;
        BattleEvents.Instance.OnExitActionSelection -= DeactivateAllSelectors;

        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
        BattleEvents.Instance.OnReplaceFaintedPokemon -= OnReplaceFaintedPokemon;
        
        BattleEvents.Instance.OnChooseMoveToForget -= OnChooseMoveToForget;
    }

    private void SetActiveSelector(GameObject selector)
    {
        foreach (GameObject s in selectors)
        {
            s.SetActive(false);
        }
        selector?.SetActive(true);
    }

    private IEnumerator SetActiveSelectorCoroutine(GameObject selector)
    {
        yield return BattleUIManager.Instance.WaitWhileBusy();
        SetActiveSelector(selector);
    }

    private void DeactivateAllSelectors() => SetActiveSelector(null);

    private void SetActiveSelectorToActionSelector()
    {
         StartCoroutine(SetActiveSelectorCoroutine(actionSelector));
    }

    private void SetActiveSelectorToMoveSelector() => SetActiveSelector(moveSelector);

    private void OnBattleStart(PokemonParty playerParty, PokemonParty enemyParty)
    {
        playerPokemonParty = playerParty;
        moveSelectorUIManager.UpdateMovesUI(playerPokemonParty.GetFirstPokemon());
        SetActiveSelector(null);
    }

    private void OnSwitchPokemonButtonPressed()
    {
        pokemonSelectorUIManager.UpdatePokemonButtons(playerPokemonParty);
        SetActiveSelector(switchPokemonSelector);
    }

    private void OnBagButtonPressed()
    {
        SetActiveSelector(bagSelector);
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        moveSelectorUIManager.UpdateMovesUI(attacker);
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        moveSelectorUIManager.UpdateMovesUI(newPokemon);
    }

    private void OnReplaceFaintedPokemon()
    {
        pokemonReplacementUIManager.UpdatePokemonButtons(playerPokemonParty);
        SetActiveSelector(replacePokemonSelector);
    }
    
    private void OnChooseMoveToForget(Pokemon pokemon, ScriptableMove newMove)
    {
        forgetMoveSelectorUIManager.UpdateUI(pokemon, newMove);
        SetActiveSelector(forgetMoveSelector);
    }
}