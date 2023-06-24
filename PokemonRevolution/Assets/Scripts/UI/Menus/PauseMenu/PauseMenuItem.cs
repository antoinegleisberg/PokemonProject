using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuItem : MonoBehaviour
{
    private Button _button;
    private Image _icon;

    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Sprite _selectedSprite;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _icon = GetComponentInChildren<Image>();

        NavigationItem navigationItem = GetComponent<NavigationItem>();
        navigationItem.OnSelected += Select;
        navigationItem.OnUnselected += Unselect;
        navigationItem.OnSubmitted += Click;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        NavigationItem navigationItem = GetComponent<NavigationItem>();
        navigationItem.OnSelected -= Select;
        navigationItem.OnUnselected -= Unselect;
        navigationItem.OnSubmitted -= Click;
    }

    private void Select()
    {
        _icon.sprite = _selectedSprite;
    }

    private void Unselect()
    {
        _icon.sprite = _unselectedSprite;
    }

    private void Click()
    {
        _button.onClick.Invoke();
    }
}
