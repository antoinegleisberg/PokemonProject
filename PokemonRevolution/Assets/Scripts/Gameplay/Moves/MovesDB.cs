using System.Collections.Generic;
using UnityEngine;

public static class MovesDB
{
    private static Dictionary<string, ScriptableMove> _moves;
    
    public static void Init()
    {
        string path = "Moves";
        ScriptableMove[] scriptableMoves = Resources.LoadAll<ScriptableMove>(path);
        _moves = new Dictionary<string, ScriptableMove>();
        foreach (ScriptableMove scriptableMove in scriptableMoves)
        {
            _moves.Add(scriptableMove.Name, scriptableMove);
        }
    }

    public static ScriptableMove GetScriptableMoveByName(string name)
    {
        if (!_moves.ContainsKey(name))
        {
            Debug.LogError($"Couldn't find move {name}");
        }

        return _moves[name];
    }
}
