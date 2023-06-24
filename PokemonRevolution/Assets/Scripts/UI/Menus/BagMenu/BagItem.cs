using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _countText;

    public void UpdateUI(ItemSlot itemSlot)
    {
        _nameText.text = itemSlot.Item.Name;
        _countText.text = $"x {itemSlot.Count}";
    }
}
