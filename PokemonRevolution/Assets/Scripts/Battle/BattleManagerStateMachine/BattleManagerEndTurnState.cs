using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerEndTurnState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        if (battleManager.PlayerPokemon.IsFainted)
        {
            Debug.Log("Player pokemon fainted! Switching to next Pokemon");
            Pokemon nextPokemon = battleManager.PlayerParty.GetFirstPokemon();
            if (nextPokemon == null)
            {
                battleManager.SwitchState(battleManager.EndBattleState);
                return;
            }
            battleManager.PlayerPokemon = nextPokemon;
            BattleEvents.Instance.PokemonSwitchedIn(nextPokemon);
        }

        if (battleManager.EnemyPokemon.IsFainted)
        {
            Debug.Log("Enemy pokemon fainted! Switching to next Pokemon");
            Pokemon nextPokemon = battleManager.EnemyParty.GetFirstPokemon();
            if (nextPokemon == null)
            {
                battleManager.SwitchState(battleManager.EndBattleState);
                return;
            }
            battleManager.EnemyPokemon = nextPokemon;
            BattleEvents.Instance.PokemonSwitchedIn(nextPokemon);
        }

        Debug.Log("Going back to start turn state");
        battleManager.SwitchState(battleManager.StartTurnState);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void OnDestroy()
    {

    }
}
