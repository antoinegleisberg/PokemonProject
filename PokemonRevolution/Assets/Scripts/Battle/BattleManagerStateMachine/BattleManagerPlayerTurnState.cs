using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerPlayerTurnState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        BattleUIEvents.Current.OnMoveSelected += OnMoveSelected;
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        BattleEvents.Current.EnterPlayerTurn();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override void OnDestroy()
    {
        BattleUIEvents.Current.OnMoveSelected -= OnMoveSelected;
    }

    private void OnMoveSelected(int moveIndex)
    {
        battleManager.PerformMove(battleManager.PlayerPokemon, battleManager.EnemyPokemon, battleManager.PlayerPokemon.Moves[moveIndex]);
        if (battleManager.EnemyPokemon.IsFainted)
        {
            battleManager.SwitchState(battleManager.EndBattleState);
        }
        else
        {
            battleManager.SwitchState(battleManager.EnemyMoveState);
        }
    }
}
