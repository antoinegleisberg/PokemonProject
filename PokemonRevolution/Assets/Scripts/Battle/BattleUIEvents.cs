using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIEvents : MonoBehaviour
{
    public static BattleUIEvents Current;

    private void Awake()
    {
        Current = this;
    }

    // Action selector events
    public event Action OnAttackButtonPressed;
    public event Action OnSwitchPokemonButtonPressed;
    public event Action OnBagButtonPressed;
    public event Action OnRunButtonPressed;

    // Move selector events
    public event Action<int> OnMoveSelected;
    public event Action OnCancelMoveSelection;
    public event Action<int> OnTargetSelected; // TODO

    // Bag button events : TODO
    public event Action OnPokeballsButtonPressed;
    public event Action OnMedicinesButtonPressed;
    public event Action OnStatusHealersButtonPressed;

    // SubMenu events : TODO
    public event Action<int> OnPokeballButtonPressed;
    public event Action<int> OnMedicineButtonPressed;
    public event Action<int> OnStatusHealerButtonPressed;

    public void AttackButtonPressed()
    {
        Debug.Log("AttackButtonPressed");
        OnAttackButtonPressed?.Invoke();
    }
    public void SwitchPokemonButtonPressed() => OnSwitchPokemonButtonPressed?.Invoke();
    public void BagButtonPressed() => OnBagButtonPressed?.Invoke();
    public void RunButtonPressed() => OnRunButtonPressed?.Invoke();

    public void MoveSelected(int moveIndex) => OnMoveSelected?.Invoke(moveIndex);
    public void CancelMoveSelection() => OnCancelMoveSelection?.Invoke();
}
