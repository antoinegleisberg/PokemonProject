public class GameManagerDialogueState : GameManagerBaseState
{
    public override void InitState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public override void EnterState()
    {
        GameEvents.Instance.EnterDialogue();
    }

    public override void UpdateState()
    {

    }


    public override void ExitState()
    {
        GameEvents.Instance.ExitDialogue();
    }

    public override void OnDestroy()
    {

    }
}
