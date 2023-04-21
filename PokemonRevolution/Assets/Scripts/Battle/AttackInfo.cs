public struct AttackInfo
{
    public bool criticalHit;
    public int damage;
    public float typeEffectiveness;

    public AttackInfo(bool criticalHit, int damage, float typeEffectiveness)
    {
        this.criticalHit = criticalHit;
        this.damage = damage;
        this.typeEffectiveness = typeEffectiveness;
    }
}
