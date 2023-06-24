using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _pauseMenuNavigator;

    private void Start()
    {
        _pauseMenuNavigator.OnCancelled += GameManager.Instance.CloseMenu;
    }

    private void OnDestroy()
    {
        _pauseMenuNavigator.OnCancelled -= GameManager.Instance.CloseMenu;
    }
}
