using System;
using UnityEngine;

[Serializable]
public class LearnableMove
{
    [SerializeField] private ScriptableMove _move;
    [SerializeField] private int _level;

    public ScriptableMove Move { get => _move; }
    public int Level { get => _level; }
}
