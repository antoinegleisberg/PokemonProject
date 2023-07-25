using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItem : ItemBase
{
    public override bool CanUse(Pokemon target)
    {
        return true;
    }

    public override void Use(Pokemon target)
    {
        
    }
}
