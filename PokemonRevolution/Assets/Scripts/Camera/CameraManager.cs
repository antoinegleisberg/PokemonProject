using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera battleCamera;

    private void Start()
    {
        mainCamera.enabled = true;
        battleCamera.enabled = false;

        GameEvents.Current.OnEnterBattle += OnBattleStart;
        GameEvents.Current.OnExitBattle += OnExitBattle;
    }

    private void OnDestroy()
    {
        GameEvents.Current.OnEnterBattle -= OnBattleStart;
        GameEvents.Current.OnExitBattle -= OnExitBattle;
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
