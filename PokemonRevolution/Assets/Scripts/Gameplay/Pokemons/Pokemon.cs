using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pokemon
{
    public ScriptablePokemon ScriptablePokemon { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public List<Move> Moves { get; private set; }
    public int CurrentHP { get; private set; }
    public PokemonOwner Owner { get; private set; }

    public Pokemon(ScriptablePokemon scriptablePokemon, int level, PokemonOwner owner) : this(scriptablePokemon, level, owner, "") { }

    public Pokemon(ScriptablePokemon scriptablePokemon, int level, PokemonOwner owner, string name)
    {
        ScriptablePokemon = scriptablePokemon;
        Level = level;
        CurrentHP = MaxHealthPoints;
        Owner = owner;
        Name = name;
        if (Name == "") Name = scriptablePokemon.Name;

        Moves = new List<Move>();
        foreach (LearnableMove move in scriptablePokemon.LearnableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Move(move.Move));
            }
        }
        if (Moves.Count > 4)
        {
            // start at 0 or 4 changes removing the first 4 moves or the last 4 moves
            Moves.RemoveRange(0, Moves.Count - 4);
        }
    }

    public int Attack { get { return Mathf.FloorToInt((ScriptablePokemon.BaseAttack * Level) / 100.0f) + 5; } }
    public int Defense { get { return Mathf.FloorToInt((ScriptablePokemon.BaseDefense * Level) / 100.0f) + 5; } }
    public int SpecialAttack { get { return Mathf.FloorToInt((ScriptablePokemon.BaseSpecialAttack * Level) / 100.0f) + 5; } }
    public int SpecialDefense { get { return Mathf.FloorToInt((ScriptablePokemon.BaseSpecialDefense * Level) / 100.0f) + 5; } }
    public int Speed { get { return Mathf.FloorToInt((ScriptablePokemon.BaseSpeed * Level) / 100.0f) + 5; } }
    public int MaxHealthPoints { get { return Mathf.FloorToInt((ScriptablePokemon.BaseHealthPoints * Level) / 100.0f) + 10; } }

    public bool IsFainted { get { return CurrentHP <= 0; } }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;
    }

    public void LoseMovePP(Move move)
    {
        foreach (Move m in Moves)
        {
            if (m.ScriptableMove.Name == move.ScriptableMove.Name) m.CurrentPP--;
        }
    }
}

public enum PokemonOwner {
    Player,
    EnemyTrainer,
    AllyTrainer,
    Wild
}