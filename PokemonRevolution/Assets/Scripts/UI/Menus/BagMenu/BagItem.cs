using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    [SerializeField] private NavigationItem _navigationItem;

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _countText;

    [SerializeField] private Mask _backgroundMask;
    [SerializeField] private Image _background;

    public void Awake()
    {
        _navigationItem.OnSelected += OnSelected;
        _navigationItem.OnUnselected += OnUnselected;
    }

    public void OnDestroy()
    {
        _navigationItem.OnSelected -= OnSelected;
        _navigationItem.OnUnselected -= OnUnselected;
    }

    public void UpdateUI(ItemSlot itemSlot)
    {
        _icon.sprite = itemSlot.Item.Icon;
        _nameText.text = itemSlot.Item.Name;
        _countText.text = $"x {itemSlot.Count}";
    }

    public void OnSelected()
    {
        _backgroundMask.showMaskGraphic = true;
        _background.gameObject.SetActive(true);
        _nameText.color = Color.white;
        _countText.color = Color.white;
    }

    public void OnUnselected()
    {
        _backgroundMask.showMaskGraphic = false;
        _background.gameObject.SetActive(false);
        _nameText.color = Color.black;
        _countText.color = Color.black;
    }
}
