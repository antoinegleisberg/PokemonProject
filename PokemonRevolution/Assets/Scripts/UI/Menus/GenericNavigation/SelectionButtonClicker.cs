using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButtonClicker : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private NavigationItem _navigationItem;

    private void OnEnable()
    {
        _navigationItem.OnSubmitted += Click;
    }

    private void OnDisable()
    {
        _navigationItem.OnSubmitted -= Click;
    }

    private void Click()
    {
        _button.onClick.Invoke();
    }
}
