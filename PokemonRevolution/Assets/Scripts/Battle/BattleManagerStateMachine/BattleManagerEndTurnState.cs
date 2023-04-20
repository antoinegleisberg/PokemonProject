using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerEndTurnState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;

        BattleUIEvents.Instance.OnReplacePokemonSelected += OnReplacingPokemonSelected;
    }

    public override void EnterState()
    {
        CheckFaintedPokemons();
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void OnDestroy()
    {
        BattleUIEvents.Instance.OnReplacePokemonSelected -= OnReplacingPokemonSelected;
    }

    private void CheckFaintedPokemons()
    {
        bool playerPokemonFainted = battleManager.PlayerPokemon.IsFainted;
        if (playerPokemonFainted)
        {
            Debug.Log("Player pokemon fainted ....");
            if (battleManager.PlayerParty.GetFirstPokemon() == null)
            {
                Debug.Log("Player has no more pokemon to switch to ! The battle is lost"); 
                battleManager.SwitchState(battleManager.EndBattleState);
            }
            else
            {
                Debug.Log("Waiting for replacement");
                BattleEvents.Instance.ReplaceFaintedPokemon();
            }
        }
        else
        {
            ReplaceEnemyFaintedPokemon();
        }
    }

    private void OnReplacingPokemonSelected(int pokemonIndex)
    {
        Debug.Log("Replacing selected pokemon !");
        Pokemon nextPokemon = battleManager.PlayerParty.Pokemons[pokemonIndex];
        battleManager.SwitchPokemon(battleManager.PlayerPokemon, nextPokemon);

        ReplaceEnemyFaintedPokemon();
    }

    private void ReplaceEnemyFaintedPokemon()
    {
        if (battleManager.EnemyPokemon.IsFainted)
        {
            Pokemon nextPokemon = battleManager.EnemyParty.GetFirstPokemon();
            if (nextPokemon == null)
            {
                battleManager.SwitchState(battleManager.EndBattleState);
                return;
            }
            battleManager.SwitchPokemon(battleManager.EnemyPokemon, nextPokemon);
        }
        battleManager.SwitchState(battleManager.StartTurnState);
    }
}
