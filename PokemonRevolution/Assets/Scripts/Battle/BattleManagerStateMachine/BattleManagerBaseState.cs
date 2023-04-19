using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleManagerBaseState
{
    protected BattleManager battleManager;
    public abstract void InitState(BattleManager battleManager);
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void OnDestroy();
}
