using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerStartTurnState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        battleManager.SwitchState(battleManager.ActionSelectionState);
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
