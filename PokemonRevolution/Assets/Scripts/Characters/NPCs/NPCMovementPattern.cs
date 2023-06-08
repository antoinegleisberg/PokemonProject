using UnityEngine;


[System.Serializable]
public struct NPCMovementPattern
{
    public Direction lookTowards;
    public Vector2Int movement;
    public bool run;
    public float waitTime;
}