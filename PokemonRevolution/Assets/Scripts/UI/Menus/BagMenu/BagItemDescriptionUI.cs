using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagItemDescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _icon;

    public void UpdateUI(ItemBase item)
    {
        if (item == null)
        {
            _nameText.text = "";
            _description.text = "";
            _icon.color = new Color(1, 1, 1, 0);
        }
        else
        {
            _nameText.text = item.Name;
            _description.text = item.Description;
            _icon.sprite = item.Icon;
            _icon.color = Color.white;
        }
    }
}
