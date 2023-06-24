using UnityEngine;

[CreateAssetMenu(fileName = "HealingItem", menuName = "Items/Healing Item")]
public class HealingItem : ItemBase
{
    [Header("HP")]
    [SerializeField] private int healthRecovered;
    [SerializeField] private bool maxHealthRecovered;

    [Header("PP")]
    [SerializeField] private int ppRecovered;
    [SerializeField] private bool maxPPRecovered;
    [SerializeField] private bool recoversAllMovesPP;

    [Header("Status")]
    [SerializeField] private StatusCondition statusHealed;
    [SerializeField] private bool recoversAllStatus;

    [Header("Revive")]
    [SerializeField] private bool isRevive;
    [SerializeField] private bool isMaxRevive;

    public int HealthRecovered { get => healthRecovered; }
    public bool MaxHealthRecovered { get => maxHealthRecovered; }

    public int PPRecovered { get => ppRecovered; }
    public bool MaxPPRecovered { get => maxPPRecovered; }
    public bool RecoversAllMovesPP { get => recoversAllMovesPP; }

    public StatusCondition StatusHealed { get => statusHealed; }
    public bool RecoversAllStatus { get => recoversAllStatus; }

    public bool IsRevive { get => isRevive; }
    public bool IsMaxRevive { get => isMaxRevive; }


    public override bool CanUse(Pokemon target)
    {
        if (target.IsFainted && !IsRevive && !IsMaxRevive)
        {
            return false;
        }

        if (HealthRecovered > 0 || MaxHealthRecovered)
        {
            if (target.CurrentHP < target.MaxHP)
            {
                return true;
            }
        }

        if (PPRecovered > 0 || MaxPPRecovered)
        {
            foreach (Move move in target.Moves)
            {
                if (move.CurrentPP < move.ScriptableMove.PP)
                {
                    return true;
                }
            }
        }

        if (StatusHealed != StatusCondition.None)
        {
            if (target.StatusCondition == StatusHealed)
            {
                return true;
            }
        }
        if (RecoversAllStatus)
        {
            if (target.StatusCondition != StatusCondition.None)
            {
                return true;
            }
        }

        if (IsRevive || IsMaxRevive)
        {
            if (target.IsFainted)
            {
                return true;
            }
        }

        return false;
    }

    public override void Use(Pokemon target)
    {
        if (target.IsFainted && !IsRevive && !IsMaxRevive)
        {
            return;
        }

        if (HealthRecovered > 0)
        {
            target.HealHP(HealthRecovered);
        }
        if (MaxHealthRecovered)
        {
            target.HealHP(target.MaxHP);
        }

        if (PPRecovered > 0)
        {
            if (RecoversAllMovesPP)
            {
                foreach (Move move in target.Moves)
                {
                    move.RecoverPP(PPRecovered);
                }
            }
            // Todo: implement recovery for a single move
        }
        if (MaxPPRecovered)
        {
            if (RecoversAllMovesPP)
            {
                foreach (Move move in target.Moves)
                {
                    move.RecoverPP(move.ScriptableMove.PP);
                }
            }
            // Todo: implement recovery for a single move
        }

        if (StatusHealed != StatusCondition.None)
        {
            if (target.StatusCondition == StatusHealed)
            {
                target.CureStatus();
            }
        }
        if (RecoversAllStatus)
        {
            if (target.StatusCondition != StatusCondition.None)
            {
                target.CureStatus();
            }
        }

        if (IsRevive)
        {
            if (target.IsFainted)
            {
                target.CureStatus();
                target.HealHP(target.MaxHP / 2);
            }
        }
        if (IsMaxRevive)
        {
            if (target.IsFainted)
            {
                target.CureStatus();
                target.HealHP(target.MaxHP);
            }
        }
    }
}
