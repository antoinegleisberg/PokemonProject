using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _settingsNavigator;

    private void OnEnable()
    {
        _settingsNavigator.OnCancelled += UIManager.Instance.OpenPauseMenu;
    }

    private void OnDisable()
    {
        _settingsNavigator.OnCancelled -= UIManager.Instance.OpenPauseMenu;
    }
}
