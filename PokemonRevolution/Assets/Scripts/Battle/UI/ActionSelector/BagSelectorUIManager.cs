using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagSelectorUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bagSelector;
    [SerializeField] private GameObject pokeballSelector;
    [SerializeField] private GameObject medicineSelector;
    [SerializeField] private GameObject statusHealerSelector;
    [SerializeField] private GameObject battleItemsSelector;
    private List<GameObject> selectors;

    private void Start()
    {
        selectors = new List<GameObject> { bagSelector, pokeballSelector, medicineSelector, statusHealerSelector, battleItemsSelector };

        BattleUIEvents.Instance.OnBagButtonPressed += () => SetActiveSelector(bagSelector);
        BattleUIEvents.Instance.OnPokeballsButtonPressed += () => SetActiveSelector(pokeballSelector);
        BattleUIEvents.Instance.OnMedicinesButtonPressed += () => SetActiveSelector(medicineSelector);
        BattleUIEvents.Instance.OnStatusHealersButtonPressed += () => SetActiveSelector(statusHealerSelector);
        BattleUIEvents.Instance.OnBattleItemsButtonPressed += () => SetActiveSelector(battleItemsSelector);
        BattleUIEvents.Instance.OnCancelBagSubMenuSelection += () => SetActiveSelector(bagSelector);
    }

    private void SetActiveSelector(GameObject selector)
    {
        foreach (GameObject s in selectors)
        {
            s.SetActive(false);
        }
        selector?.SetActive(true);
    }

    private void OnDestroy()
    {
        BattleUIEvents.Instance.OnBagButtonPressed -= () => SetActiveSelector(bagSelector);
        BattleUIEvents.Instance.OnPokeballsButtonPressed -= () => SetActiveSelector(pokeballSelector);
        BattleUIEvents.Instance.OnMedicinesButtonPressed -= () => SetActiveSelector(medicineSelector);
        BattleUIEvents.Instance.OnStatusHealersButtonPressed -= () => SetActiveSelector(statusHealerSelector);
        BattleUIEvents.Instance.OnBattleItemsButtonPressed -= () => SetActiveSelector(battleItemsSelector);
        BattleUIEvents.Instance.OnCancelBagSubMenuSelection -= () => SetActiveSelector(bagSelector);
    }
}
