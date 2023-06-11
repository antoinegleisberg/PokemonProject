using UnityEngine;


[System.Serializable]
public struct NPCMovementPattern
{
    public Direction LookTowards;
    public Vector2Int Movement;
    public bool Run;
    public float WaitTime;
}