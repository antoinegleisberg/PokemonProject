using System.Collections;
using UnityEngine;

public class BattleManagerEndBattleState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    public override void EnterState()
    {
        _battleManager.StartCoroutine(EndBattleCoroutine());
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
        _battleManager.PlayerPokemon.OnExitBattle();
        _battleManager.EnemyPokemon.OnExitBattle();

        if (_battleManager.IsTrainerBattle)
        {
            Pokemon enemyPokemon = _battleManager.EnemyParty.GetFirstPokemon();
            bool enemyLost = enemyPokemon == null;

            _battleManager.EnemyTrainer.CanBattle = !enemyLost;
        }

        yield return BattleUIManager.Instance.WaitWhileBusy();
        
        _battleManager.SwitchState(_battleManager.OutOfBattleState);
        GameManager.Instance.SwitchState(GameManager.Instance.FreeRoamState);
    }
}
