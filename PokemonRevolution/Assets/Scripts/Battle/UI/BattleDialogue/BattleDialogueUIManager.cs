using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogueUIManager : MonoBehaviour
{
    private void Start()
    {
        BattleEvents.Instance.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedOut += OnPokemonSwitchedOut;
        BattleEvents.Instance.OnPokemonSwitchedIn += OnPokemonSwitchedIn;

        BattleEvents.Instance.OnPokemonStatBoosted += OnPokemonStatBoosted;
        BattleEvents.Instance.OnStatusConditionApplied += OnStatusConditionApplied;
        BattleEvents.Instance.OnStatusConditionRemoved += OnStatusConditionRemoved;
        BattleEvents.Instance.OnStatusConditionMessage += OnStatusConditionMessage;

        BattleEvents.Instance.OnPokeballThrown += OnPokeballThrown;
        BattleEvents.Instance.OnPokemonEscaped += OnPokemonEscaped;
        BattleEvents.Instance.OnPokemonCaught += OnPokemonCaught;
        BattleEvents.Instance.OnAttemptCatchTrainerPokemon += OnAttemptCatchTrainerPokemon;
        
        BattleEvents.Instance.OnLevelUp += OnLevelUp;
        BattleEvents.Instance.OnMoveLearnt += OnMoveLearnt;
    }

    private void OnDestroy()
    {
        BattleEvents.Instance.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Instance.OnPokemonSwitchedOut -= OnPokemonSwitchedOut;
        BattleEvents.Instance.OnPokemonSwitchedIn -= OnPokemonSwitchedIn;

        BattleEvents.Instance.OnPokemonStatBoosted -= OnPokemonStatBoosted;
        BattleEvents.Instance.OnStatusConditionApplied -= OnStatusConditionApplied;
        BattleEvents.Instance.OnStatusConditionRemoved -= OnStatusConditionRemoved;

        BattleEvents.Instance.OnPokeballThrown -= OnPokeballThrown;
        BattleEvents.Instance.OnPokemonEscaped -= OnPokemonEscaped;
        BattleEvents.Instance.OnPokemonCaught -= OnPokemonCaught;
        BattleEvents.Instance.OnAttemptCatchTrainerPokemon -= OnAttemptCatchTrainerPokemon;
        
        BattleEvents.Instance.OnLevelUp -= OnLevelUp;
        BattleEvents.Instance.OnMoveLearnt -= OnMoveLearnt;
    }

    public void OnBattleStart(PokemonParty playerParty, PokemonParty enemyParty)
    {
        BattleUIManager.Instance.WriteDialogueText($"A wild {enemyParty.GetFirstPokemon().Name} appeared!");
    }

    public void OnEnterActionSelection()
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

    public void OnPokemonFainted(Pokemon pokemon)
    {
        BattleUIManager.Instance.WriteDialogueText($"{pokemon.Name} fainted!");
        if (pokemon.Owner == PokemonOwner.Player)
        {
            BattleUIManager.Instance.WriteDialogueText($"Who will you send in next ?");
        }
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
            BattleUIManager.Instance.WriteDialogueText($"The enemy trainer sent out {newPokemon.Name}!");
        else
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
        string msg = $"{pokemon.Name} {ConditionsDB.GetCondition(statusCondition).StartMessage}";
        BattleUIManager.Instance.WriteDialogueText(msg);
    }

    private void OnStatusConditionRemoved(StatusCondition statusCondition, Pokemon pokemon)
    {
        string msg = $"{pokemon.Name} {ConditionsDB.GetCondition(statusCondition).EndMessage}";
        BattleUIManager.Instance.WriteDialogueText(msg);
    }

    private void OnStatusConditionMessage(string msg)
    {
        BattleUIManager.Instance.WriteDialogueText(msg);
    }

    private void OnPokeballThrown(Pokemon pokemon)
    {
        BattleUIManager.Instance.WriteDialogueText($"You throw a pokeball at the wild {pokemon.Name} !");
    }

    private void OnPokemonEscaped(Pokemon pokemon)
    {
        BattleUIManager.Instance.WriteDialogueText($"The wild {pokemon.Name} appeared to get cought !");
    }

    private void OnPokemonCaught(Pokemon pokemon)
    {
        BattleUIManager.Instance.WriteDialogueText($"You caught {pokemon.Name} !");
    }

    private void OnAttemptCatchTrainerPokemon()
    {
        BattleUIManager.Instance.WriteDialogueText($"You can't catch a trainer's pokemon !");
    }

    public void OnTryToRunAway()
    {
        BattleUIManager.Instance.WriteDialogueText($"You run from the battle");
    }

    public void OnRunAwaySuccess()
    {
        BattleUIManager.Instance.WriteDialogueText($"Got away safely !");
    }

    public void OnRunAwayFail()
    {
        BattleUIManager.Instance.WriteDialogueText($"Can't escape !");
    }

    public void OnAttemptRunFromTrainer()
    {
        BattleUIManager.Instance.WriteDialogueText($"You can't run from a trainer battle !");
    }

    public void OnExpGained(Pokemon pokemon, int exp)
    {
        BattleUIManager.Instance.WriteDialogueText($"{pokemon.Name} gained {exp} exp !");
    }

    private void OnLevelUp(Pokemon pokemon)
    {
        BattleUIManager.Instance.WriteDialogueText($"{pokemon.Name} grew to level {pokemon.Level} !");
    }

    private void OnMoveLearnt(Pokemon pokemon, ScriptableMove oldMove, ScriptableMove newMove)
    {
        if (oldMove == null)
            BattleUIManager.Instance.WriteDialogueText($"{pokemon.Name} learnt {newMove.Name} !");
        else
            BattleUIManager.Instance.WriteDialogueText($"{pokemon.Name} forgot {oldMove.Name} and learnt {newMove.Name} !");
    }

    public void OnChooseMoveToForget(Pokemon pokemon, ScriptableMove newMove)
    {
        BattleUIManager.Instance.WriteDialogueText($"{pokemon.Name} is trying to learn {newMove.Name}, but it already knows {pokemon.Moves.Count} moves. Forget an old move ?");
    }
}
