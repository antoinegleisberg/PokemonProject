using System;

public class StatusConditionData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    public string EndMessage { get; set; }
    public bool IsVolatile { get; set; }

    public Action<Pokemon> OnBattleTurnEnd { get; set; }
}
