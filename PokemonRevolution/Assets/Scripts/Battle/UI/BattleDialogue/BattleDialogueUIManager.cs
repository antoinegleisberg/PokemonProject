using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogueUIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Start()
    {
        GameEvents.Instance.OnEnterBattle += OnEnterBattle;
        
        BattleEvents.Instance.OnEnterActionSelection += OnEnterActionSelection;
        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonFainted += OnPokemonFainted;
        BattleEvents.Instance.OnPokemonSwitchedOut += OnPokemonSwitchedOut;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;

        BattleEvents.Instance.OnPokemonStatBoosted += OnPokemonStatBoosted;
        BattleEvents.Instance.OnStatusConditionApplied += OnStatusConditionApplied;
        BattleEvents.Instance.OnStatusConditionRemoved += OnStatusConditionRemoved;
        BattleEvents.Instance.OnStatusConditionMessage += OnStatusConditionMessage;
    }


    private void OnDestroy()
    {
        GameEvents.Instance.OnEnterBattle -= OnEnterBattle;
        
        BattleEvents.Instance.OnEnterActionSelection -= OnEnterActionSelection;
        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonFainted -= OnPokemonFainted;
        BattleEvents.Instance.OnPokemonSwitchedOut -= OnPokemonSwitchedOut;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;

        BattleEvents.Instance.OnPokemonStatBoosted -= OnPokemonStatBoosted;
        BattleEvents.Instance.OnStatusConditionApplied -= OnStatusConditionApplied;
        BattleEvents.Instance.OnStatusConditionRemoved -= OnStatusConditionRemoved;
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        UIManager.Instance.WriteDialogueText(dialogueText, $"A wild {enemyPokemon.Name} appeared!");
    }

    private void OnEnterActionSelection()
    {
        UIManager.Instance.WriteDialogueText(dialogueText, $"What will you do?");
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        List<string> messages = new List<string>();
        string msg = $"{attacker.Name} used {move.ScriptableMove.Name}!";
        messages.Add(msg);
        if (attackInfo.moveHits)
        {
            if (attackInfo.criticalHit)
                messages.Add("It's a critical hit!");
            if ((Mathf.Abs(attackInfo.typeEffectiveness - 0.5f) < Mathf.Epsilon) || (Mathf.Abs(attackInfo.typeEffectiveness - 0.25f) < Mathf.Epsilon))
                messages.Add("It's not very effective...");
            else if ((Mathf.Abs(attackInfo.typeEffectiveness - 2.0f) < Mathf.Epsilon) || (Mathf.Abs(attackInfo.typeEffectiveness - 4.0f) < Mathf.Epsilon))
                messages.Add("It's super effective!");
            else if (Mathf.Abs(attackInfo.typeEffectiveness) < Mathf.Epsilon)
                messages.Add($"It doesn't affect {defender.Name} ...");
        }
        else
        {
            messages.Add($"{attacker.Name}'s attack missed !");
        }
        UIManager.Instance.WriteDialogueTexts(dialogueText, messages);
    }

    private void OnPokemonFainted(Pokemon pokemon)
    {
        UIManager.Instance.WriteDialogueText(dialogueText, $"{pokemon.Name} fainted!");
    }

    private void OnPokemonSwitchedOut(Pokemon oldPokemon)
    {
        if (oldPokemon.Owner != PokemonOwner.Player)
            return;
        UIManager.Instance.WriteDialogueText(dialogueText, $"{oldPokemon.Name}, come back!");
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        if (newPokemon.Owner != PokemonOwner.Player)
            return;
        UIManager.Instance.WriteDialogueText(dialogueText, $"Go {newPokemon.Name} !");
    }

    private void OnPokemonStatBoosted(Stat stat, int boost, Pokemon pokemon) 
    {
        string msg;
        switch (boost)
        {
            case -2:
                msg = $"{pokemon.Name}'s {stat} sharply fell !";
                break;
            case -1:
                msg = $"{pokemon.Name}'s {stat} fell !";
                break;
            case 0:
                msg = $"It had no effect !";
                break;
            case 1:
                msg = $"{pokemon.Name}'s {stat} rose !";
                break;
            case 2:
                msg = $"{pokemon.Name}'s {stat} sharply rose ! ";
                break;
            default:
                msg = "I have no idea how the hell this happened !";
                break;
        }
        UIManager.Instance.WriteDialogueText(dialogueText, msg);
    }

    private void OnStatusConditionApplied(StatusCondition statusCondition, Pokemon pokemon)
    {
        string msg = $"{pokemon.Name} {ConditionsDB.Conditions[statusCondition].StartMessage}";
        UIManager.Instance.WriteDialogueText(dialogueText, msg);
    }

    private void OnStatusConditionRemoved(StatusCondition statusCondition, Pokemon pokemon)
    {
        string msg = $"{pokemon.Name} {ConditionsDB.Conditions[statusCondition].EndMessage}";
        UIManager.Instance.WriteDialogueText(dialogueText, msg);
    }
    
    private void OnStatusConditionMessage(string msg)
    {
        UIManager.Instance.WriteDialogueText(dialogueText, msg);
    }
}
