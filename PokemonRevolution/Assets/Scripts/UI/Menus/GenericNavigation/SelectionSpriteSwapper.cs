using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSpriteSwapper : MonoBehaviour
{
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;

    [SerializeField] private NavigationItem _navigationItem;

    [SerializeField] private Image _image;


    private void OnEnable()
    {
        _navigationItem.OnSelected += OnSelected;
        _navigationItem.OnUnselected += OnUnselected;
    }

    private void OnDisable()
    {
        _navigationItem.OnSelected -= OnSelected;
        _navigationItem.OnUnselected -= OnUnselected;
    }

    private void OnSelected()
    {
        _image.sprite = _selectedSprite;
    }

    private void OnUnselected()
    {
        _image.sprite = _unselectedSprite;
    }
}
