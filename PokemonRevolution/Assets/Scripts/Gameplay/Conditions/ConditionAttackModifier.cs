public struct ConditionAttackModifier
{
    public bool CanPerformMove;
    public float DamageMultiplier;

    public ConditionAttackModifier(bool canPerformMove, float damageMultiplier)
    {
        CanPerformMove = canPerformMove;
        DamageMultiplier = damageMultiplier;
    }

    public ConditionAttackModifier Merge(ConditionAttackModifier other)
    {
        return new ConditionAttackModifier(CanPerformMove && other.CanPerformMove, DamageMultiplier * other.DamageMultiplier);
    }
}
