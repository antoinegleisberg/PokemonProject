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
        battleManager.PlayerPokemon.OnExitBattle();
        battleManager.EnemyPokemon.OnExitBattle();

        yield return UIManager.Instance.WaitWhileBusy();

        GameEvents.Instance.ExitBattle();
        battleManager.SwitchState(battleManager.OutOfBattleState);
    }
}
