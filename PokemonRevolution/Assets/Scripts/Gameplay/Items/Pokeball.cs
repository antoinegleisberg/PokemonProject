using UnityEngine;

[CreateAssetMenu(fileName = "New Pokeball", menuName = "Items/Pokeball")]
public class Pokeball : ItemBase
{
    [SerializeField] private float catchRateModifier;

    public override bool CanUse(Pokemon target)
    {
        return true;
    }

    public override void Use(Pokemon target)
    {
        
    }
}
