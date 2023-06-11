using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _battleCamera;

    private void Start()
    {
        _mainCamera.enabled = true;
        _battleCamera.enabled = false;

        GameEvents.Instance.OnEnterBattle += OnEnterBattle;
        GameEvents.Instance.OnExitBattle += OnExitBattle;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnEnterBattle -= OnEnterBattle;
        GameEvents.Instance.OnExitBattle -= OnExitBattle;
    }

    private void OnEnterBattle()
    {
        _mainCamera.enabled = false;
        _battleCamera.enabled = true;
    }

    private void OnExitBattle()
    {
        _mainCamera.enabled = true;
        _battleCamera.enabled = false;
    }
}
