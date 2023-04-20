using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pokemon
{
    private readonly static Dictionary<int, float> baseStatsBoostValues = new Dictionary<int, float>() {
        {-6, 2f/8f},
        {-5, 2f/7f},
        {-4, 2f/6f},
        {-3, 2f/5f},
        {-2, 2f/4f},
        {-1, 2f/3f},
        {0, 1f},
        {1, 3f/2f},
        {2, 4f/2f},
        {3, 5f/2f},
        {4, 6f/2f},
        {5, 7f/2f},
        {6, 8f/2f},
    };

    private readonly static Dictionary<int, float> combatStatsBoostValues = new Dictionary<int, float>() {
        {-6, 3f/9f},
        {-5, 3f/8f},
        {-4, 3f/7f},
        {-3, 3f/6f},
        {-2, 3f/5f},
        {-1, 3f/4f},
        {0, 1f},
        {1, 4f/3f},
        {2, 5f/3f},
        {3, 6f/3f},
        {4, 7f/3f},
        {5, 8f/3f},
        {6, 9f/3f},
    };

    public ScriptablePokemon ScriptablePokemon { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int CurrentHP { get; private set; }
    public PokemonOwner Owner { get; private set; }
    public Gender Gender { get; private set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> IVs { get; private set; } // Not implemented yet
    public Dictionary<Stat, int> EVs { get; private set; } // Not implemented yet
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public List<Move> Moves { get; private set; }
    public NonVolatileStatus NonVolatileStatus { get; private set; }
    public List<VolatileStatus> VolatileStatuses { get; private set; }

    public int MaxHP { get { return GetStat(Stat.MaxHP); } }
    public int Attack { get { return GetStat(Stat.Attack); } }
    public int Defense { get { return GetStat(Stat.Defense); } }
    public int SpecialAttack { get { return GetStat(Stat.SpecialAttack); } }
    public int SpecialDefense { get { return GetStat(Stat.SpecialDefense); } }
    public int Speed { get { return GetStat(Stat.Speed); } }
    public int Accuracy { get { return GetStat(Stat.Accuracy); } }
    public int Evasion { get { return GetStat(Stat.Evasion); } }

    public bool IsFainted { get { return CurrentHP <= 0; } }

    public Pokemon(ScriptablePokemon scriptablePokemon, int level, PokemonOwner owner) : this(scriptablePokemon, level, owner, "") { }

    public Pokemon(ScriptablePokemon scriptablePokemon, int level, PokemonOwner owner, string name)
    {
        ScriptablePokemon = scriptablePokemon;
        Name = name;
        if (Name == "") Name = scriptablePokemon.Name;
        Level = level;
        Owner = owner;
        Gender = (UnityEngine.Random.Range(0, 100) < scriptablePokemon.MalePercentage) ? Gender.Male : Gender.Female;

        CalculateStats();
        ResetStatBoosts();
        SetInitialMoves();

        CurrentHP = MaxHP;
    }

    public void ResetStatBoosts()
    {
        StatBoosts = new Dictionary<Stat, int> {
            { Stat.Attack, 0 },
            { Stat.Defense, 0 },
            { Stat.SpecialAttack, 0 },
            { Stat.SpecialDefense, 0 },
            { Stat.Speed, 0 },
            { Stat.Accuracy, 0 },
            { Stat.Evasion, 0 }
        };
    }

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

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (StatBoost statBoost in statBoosts)
        {
            Stat stat = statBoost.stat;
            int boostValue = statBoost.boostValue;
            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boostValue, -6, 6);
        }
    }

    private void CalculateStats()
    {
        Stats = new Dictionary<Stat, int> {
            { Stat.MaxHP, Mathf.FloorToInt((ScriptablePokemon.BaseHP * Level) / 100.0f) + Level + 10 },
            { Stat.Attack, Mathf.FloorToInt((ScriptablePokemon.BaseAttack * Level) / 100.0f) + 5 },
            { Stat.Defense, Mathf.FloorToInt((ScriptablePokemon.BaseDefense * Level) / 100.0f) + 5 },
            { Stat.SpecialAttack, Mathf.FloorToInt((ScriptablePokemon.BaseSpecialAttack * Level) / 100.0f) + 5 },
            { Stat.SpecialDefense, Mathf.FloorToInt((ScriptablePokemon.BaseSpecialDefense * Level) / 100.0f) + 5 },
            { Stat.Speed, Mathf.FloorToInt((ScriptablePokemon.BaseSpeed * Level) / 100.0f) + 5 },
            { Stat.Accuracy, 100 },
            { Stat.Evasion, 100 }
        };
    }

    private void SetInitialMoves()
    {
        Moves = new List<Move>();
        foreach (LearnableMove move in ScriptablePokemon.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Move));
            }
        }
        if (Moves.Count > 4)
        {
            Moves.RemoveRange(0, Moves.Count - 4);
        }
    }

    private int GetStat(Stat stat)
    {
        int statValue = Stats[stat];

        // Apply stat boosting
        int boost = StatBoosts[stat];
        bool isCombatStat = (stat == Stat.Accuracy || stat == Stat.Evasion);
        Dictionary<int, float> boostValues = (isCombatStat) ? combatStatsBoostValues : baseStatsBoostValues;
        statValue = Mathf.FloorToInt(statValue * boostValues[boost]);
        
        return statValue;
    }
}

public enum PokemonOwner {
    Player,
    EnemyTrainer,
    AllyTrainer,
    Wild
}

public enum Gender
{
    None,
    Male,
    Female
}

public enum Stat
{
    MaxHP,
    Attack,
    Defense,
    SpecialAttack,
    SpecialDefense,
    Speed,
    Accuracy,
    Evasion
}