public class GameManagerFreeRoamState : GameManagerBaseState
{
    public override void InitState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public override void EnterState()
    {
        GameEvents.Instance.EnterFreeRoam();
    }

    public override void UpdateState()
    {

    }


    public override void ExitState()
    {
        GameEvents.Instance.ExitFreeRoam();
    }

    public override void OnDestroy()
    {

    }
}
