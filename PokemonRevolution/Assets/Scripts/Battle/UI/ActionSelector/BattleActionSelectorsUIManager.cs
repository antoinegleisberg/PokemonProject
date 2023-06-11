using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionSelectorsUIManager : MonoBehaviour
{
    [SerializeField] private UISelectorNavigationManager _actionSelector;
    [SerializeField] private UISelectorNavigationManager _moveSelector;
    [SerializeField] private UISelectorNavigationManager _switchPokemonSelector;
    [SerializeField] private UISelectorNavigationManager _replacePokemonSelector;
    [SerializeField] private UISelectorNavigationManager _bagSelector;
    [SerializeField] private UISelectorNavigationManager _pokeballSelector;
    [SerializeField] private UISelectorNavigationManager _medicineSelector;
    [SerializeField] private UISelectorNavigationManager _statusHealerSelector;
    [SerializeField] private UISelectorNavigationManager _battleItemSelector;
    [SerializeField] private UISelectorNavigationManager _forgetMoveSelector;
    private List<UISelectorNavigationManager> _selectors;

    private UISelectorNavigationManager _currentSelector;

    [SerializeField] private RectTransform _selectionIndicator;

    // Specific selectors for UI content updates
    [SerializeField] private MoveSelectorUIManager _moveSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager _pokemonSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager _pokemonReplacementUIManager;
    [SerializeField] private ForgetMoveSelectorUIManager _forgetMoveSelectorUIManager;
    
    private void Start()
    {
        _selectors = new List<UISelectorNavigationManager> { 
            _actionSelector,
            _moveSelector,
            _switchPokemonSelector,
            _replacePokemonSelector,
            _bagSelector,
            _pokeballSelector,
            _medicineSelector,
            _statusHealerSelector,
            _battleItemSelector,
            _forgetMoveSelector
        };

        SubscribeToEvents();

        foreach (UISelectorNavigationManager navManager in _selectors)
        {
            navManager.SetSelectionIndicator(_selectionIndicator);
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        BattleUIEvents.Instance.OnAttackButtonPressed += () => SetActiveSelector(_moveSelector);
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed += () => SetActiveSelector(_bagSelector);
        
        BattleUIEvents.Instance.OnCancelMoveSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection += SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;

        BattleUIEvents.Instance.OnPokeballsButtonPressed += () => SetActiveSelector(_pokeballSelector);
        BattleUIEvents.Instance.OnMedicinesButtonPressed += () => SetActiveSelector(_medicineSelector);
        BattleUIEvents.Instance.OnStatusHealersButtonPressed += () => SetActiveSelector(_statusHealerSelector);
        BattleUIEvents.Instance.OnBattleItemsButtonPressed += () => SetActiveSelector(_battleItemSelector);
        BattleUIEvents.Instance.OnCancelBagSubMenuSelection += () => SetActiveSelector(_bagSelector);
    }

    private void UnsubscribeToEvents()
    {
        BattleUIEvents.Instance.OnAttackButtonPressed -= () => SetActiveSelector(_moveSelector);
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed -= OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed -= () => SetActiveSelector(_bagSelector);
        
        BattleUIEvents.Instance.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection -= SetActiveSelectorToActionSelector;
        BattleUIEvents.Instance.OnCancelBagSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
        
        BattleUIEvents.Instance.OnPokeballsButtonPressed -= () => SetActiveSelector(_pokeballSelector);
        BattleUIEvents.Instance.OnMedicinesButtonPressed -= () => SetActiveSelector(_medicineSelector);
        BattleUIEvents.Instance.OnStatusHealersButtonPressed -= () => SetActiveSelector(_statusHealerSelector);
        BattleUIEvents.Instance.OnBattleItemsButtonPressed -= () => SetActiveSelector(_battleItemSelector);
        BattleUIEvents.Instance.OnCancelBagSubMenuSelection -= () => SetActiveSelector(_bagSelector);
    }

    public void OpenFaintedPokemonReplacementMenu()
    {
        PokemonParty playerParty = GameManager.Instance.PlayerPartyManager.PokemonParty;
        _pokemonReplacementUIManager.UpdatePokemonButtons(playerParty);
        SetActiveSelector(_replacePokemonSelector);
    }

    public void HandleUINavigation(Vector2Int input)
    {
        _currentSelector?.HandleUINavigation(input);
    }

    public void HandleUISubmit()
    {
        _currentSelector?.HandleUISubmit();
    }

    public void HandleUICancel()
    {
        _currentSelector?.HandleUICancel();
    }

    private void SetActiveSelector(UISelectorNavigationManager selector)
    {
        if (_currentSelector == selector)
        {
            return;
        }

        _currentSelector?.gameObject.SetActive(false);
        _currentSelector = selector;
        _currentSelector?.gameObject.SetActive(true);
    }

    private IEnumerator SetActiveSelectorCoroutine(UISelectorNavigationManager selector)
    {
        yield return BattleUIManager.Instance.WaitWhileBusy();
        SetActiveSelector(selector);
    }

    public void DeactivateAllSelectors() => SetActiveSelector(null);

    public void SetActiveSelectorToActionSelector()
    {
         StartCoroutine(SetActiveSelectorCoroutine(_actionSelector));
    }

    public void OnBattleStart(PokemonParty playerParty)
    {
        _moveSelectorUIManager.UpdateMovesUI(playerParty.GetFirstPokemon());
        SetActiveSelector(null);
    }

    private void OnSwitchPokemonButtonPressed()
    {
        PokemonParty playerParty = GameManager.Instance.PlayerPartyManager.PokemonParty;
        _pokemonSelectorUIManager.UpdatePokemonButtons(playerParty);
        SetActiveSelector(_switchPokemonSelector);
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        _moveSelectorUIManager.UpdateMovesUI(attacker);
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        _moveSelectorUIManager.UpdateMovesUI(newPokemon);
    }
    
    public void OnChooseMoveToForget(Pokemon pokemon, ScriptableMove newMove)
    {
        _forgetMoveSelectorUIManager.UpdateUI(pokemon, newMove);
        SetActiveSelector(_forgetMoveSelector);
    }
}