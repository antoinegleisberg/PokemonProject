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
        BattleEvents.Current.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Current.OnPokemonSwitched += OnPokemonSwitched;
    }

    private void OnDestroy()
    {
        GameEvents.Current.OnEnterBattle -= OnEnterBattle;
        BattleEvents.Current.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Current.OnPokemonSwitched -= OnPokemonSwitched;
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        UIManager.Current.WriteDialogueText(dialogueText, $"A wild {enemyPokemon.Name} appeared!");
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        List<string> messages = new List<string>();
        string msg = $"{attacker.Name} used {move.ScriptableMove.Name}!" +
            $"\n{attacker.Name} attacks {defender.Name} with {move.ScriptableMove.Name}, dealing {attackInfo.damage} damage!";
        messages.Add(msg);
        if (attackInfo.criticalHit)
            messages.Add("It's a critical hit!");
        if ((Mathf.Abs(attackInfo.typeEffectiveness - 0.5f) < Mathf.Epsilon) || (Mathf.Abs(attackInfo.typeEffectiveness - 0.25f) < Mathf.Epsilon))
            messages.Add("It's not very effective...");
        else if ((Mathf.Abs(attackInfo.typeEffectiveness - 2.0f) < Mathf.Epsilon) || (Mathf.Abs(attackInfo.typeEffectiveness - 4.0f) < Mathf.Epsilon))
            messages.Add("It's super effective!");
        else if (Mathf.Abs(attackInfo.typeEffectiveness) < Mathf.Epsilon)
            messages.Add($"It doesn't affect {defender.Name} ...");
        if (attackInfo.fainted)
            messages.Add($"{defender.Name} fainted!");
        UIManager.Current.WriteDialogueTexts(dialogueText, messages);
    }

    private void OnPokemonSwitched(Pokemon oldPokemon, Pokemon newPokemon)
    {
        if (newPokemon.Owner != PokemonOwner.Player)
            return;
        string msg1 = $"{oldPokemon.Name}, come back !";
        string msg2 = $"Go {newPokemon.Name} !";
        UIManager.Current.WriteDialogueTexts(dialogueText, new List<string> { msg1, msg2 });
    }
}
