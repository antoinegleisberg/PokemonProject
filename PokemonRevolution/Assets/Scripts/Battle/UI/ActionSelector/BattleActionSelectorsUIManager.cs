using System;
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


    private Action<int> onFaintedPokemonReplacementSelected;
    
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
        BattleUIEvents.Instance.OnAttackButtonPressed += SetActiveSelectorToMoveSelector;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed += OnBagButtonPressed;
        
        BattleUIEvents.Instance.OnCancelMoveSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
        BattleUIEvents.Instance.OnReplacePokemonSelected += OnFaintedPokemonReplacementSelected;
    }

    private void UnsubscribeToEvents()
    {
        BattleUIEvents.Instance.OnAttackButtonPressed -= SetActiveSelectorToMoveSelector;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed -= OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed -= OnBagButtonPressed;
        
        BattleUIEvents.Instance.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
        BattleUIEvents.Instance.OnReplacePokemonSelected -= OnFaintedPokemonReplacementSelected;
    }

    public void OpenFaintedPokemonReplacementMenu(Action<int> OnReplacementSelected = null)
    {
        onFaintedPokemonReplacementSelected = OnReplacementSelected;
        pokemonReplacementUIManager.UpdatePokemonButtons(playerPokemonParty);
        SetActiveSelector(replacePokemonSelector);
    }

    private void OnFaintedPokemonReplacementSelected(int pokemonIdx)
    {
        onFaintedPokemonReplacementSelected?.Invoke(pokemonIdx);
        onFaintedPokemonReplacementSelected = null;
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

    public void DeactivateAllSelectors() => SetActiveSelector(null);

    public void SetActiveSelectorToActionSelector()
    {
         StartCoroutine(SetActiveSelectorCoroutine(actionSelector));
    }

    private void SetActiveSelectorToMoveSelector() => SetActiveSelector(moveSelector);

    public void OnBattleStart(PokemonParty playerParty)
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
    
    public void OnChooseMoveToForget(Pokemon pokemon, ScriptableMove newMove)
    {
        forgetMoveSelectorUIManager.UpdateUI(pokemon, newMove);
        SetActiveSelector(forgetMoveSelector);
    }
}