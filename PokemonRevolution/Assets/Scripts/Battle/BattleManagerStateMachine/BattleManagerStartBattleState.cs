public class BattleManagerStartBattleState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        battleManager.BattleActionSelectorsUIManager.OnBattleStart(battleManager.PlayerParty);
        battleManager.BattleDialogueUIManager.OnBattleStart(battleManager.PlayerParty, battleManager.EnemyParty);
        battleManager.BattleSceneUIManager.OnBattleStart(battleManager.PlayerParty, battleManager.EnemyParty);

        battleManager.SwitchState(battleManager.StartTurnState);
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
