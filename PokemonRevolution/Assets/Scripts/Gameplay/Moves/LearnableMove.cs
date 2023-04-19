using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LearnableMove
{
    [SerializeField] private ScriptableMove move;
    [SerializeField] private int level;

    public ScriptableMove Move { get => move; }
    public int Level { get => level; }
}
