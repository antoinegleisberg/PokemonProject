public class BattleManagerStartTurnState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    public override void EnterState()
    {
        _battleManager.SwitchState(_battleManager.ActionSelectionState);
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
