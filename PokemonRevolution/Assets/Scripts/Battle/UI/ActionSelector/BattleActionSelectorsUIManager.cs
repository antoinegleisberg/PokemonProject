using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionSelectorsUIManager : MonoBehaviour
{
    [SerializeField] private UISelectorNavigationManager actionSelector;
    [SerializeField] private UISelectorNavigationManager moveSelector;
    [SerializeField] private UISelectorNavigationManager switchPokemonSelector;
    [SerializeField] private UISelectorNavigationManager replacePokemonSelector;
    [SerializeField] private UISelectorNavigationManager bagSelector;
    [SerializeField] private UISelectorNavigationManager pokeballSelector;
    [SerializeField] private UISelectorNavigationManager medicineSelector;
    [SerializeField] private UISelectorNavigationManager statusHealerSelector;
    [SerializeField] private UISelectorNavigationManager battleItemSelector;
    [SerializeField] private UISelectorNavigationManager forgetMoveSelector;
    private List<UISelectorNavigationManager> selectors;

    private UISelectorNavigationManager currentSelector;

    [SerializeField] private RectTransform selectionIndicator;

    // Specific selectors for UI content updates
    [SerializeField] private MoveSelectorUIManager moveSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager pokemonSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager pokemonReplacementUIManager;
    [SerializeField] private ForgetMoveSelectorUIManager forgetMoveSelectorUIManager;

    private PokemonParty playerPokemonParty;

    private Action<int> onFaintedPokemonReplacementSelected;
    
    private void Start()
    {
        selectors = new List<UISelectorNavigationManager> { 
            actionSelector,
            moveSelector,
            switchPokemonSelector,
            replacePokemonSelector,
            bagSelector,
            pokeballSelector,
            medicineSelector,
            statusHealerSelector,
            battleItemSelector,
            forgetMoveSelector
        };

        SubscribeToEvents();

        foreach (UISelectorNavigationManager navManager in selectors)
        {
            navManager.SetSelectionIndicator(selectionIndicator);
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        BattleUIEvents.Instance.OnAttackButtonPressed += () => SetActiveSelector(moveSelector);
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed += () => SetActiveSelector(bagSelector);
        
        BattleUIEvents.Instance.OnCancelMoveSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
        BattleUIEvents.Instance.OnReplacePokemonSelected += OnFaintedPokemonReplacementSelected;

        BattleUIEvents.Instance.OnPokeballsButtonPressed += () => SetActiveSelector(pokeballSelector);
        BattleUIEvents.Instance.OnMedicinesButtonPressed += () => SetActiveSelector(medicineSelector);
        BattleUIEvents.Instance.OnStatusHealersButtonPressed += () => SetActiveSelector(statusHealerSelector);
        BattleUIEvents.Instance.OnBattleItemsButtonPressed += () => SetActiveSelector(battleItemSelector);
        BattleUIEvents.Instance.OnCancelBagSubMenuSelection += () => SetActiveSelector(bagSelector);
    }

    private void UnsubscribeToEvents()
    {
        BattleUIEvents.Instance.OnAttackButtonPressed -= () => SetActiveSelector(moveSelector);
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed -= OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed -= () => SetActiveSelector(bagSelector);
        
        BattleUIEvents.Instance.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
        BattleUIEvents.Instance.OnReplacePokemonSelected -= OnFaintedPokemonReplacementSelected;
        
        BattleUIEvents.Instance.OnPokeballsButtonPressed -= () => SetActiveSelector(pokeballSelector);
        BattleUIEvents.Instance.OnMedicinesButtonPressed -= () => SetActiveSelector(medicineSelector);
        BattleUIEvents.Instance.OnStatusHealersButtonPressed -= () => SetActiveSelector(statusHealerSelector);
        BattleUIEvents.Instance.OnBattleItemsButtonPressed -= () => SetActiveSelector(battleItemSelector);
        BattleUIEvents.Instance.OnCancelBagSubMenuSelection -= () => SetActiveSelector(bagSelector);
    }

    public void OpenFaintedPokemonReplacementMenu(Action<int> OnReplacementSelected = null)
    {
        onFaintedPokemonReplacementSelected = OnReplacementSelected;
        pokemonReplacementUIManager.UpdatePokemonButtons(playerPokemonParty);
        SetActiveSelector(replacePokemonSelector);
    }

    public void HandleUINavigation(Vector2Int input)
    {
        currentSelector?.HandleUINavigation(input);
    }

    public void HandleUISubmit()
    {
        currentSelector?.HandleUISubmit();
    }

    public void HandleUICancel()
    {
        currentSelector?.HandleUICancel();
    }

    private void OnFaintedPokemonReplacementSelected(int pokemonIdx)
    {
        onFaintedPokemonReplacementSelected?.Invoke(pokemonIdx);
        onFaintedPokemonReplacementSelected = null;
    }

    private void SetActiveSelector(UISelectorNavigationManager selector)
    {
        if (currentSelector == selector)
        {
            return;
        }

        currentSelector?.gameObject.SetActive(false);
        currentSelector = selector;
        currentSelector?.gameObject.SetActive(true);

        //foreach (UISelectorNavigationManager s in selectors)
        //{
        //    s.gameObject.SetActive(false);
        //}
        //selector?.gameObject.SetActive(true);
    }

    private IEnumerator SetActiveSelectorCoroutine(UISelectorNavigationManager selector)
    {
        yield return BattleUIManager.Instance.WaitWhileBusy();
        SetActiveSelector(selector);
    }

    public void DeactivateAllSelectors() => SetActiveSelector(null);

    public void SetActiveSelectorToActionSelector()
    {
         StartCoroutine(SetActiveSelectorCoroutine(actionSelector));
    }

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