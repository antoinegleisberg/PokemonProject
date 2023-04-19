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
    private List<GameObject> selectors;

    [SerializeField] private MoveSelectorUIManager moveSelectorUIManager;
    [SerializeField] private PokemonSelectorUIManager pokemonSelectorUIManager;

    private PokemonParty playerPokemonParty;

    private void Start()
    {
        selectors = new List<GameObject> { actionSelector, moveSelector, switchPokemonSelector };

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

        BattleUIEvents.Instance.OnAttackButtonPressed += OnAttackButtonPressed;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed += OnBagButtonPressed;

        BattleUIEvents.Instance.OnMoveSelected += OnMoveSelected;
        BattleUIEvents.Instance.OnCancelMoveSelection += SetActiveSelectorToActionSelector;

        BattleUIEvents.Instance.OnSwitchPokemonSelected += OnSwitchPokemonSelected;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnEnterActionSelection += SetActiveSelectorToActionSelector;
        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;
    }

    private void UnsubscribeToEvents()
    {
        GameEvents.Instance.OnPokemonEncounter -= (playerParty, enemyParty) => playerPokemonParty = playerParty;
        GameEvents.Instance.OnEnterBattle -= OnEnterBattle;

        BattleUIEvents.Instance.OnAttackButtonPressed -= OnAttackButtonPressed;
        BattleUIEvents.Instance.OnSwitchPokemonButtonPressed -= OnSwitchPokemonButtonPressed;
        BattleUIEvents.Instance.OnBagButtonPressed -= OnBagButtonPressed;

        BattleUIEvents.Instance.OnMoveSelected -= OnMoveSelected;
        BattleUIEvents.Instance.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;

        BattleUIEvents.Instance.OnSwitchPokemonSelected -= OnSwitchPokemonSelected;
        BattleUIEvents.Instance.OnCancelSwitchPokemonSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Instance.OnEnterActionSelection -= SetActiveSelectorToActionSelector;
        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;
    }

    private void SetActiveSelector(GameObject selector)
    {
        Debug.Log($"Setting active selector to {selector?.name ?? "null"}");
        foreach (GameObject s in selectors)
        {
            s.SetActive(false);
        }
        selector?.SetActive(true);
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        moveSelectorUIManager.UpdateMovesUI(playerPokemon);
        SetActiveSelector(null);
    }

    private void SetActiveSelectorToActionSelector() => SetActiveSelector(actionSelector);
    
    private void OnAttackButtonPressed()
    {
        SetActiveSelector(moveSelector);
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

    private void OnMoveSelected(int moveIndex) => SetActiveSelector(null);

    private void OnSwitchPokemonSelected(int pokemonIndex)
    {
        Debug.Log("Switch to pokemon " + pokemonIndex);
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        moveSelectorUIManager.UpdateMovesUI(attacker);
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        moveSelectorUIManager.UpdateMovesUI(newPokemon);
    }
}