public class BattleManagerStartBattleState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        GameEvents.Instance.EnterBattle(battleManager.PlayerPokemon, battleManager.EnemyPokemon);
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
