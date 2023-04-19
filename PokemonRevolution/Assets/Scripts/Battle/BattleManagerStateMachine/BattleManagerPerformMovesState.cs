using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerPerformMovesState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager) => this.battleManager = battleManager;

    public override void EnterState()
    {
        PerformAction(battleManager.NextPlayerAction);

        if ((battleManager.NextPlayerAction.BattleAction != BattleAction.Run) &&
                (!battleManager.NextEnemyAction.SourcePokemon.IsFainted))
            PerformAction(battleManager.NextEnemyAction);        
        
        battleManager.SwitchState(battleManager.EndTurnState);
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void OnDestroy() { }

    private void PerformAction(BattleActionInfo battleActionInfo)
    {
        switch (battleActionInfo.BattleAction)
        {
            case BattleAction.Attack:
                int moveIndex = battleActionInfo.ActionParameter;
                Move move = battleActionInfo.SourcePokemon.Moves[moveIndex];
                battleManager.PerformMove(battleActionInfo.SourcePokemon, battleActionInfo.TargetPokemon, move);
                break;

            default:
                Debug.Log("Used something else than an attack move : not implemented yet");
                break;
        }
        
    }
}
