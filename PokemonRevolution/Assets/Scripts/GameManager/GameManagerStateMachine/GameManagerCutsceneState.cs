public class GameManagerCutsceneState : GameManagerBaseState
{
    public override void InitState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public override void EnterState()
    {
        GameEvents.Instance.EnterCutscene();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        GameEvents.Instance.ExitCutscene();
    }

    public override void OnDestroy()
    {

    }
}
