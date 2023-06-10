using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    [SerializeField] private Sprite unselectedSprite;
    [SerializeField] private Sprite selectedSprite;

    public void Select()
    {
        icon.sprite = selectedSprite;
    }

    public void Unselect()
    {
        icon.sprite = unselectedSprite;
    }

    public void Click()
    {
        button.onClick.Invoke();
    }
}
