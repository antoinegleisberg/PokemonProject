public struct AttackInfo
{
    public bool criticalHit;
    public int damage;
    public float typeEffectiveness;
    public bool moveHits;

    public AttackInfo(bool criticalHit, int damage, float typeEffectiveness, bool moveHits)
    {
        this.criticalHit = criticalHit;
        this.damage = damage;
        this.typeEffectiveness = typeEffectiveness;
        this.moveHits = moveHits;
    }
}
