using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public ScriptableMove scriptableMove { get; set; }
    public int currentPP { get; set; }

    public Move(ScriptableMove scriptableMove)
    {
        this.scriptableMove = scriptableMove;
        this.currentPP = scriptableMove.PP;
    }
}
