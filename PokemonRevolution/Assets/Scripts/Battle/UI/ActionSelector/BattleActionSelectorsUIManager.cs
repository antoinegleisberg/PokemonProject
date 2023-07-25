using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionSelectorsUIManager : MonoBehaviour
{
    [SerializeField] private UINavigator _actionSelector;
    [SerializeField] private UINavigator _moveSelector;
    private List<UINavigator> _navigators;

    private UINavigator _currentNavigator;

    [SerializeField] private RectTransform _selectionIndicator;

    // Specific selectors for UI content updates
    [SerializeField] private BattleMoveSelector _moveSelectorUIManager;
    
    private void Start()
    {
        _navigators = new List<UINavigator> { 
            _actionSelector,
            _moveSelector
        };

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        BattleUIEvents.Instance.OnAttackButtonPressed += () => SetActiveSelector(_moveSelector);
        
        BattleUIEvents.Instance.OnCancelMoveSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
    }

    private void UnsubscribeToEvents()
    {
        BattleUIEvents.Instance.OnAttackButtonPressed -= () => SetActiveSelector(_moveSelector);
        
        BattleUIEvents.Instance.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
    }

    public void OpenFaintedPokemonReplacementMenu()
    {
        // SetActiveSelector(_replacePokemonSelector);
    }

    public void HandleUINavigation(Vector2Int input)
    {
        _currentNavigator?.OnNavigate(input);
    }

    public void HandleUISubmit()
    {
        _currentNavigator?.OnSubmit();
    }

    public void HandleUICancel()
    {
        _currentNavigator?.OnCancel();
    }

    private void SetActiveSelector(UINavigator selector)
    {
        if (_currentNavigator == selector)
        {
            return;
        }

        _currentNavigator?.gameObject.SetActive(false);
        _currentNavigator = selector;
        _currentNavigator?.gameObject.SetActive(true);
    }

    private IEnumerator SetActiveSelectorCoroutine(UINavigator selector)
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
        SetActiveSelector(null);
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        // _moveSelectorUIManager.UpdateMovesUI(attacker);
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        // _moveSelectorUIManager.UpdateMovesUI(newPokemon);
    }
    
    public void OnChooseMoveToForget(Pokemon pokemon, ScriptableMove newMove)
    {
        // SetActiveSelector(_forgetMoveSelector);
    }
}