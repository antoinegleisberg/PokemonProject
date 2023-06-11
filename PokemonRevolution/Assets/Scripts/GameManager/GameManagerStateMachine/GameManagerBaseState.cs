public abstract class GameManagerBaseState
{
    protected GameManager _gameManager;

    public abstract void InitState(GameManager gameManager);
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void OnDestroy();
}
