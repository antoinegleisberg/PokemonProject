using System;
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
    public event Action<int> OnSelectMoveToForget;

    // Bag SubMenu events : TODO
    public event Action<BagCategory, int> OnItemSelected;
    public event Action<int> OnPokeballButtonPressed;
    public event Action<int> OnBattleItemButtonPressed;
    public event Action<int> OnMedicineButtonPressed;
    public event Action<int> OnStatusHealerButtonPressed;
    public event Action OnCancelBagSubMenuSelection;

    // SwitchPokemonEvents
    public event Action<int> OnSwitchPokemonSelected;
    public event Action OnCancelSwitchPokemonSelection;
    public event Action<int> OnReplacePokemonSelected;

    public void AttackButtonPressed() => OnAttackButtonPressed?.Invoke();
    public void SwitchPokemonButtonPressed() => OnSwitchPokemonButtonPressed?.Invoke();
    public void BagButtonPressed() => OnBagButtonPressed?.Invoke();
    public void RunButtonPressed() => OnRunButtonPressed?.Invoke();

    public void MoveSelected(int moveIndex) => OnMoveSelected?.Invoke(moveIndex);
    public void CancelMoveSelection() => OnCancelMoveSelection?.Invoke();
    public void TargetSelected(int targetIndex) => OnTargetSelected?.Invoke(targetIndex);
    public void SelectMoveToForget(int moveIndex) => OnSelectMoveToForget?.Invoke(moveIndex);

    public void ItemSelected(BagCategory bagCategory, int itemIndex) => OnItemSelected?.Invoke(bagCategory, itemIndex);
    public void PokeballButtonPressed(int pokeballIndex) => OnPokeballButtonPressed?.Invoke(pokeballIndex);
    public void BattleItemButtonPressed(int battleItemIndex) => OnBattleItemButtonPressed?.Invoke(battleItemIndex);
    public void MedicineButtonPressed(int medicineIndex) => OnMedicineButtonPressed?.Invoke(medicineIndex);
    public void StatusHealerButtonPressed(int statusHealerIndex) => OnStatusHealerButtonPressed?.Invoke(statusHealerIndex);
    public void CancelBagSubMenuSelection() => OnCancelBagSubMenuSelection?.Invoke();

    public void SwitchPokemonSelected(int pokemonIndex) => OnSwitchPokemonSelected?.Invoke(pokemonIndex);
    public void CancelSwitchPokemonSelection() => OnCancelSwitchPokemonSelection?.Invoke();
    public void ReplacePokemonSelected(int pokemonIndex) => OnReplacePokemonSelected?.Invoke(pokemonIndex);
}
