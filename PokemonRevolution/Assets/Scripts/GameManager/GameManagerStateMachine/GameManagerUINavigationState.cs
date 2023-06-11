public class GameManagerUINavigationState : GameManagerBaseState
{
    public override void InitState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public override void EnterState()
    {
        GameEvents.Instance.EnterMenu();    
    }

    public override void UpdateState()
    {

    }
    
    public override void ExitState()
    {
        GameEvents.Instance.ExitMenu();
    }

    public override void OnDestroy()
    {

    }
}
