using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerStartState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        GameEvents.Current.EnterBattle(battleManager.PlayerPokemon, battleManager.EnemyPokemon);
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
