using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleActionSelectorsUIManager : MonoBehaviour
{
    [SerializeField] private GameObject actionSelector;
    [SerializeField] private GameObject moveSelector;
    [SerializeField] private GameObject switchPokemonSelector;
    [SerializeField] private GameObject replacePokemonSelector;
    private List<GameObject> selectors;

    [SerializeField] private MoveSelectorUIManager moveSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager pokemonSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager pokemonReplacementUIManager;

    private PokemonParty playerPokemonParty;

    private void Start()
    {
        selectors = new List<GameObject> { actionSelector, moveSelector, switchPokemonSelector, replacePokemonSelector };

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.Instance.OnPokemonEncounter += (playerParty, enemyParty) => playerPokemonParty = playerParty;
        GameEvents.Instance.OnEnterBattle += OnEnterBattle;

        BattleUIEvents.Instance.OnAttackButtonPressed += SetActiveSelectorToMoveSelector;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed += OnBagButtonPressed;

        BattleUIEvents.Instance.OnMoveSelected += DeactivateAllSelectors;
        BattleUIEvents.Instance.OnCancelMoveSelection += SetActiveSelectorToActionSelector;

        BattleUIEvents.Instance.OnSwitchPokemonSelected += DeactivateAllSelectors;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnEnterActionSelection += SetActiveSelectorToActionSelector;
        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
        BattleEvents.Instance.OnReplaceFaintedPokemon += OnReplaceFaintedPokemon;
    }

    private void UnsubscribeToEvents()
    {
        GameEvents.Instance.OnPokemonEncounter -= (playerParty, enemyParty) => playerPokemonParty = playerParty;
        GameEvents.Instance.OnEnterBattle -= OnEnterBattle;

        BattleUIEvents.Instance.OnAttackButtonPressed -= SetActiveSelectorToMoveSelector;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed -= OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed -= OnBagButtonPressed;

        BattleUIEvents.Instance.OnMoveSelected -= DeactivateAllSelectors;
        BattleUIEvents.Instance.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;

        BattleUIEvents.Instance.OnSwitchPokemonSelected -= DeactivateAllSelectors;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnEnterActionSelection -= SetActiveSelectorToActionSelector;
        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
        BattleEvents.Instance.OnReplaceFaintedPokemon -= OnReplaceFaintedPokemon;
    }

    private void SetActiveSelector(GameObject selector)
    {
        foreach (GameObject s in selectors)
        {
            s.SetActive(false);
        }
        selector?.SetActive(true);
    }

    private void DeactivateAllSelectors(int _) => SetActiveSelector(null);

    private void SetActiveSelectorToActionSelector() => SetActiveSelector(actionSelector);

    private void SetActiveSelectorToMoveSelector() => SetActiveSelector(moveSelector);

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        moveSelectorUIManager.UpdateMovesUI(playerPokemon);
        SetActiveSelector(null);
    }

    private void OnSwitchPokemonButtonPressed()
    {
        pokemonSelectorUIManager.UpdatePokemonButtons(playerPokemonParty);
        SetActiveSelector(switchPokemonSelector);
    }

    private void OnBagButtonPressed()
    {
        // TODO
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
}