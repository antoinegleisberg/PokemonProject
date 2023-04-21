public class Move
{
    public ScriptableMove ScriptableMove { get; set; }
    public int CurrentPP { get; set; }

    public Move(ScriptableMove scriptableMove)
    {
        ScriptableMove = scriptableMove;
        CurrentPP = scriptableMove.PP;
    }
}
