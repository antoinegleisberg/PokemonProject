using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    private void Awake() => Instance = this;
    
    public event Action OnEnterBattle;
    public event Action OnExitBattle;

    public event Action OnEnterDialogue;
    public event Action OnExitDialogue;

    public event Action OnEnterMenu;
    public event Action OnExitMenu;

    public event Action OnEnterCutscene;
    public event Action OnExitCutscene;

    public event Action OnEnterFreeRoam;
    public event Action OnExitFreeRoam;

    public void EnterBattle() => OnEnterBattle?.Invoke();
    public void ExitBattle() => OnExitBattle?.Invoke();

    public void EnterDialogue() => OnEnterDialogue?.Invoke();
    public void ExitDialogue() => OnExitDialogue?.Invoke();

    public void EnterMenu() => OnEnterMenu?.Invoke();
    public void ExitMenu() => OnExitMenu?.Invoke();

    public void EnterCutscene() => OnEnterCutscene?.Invoke();
    public void ExitCutscene() => OnExitCutscene?.Invoke();

    public void EnterFreeRoam() => OnEnterFreeRoam?.Invoke();
    public void ExitFreeRoam() => OnExitFreeRoam?.Invoke();
}
