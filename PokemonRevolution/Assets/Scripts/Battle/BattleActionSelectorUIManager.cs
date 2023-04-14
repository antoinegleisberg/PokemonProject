using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionSelectorUIManager : MonoBehaviour
{
    [SerializeField] private GameObject actionSelector;
    [SerializeField] private GameObject moveSelector;
    private List<GameObject> selectors;

    private void Start()
    {
        selectors = new List<GameObject> { actionSelector, moveSelector };

        GameEvents.Current.OnEnterBattle += OnEnterBattle;

        BattleUIEvents.Current.OnAttackButtonPressed += OnAttackButtonPressed;
        BattleUIEvents.Current.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Current.OnBagButtonPressed += OnBagButtonPressed;
        BattleUIEvents.Current.OnRunButtonPressed += OnRunButtonPressed;

        BattleUIEvents.Current.OnMoveSelected += OnMoveSelected;
        BattleUIEvents.Current.OnCancelMoveSelection += OnCancelMoveSelection;
    }

    private void SetActiveSelector(GameObject selector)
    {
        foreach (GameObject s in selectors)
        {
            s.SetActive(false);
        }
        selector.SetActive(true);
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        SetActiveSelector(actionSelector);
    }
    
    private void OnAttackButtonPressed()
    {
        SetActiveSelector(moveSelector);
    }

    private void OnSwitchPokemonButtonPressed()
    {
        // TODO
    }

    private void OnBagButtonPressed()
    {
        // TODO
    }

    private void OnRunButtonPressed()
    {
        SetActiveSelector(actionSelector);
    }

    private void OnMoveSelected(int moveIndex)
    {
        SetActiveSelector(actionSelector);
    }

    private void OnCancelMoveSelection()
    {
        SetActiveSelector(actionSelector);
    }
}