public struct AttackInfo
{
    public bool CriticalHit;
    public float Damage;
    public float TypeEffectiveness;
    public bool MoveHits;

    public AttackInfo(bool criticalHit, float damage, float typeEffectiveness, bool moveHits)
    {
        CriticalHit = criticalHit;
        Damage = damage;
        TypeEffectiveness = typeEffectiveness;
        MoveHits = moveHits;
    }
}
