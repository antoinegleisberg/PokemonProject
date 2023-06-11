using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    private Button _button;
    private Image _icon;

    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Sprite _selectedSprite;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _icon = GetComponentInChildren<Image>();
    }

    public void Select()
    {
        _icon.sprite = _selectedSprite;
    }

    public void Unselect()
    {
        _icon.sprite = _unselectedSprite;
    }

    public void Click()
    {
        _button.onClick.Invoke();
    }
}
