using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public List<ItemSlot> Slots { get; private set; }

    public void UseItem(int itemIndex, Pokemon target)
    {
        ItemBase item = Slots[itemIndex].Item;
        
        if (!item.CanUse(target))
        {
            return;
        }

        item.Use(target);
        RemoveItem(item);
    }

    public void RemoveItem(ItemBase item, int count = 1)
    {
       for (int i = 0; i < Slots.Count; i++)
       {
            ItemSlot slot = Slots[i];
            if (item == slot.Item)
            {
                slot.Count -= count;
                Slots[i] = slot;  // Because a struct is passed as value, not as reference
                if (slot.Count <= 0)
                {
                    Slots.RemoveAt(i);
                    break;
                }
            }
       }
    }
}


[System.Serializable]
public struct ItemSlot
{
    public ItemBase Item;
    public int Count;

    public ItemSlot(ItemBase item, int count)
    {
        Item = item;
        Count = count;
    }
}