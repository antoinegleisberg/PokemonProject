using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerEndBattleState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        battleManager.StartCoroutine(EndBattleCoroutine());
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

    private IEnumerator EndBattleCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        while (UIManager.Instance.IsBusy) yield return null;

        GameEvents.Instance.ExitBattle();
        battleManager.SwitchState(battleManager.OutOfBattleState);
    }
}
