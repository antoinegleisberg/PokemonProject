using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] private new string name;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private BagCategory bagCategory;

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public BagCategory BagCategory { get => bagCategory; set => bagCategory = value; }


    public abstract bool CanUse(Pokemon target);
    public abstract void Use(Pokemon target);
}

public enum BagCategory
{
    HeldItem,
    Medicine,
    Pokeball,
    TM,
    // Other,
    Berries,
    // EvolutionItems,
    // MegaStones,
    BattleItems,
    // Treasures,
    KeyItems,
}