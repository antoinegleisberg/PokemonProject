using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerEnemyMoveState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        BattleEvents.Current.EnterEnemyTurn();
        Move randomMove = battleManager.EnemyPokemon.Moves[Random.Range(0, battleManager.EnemyPokemon.Moves.Count)];
        battleManager.PerformMove(battleManager.EnemyPokemon, battleManager.PlayerPokemon, randomMove);
        if (battleManager.PlayerPokemon.IsFainted)
        {
            Pokemon oldPokemon = battleManager.PlayerPokemon;
            Pokemon nextPokemon = battleManager.PlayerParty.GetFirstPokemon();
            if (nextPokemon != null)
            {
                battleManager.PlayerPokemon = nextPokemon;
                BattleEvents.Current.PokemonSwitched(oldPokemon, nextPokemon);
                battleManager.SwitchState(battleManager.PlayerTurnState);
            }
            else
            {
                battleManager.SwitchState(battleManager.EndBattleState);
            }
        }
        else
            battleManager.SwitchState(battleManager.PlayerTurnState);
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
