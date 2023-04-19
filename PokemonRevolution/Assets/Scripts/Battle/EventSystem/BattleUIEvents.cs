using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIEvents : MonoBehaviour
{
    public static BattleUIEvents Instance;

    private void Awake() => Instance = this;

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

    // Bag SubMenu events : TODO
    public event Action<int> OnPokeballButtonPressed;
    public event Action<int> OnMedicineButtonPressed;
    public event Action<int> OnStatusHealerButtonPressed;

    // SwitchPokemonEvents
    public event Action<int> OnSwitchPokemonSelected;
    public event Action OnCancelSwitchPokemonSelection;

    public void AttackButtonPressed() => OnAttackButtonPressed?.Invoke();
    public void SwitchPokemonButtonPressed() => OnSwitchPokemonButtonPressed?.Invoke();
    public void BagButtonPressed() => OnBagButtonPressed?.Invoke();
    public void RunButtonPressed() => OnRunButtonPressed?.Invoke();

    public void MoveSelected(int moveIndex) => OnMoveSelected?.Invoke(moveIndex);
    public void CancelMoveSelection() => OnCancelMoveSelection?.Invoke();
    public void TargetSelected(int targetIndex) => OnTargetSelected?.Invoke(targetIndex);

    public void PokeballsButtonPressed() => OnPokeballsButtonPressed?.Invoke();
    public void MedicinesButtonPressed() => OnMedicinesButtonPressed?.Invoke();
    public void StatusHealersButtonPressed() => OnStatusHealersButtonPressed?.Invoke();

    public void PokeballButtonPressed(int pokeballIndex) => OnPokeballButtonPressed?.Invoke(pokeballIndex);
    public void MedicineButtonPressed(int medicineIndex) => OnMedicineButtonPressed?.Invoke(medicineIndex);
    public void StatusHealerButtonPressed(int statusHealerIndex) => OnStatusHealerButtonPressed?.Invoke(statusHealerIndex);

    public void SwitchPokemonSelected(int pokemonIndex) => OnSwitchPokemonSelected?.Invoke(pokemonIndex);
    public void CancelSwitchPokemonSelection() => OnCancelSwitchPokemonSelection?.Invoke();
}
