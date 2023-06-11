public class GameManagerBattleState : GameManagerBaseState
{
    public override void InitState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public override void EnterState()
    {
        GameEvents.Instance.EnterBattle();
    }

    public override void UpdateState()
    {
        
    }


    public override void ExitState()
    {
        GameEvents.Instance.ExitBattle();
    }

    public override void OnDestroy()
    {

    }
}
