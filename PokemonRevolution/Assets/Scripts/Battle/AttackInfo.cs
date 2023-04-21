public struct AttackInfo
{
    public bool criticalHit;
    public int damage;
    public float typeEffectiveness;
    public bool fainted;

    public AttackInfo(bool criticalHit, int damage, float typeEffectiveness, bool fainted)
    {
        this.criticalHit = criticalHit;
        this.damage = damage;
        this.typeEffectiveness = typeEffectiveness;
        this.fainted = fainted;
    }
}
