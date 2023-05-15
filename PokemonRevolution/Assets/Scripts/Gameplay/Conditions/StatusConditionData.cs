using System;
using UnityEngine;

public class StatusConditionData
{
    public string Name { get; set; }
    public string HUDName { get; set; }
    public Color HUDColor { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    public string EndMessage { get; set; }
    public bool IsVolatile { get; set; }
    public float CatchRateModifier { get; set; }

    public Func<Pokemon, Stat, float> OnGetStat { get; set; }
    public Action<Pokemon> OnStart { get; set; }
    public Func<Pokemon, Move, ConditionAttackModifier> OnBeforeMove { get; set; }
    public Action<Pokemon> OnBattleTurnEnd { get; set; }
}
