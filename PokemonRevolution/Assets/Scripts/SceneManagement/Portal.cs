using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController playerController)
    {
        Debug.Log("Switch scenes");
    }
}
