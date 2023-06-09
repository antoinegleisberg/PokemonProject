using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerDialogueState : GameManagerBaseState
{
    public override void InitState(GameManager gameManager)
    {
        this.gameManager = gameManager;
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
