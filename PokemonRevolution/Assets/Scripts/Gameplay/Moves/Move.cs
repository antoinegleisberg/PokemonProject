using UnityEngine;

public class Move
{
    public ScriptableMove ScriptableMove { get; set; }
    public int CurrentPP { get; set; }

    public Move(ScriptableMove scriptableMove)
    {
        ScriptableMove = scriptableMove;
        CurrentPP = scriptableMove.PP;
    }

    public Move(MoveSaveData moveSaveData)
    {
        ScriptableMove scriptableMove = MovesDB.GetScriptableMoveByName(moveSaveData.Name);
        ScriptableMove = scriptableMove;
        CurrentPP = moveSaveData.PP;
    }

    public MoveSaveData GetSaveData()
    {
        return new MoveSaveData(ScriptableMove.Name, CurrentPP);
    }
}
