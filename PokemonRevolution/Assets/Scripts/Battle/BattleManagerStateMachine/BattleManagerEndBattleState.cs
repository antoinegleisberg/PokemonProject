using System.Collections;
using UnityEngine;

public class BattleManagerEndBattleState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        battleManager.PlayerPokemon.OnExitBattle();
        battleManager.EnemyPokemon.OnExitBattle();
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
        yield return UIManager.Instance.WaitWhileBusy();

        GameEvents.Instance.ExitBattle();
        battleManager.SwitchState(battleManager.OutOfBattleState);
    }
}
