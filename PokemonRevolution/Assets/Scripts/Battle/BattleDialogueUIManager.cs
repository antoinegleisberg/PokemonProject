using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogueUIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Start()
    {
        GameEvents.Current.OnEnterBattle += OnEnterBattle;
    }

    private void OnDestroy()
    {
        GameEvents.Current.OnEnterBattle -= OnEnterBattle;
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        UIManager.Current.WriteDialogueText(dialogueText, $"A wild {enemyPokemon.Name} appeared!");
    }
}
