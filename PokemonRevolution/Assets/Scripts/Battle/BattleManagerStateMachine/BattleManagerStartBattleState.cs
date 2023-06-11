public class BattleManagerStartBattleState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    public override void EnterState()
    {
        _battleManager.BattleActionSelectorsUIManager.OnBattleStart(_battleManager.PlayerParty);
        _battleManager.BattleDialogueUIManager.OnBattleStart(_battleManager.PlayerParty, _battleManager.EnemyParty);
        _battleManager.BattleSceneUIManager.OnBattleStart(_battleManager.PlayerParty, _battleManager.EnemyParty);

        _battleManager.SwitchState(_battleManager.StartTurnState);
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
