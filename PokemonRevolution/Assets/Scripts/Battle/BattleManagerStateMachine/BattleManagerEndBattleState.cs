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

        if (battleManager.IsTrainerBattle)
        {
            Pokemon enemyPokemon = battleManager.EnemyParty.GetFirstPokemon();
            bool enemyLost = enemyPokemon == null;

            battleManager.EnemyTrainer.CanBattle = !enemyLost;
        }

        yield return BattleUIManager.Instance.WaitWhileBusy();
        
        battleManager.SwitchState(battleManager.OutOfBattleState);
        GameManager.Instance.SwitchState(GameManager.Instance.FreeRoamState);
    }
}
