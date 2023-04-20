using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> statBoosts;
    [SerializeField] List<AppliedVolatileStatusEffect> appliedvolatileStatusEffects;
    [SerializeField] List<AppliedNonVolatileStatusEffect> appliedNonVolatileStatusEffects;

    public List<StatBoost> StatBoosts { get => statBoosts; }
    public List<AppliedVolatileStatusEffect> AppliedVolatileStatusEffects { get => appliedvolatileStatusEffects; }
    public List<AppliedNonVolatileStatusEffect> AppliedNonVolatileStatusEffects
    {
        get => appliedNonVolatileStatusEffects;
    }
}


[System.Serializable]
public struct AppliedNonVolatileStatusEffect
{
    public int probability;
    public NonVolatileStatus nonVolatileStatus;
    public List<MoveTarget> statusEffectTarget;
}

[System.Serializable]
public struct AppliedVolatileStatusEffect
{
    public int probability;
    public VolatileStatus volatileStatuses;
    public List<MoveTarget> statusEffectTarget;
}

[System.Serializable]
public struct StatBoost
{
    public int probability;
    public Stat stat;
    public int boostValue;
    public List<MoveTarget> statBoostTarget;
}