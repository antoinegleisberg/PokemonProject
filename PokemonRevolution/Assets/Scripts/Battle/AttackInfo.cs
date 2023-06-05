public struct AttackInfo
{
    public bool criticalHit;
    public float damage;
    public float typeEffectiveness;
    public bool moveHits;

    public AttackInfo(bool criticalHit, float damage, float typeEffectiveness, bool moveHits)
    {
        this.criticalHit = criticalHit;
        this.damage = damage;
        this.typeEffectiveness = typeEffectiveness;
        this.moveHits = moveHits;
    }
}
