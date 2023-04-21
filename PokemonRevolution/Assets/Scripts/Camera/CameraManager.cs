using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera battleCamera;

    private void Start()
    {
        mainCamera.enabled = true;
        battleCamera.enabled = false;

        GameEvents.Instance.OnEnterBattle += OnBattleStart;
        GameEvents.Instance.OnExitBattle += OnExitBattle;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnEnterBattle -= OnBattleStart;
        GameEvents.Instance.OnExitBattle -= OnExitBattle;
    }

    private void OnBattleStart(Pokemon p1, Pokemon p2)
    {
        mainCamera.enabled = false;
        battleCamera.enabled = true;
    }

    private void OnExitBattle()
    {
        mainCamera.enabled = true;
        battleCamera.enabled = false;
    }
}
