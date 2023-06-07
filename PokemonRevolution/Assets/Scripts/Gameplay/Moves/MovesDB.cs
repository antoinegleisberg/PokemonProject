using System.Collections.Generic;
using UnityEngine;

public static class MovesDB
{
    private static Dictionary<string, ScriptableMove> moves;
    
    public static void Init()
    {
        string path = "Moves";
        ScriptableMove[] scriptableMoves = Resources.LoadAll<ScriptableMove>(path);
        moves = new Dictionary<string, ScriptableMove>();
        foreach (ScriptableMove scriptableMove in scriptableMoves)
        {
            moves.Add(scriptableMove.Name, scriptableMove);
        }
    }

    public static ScriptableMove GetScriptableMoveByName(string name)
    {
        if (!moves.ContainsKey(name))
        {
            Debug.LogError($"Couldn't find move {name}");
        }

        return moves[name];
    }
}
