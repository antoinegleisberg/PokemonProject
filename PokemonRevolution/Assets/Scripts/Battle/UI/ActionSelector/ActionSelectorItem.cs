using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelectorItem : MonoBehaviour
{
    [SerializeField] private NavigationItem _navigationItem;
    [SerializeField] private Button _button;

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
        // TODO Update UI
    }

    private void OnUnselected()
    {
        
    }
}
