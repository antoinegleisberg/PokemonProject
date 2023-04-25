using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogueUIManager : MonoBehaviour
{
    private void Start()
    {
        BattleEvents.Instance.OnBattleStart += OnBattleStart;
        
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
        BattleEvents.Instance.OnBattleStart -= OnBattleStart;
        
        BattleEvents.Instance.OnEnterActionSelection -= OnEnterActionSelection;
        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonFainted -= OnPokemonFainted;
        BattleEvents.Instance.OnPokemonSwitchedOut -= OnPokemonSwitchedOut;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;

        BattleEvents.Instance.OnPokemonStatBoosted -= OnPokemonStatBoosted;
        BattleEvents.Instance.OnStatusConditionApplied -= OnStatusConditionApplied;
        BattleEvents.Instance.OnStatusConditionRemoved -= OnStatusConditionRemoved;
    }

    private void OnBattleStart(PokemonParty playerParty, PokemonParty enemyParty)
    {
        BattleUIManager.Instance.WriteDialogueText($"A wild {enemyParty.GetFirstPokemon().Name} appeared!");
    }

    private void OnEnterActionSelection()
    {
        BattleUIManager.Instance.WriteDialogueText($"What will you do?");
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
        BattleUIManager.Instance.WriteDialogueTexts(messages);
    }

    private void OnPokemonFainted(Pokemon pokemon)
    {
        BattleUIManager.Instance.WriteDialogueText($"{pokemon.Name} fainted!");
    }

    private void OnPokemonSwitchedOut(Pokemon oldPokemon)
    {
        if (oldPokemon.Owner != PokemonOwner.Player)
            return;
        BattleUIManager.Instance.WriteDialogueText($"{oldPokemon.Name}, come back!");
    }

    private void OnPokemonSwitchedIn(Pokemon newPokemon)
    {
        if (newPokemon.Owner != PokemonOwner.Player)
            return;
        BattleUIManager.Instance.WriteDialogueText($"Go {newPokemon.Name} !");
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
        BattleUIManager.Instance.WriteDialogueText(msg);
    }

    private void OnStatusConditionApplied(StatusCondition statusCondition, Pokemon pokemon)
    {
        string msg = $"{pokemon.Name} {ConditionsDB.Conditions[statusCondition].StartMessage}";
        BattleUIManager.Instance.WriteDialogueText(msg);
    }

    private void OnStatusConditionRemoved(StatusCondition statusCondition, Pokemon pokemon)
    {
        string msg = $"{pokemon.Name} {ConditionsDB.Conditions[statusCondition].EndMessage}";
        BattleUIManager.Instance.WriteDialogueText(msg);
    }
    
    private void OnStatusConditionMessage(string msg)
    {
        BattleUIManager.Instance.WriteDialogueText(msg);
    }
}
