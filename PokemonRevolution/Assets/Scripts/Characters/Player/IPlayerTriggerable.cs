using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerTriggerable
{
    public abstract void OnPlayerTriggered(PlayerController playerController);
}
