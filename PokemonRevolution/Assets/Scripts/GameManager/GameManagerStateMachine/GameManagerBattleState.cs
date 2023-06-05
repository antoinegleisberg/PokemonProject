using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBattleState : GameManagerBaseState
{
    public override void InitState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void EnterState()
    {
        GameEvents.Instance.EnterBattle();
    }

    public override void UpdateState()
    {
        
    }


    public override void ExitState()
    {
        GameEvents.Instance.ExitBattle();
    }

    public override void OnDestroy()
    {

    }
}
