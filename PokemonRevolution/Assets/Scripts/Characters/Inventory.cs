using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Only for initialisation
    [SerializeField] private List<ItemSlot> _initialSlots;

    private Dictionary<BagCategory, List<ItemSlot>> _slots;

    private void Awake()
    {
        _slots = new Dictionary<BagCategory, List<ItemSlot>>();

        foreach (ItemSlot slot in _initialSlots)
        {
            AddItem(slot.Item, slot.Count);
        }
    }

    public void AddItem(ItemBase item, int count = 1)
    {
        BagCategory category = item.BagCategory;
        if (!_slots.ContainsKey(category))
        {
            _slots.Add(category, new List<ItemSlot>());
        }
        for (int i=0; i < _slots[category].Count; i++)
        {
            ItemSlot slot = _slots[category][i];
            if (slot.Item == item)
            {
                slot.Count += count;
                _slots[category][i] = slot;
                return;
            }
        }
        _slots[category].Add(new ItemSlot(item, count));
    }

    public List<ItemSlot> GetSlots(BagCategory category)
    {
        if (_slots.ContainsKey(category))
        {
            return _slots[category];
        }
        return new List<ItemSlot>();
    }

    public void UseItem(BagCategory category, int itemIndex, Pokemon target)
    {
        ItemBase item = _slots[category][itemIndex].Item;
        
        if (!item.CanUse(target))
        {
            return;
        }

        item.Use(target);
        RemoveItem(category, item);
    }

    public void RemoveItemAt(BagCategory category, int itemIndex, int count = 1)
    {
        ItemSlot slot = _slots[category][itemIndex];
        slot.Count -= count;
        if (slot.Count <= 0)
        {
            _slots[category].RemoveAt(itemIndex);
        }
        else
        {
            // Because a struct is passed as value, not as reference,
            // we have to reassign the slot in the list
            _slots[category][itemIndex] = slot;
        }
    }

    public void RemoveItem(BagCategory category, ItemBase item, int count = 1)
    {
       for (int i = 0; i < _slots[category].Count; i++)
       {
            ItemSlot slot = _slots[category][i];
            if (item != slot.Item)
            {
                continue;
            }

            RemoveItemAt(category, i, count);
            break;
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