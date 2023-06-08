using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

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
    
    public static int MaxLevel { get; private set; } = 100;
    public static int MaxNumberMoves { get; private set; } = 4;
    public static int MaxEVs { get; private set; } = 510;
    public static int MaxEVsPerStat { get; private set; } = 255;
    public static int MaxIVs { get; private set; } = 31;

    public ScriptablePokemon ScriptablePokemon { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int TotalExperiencePoints { get; private set; }
    public int CurrentHP { get; private set; }
    public PokemonOwner Owner { get; set; }
    public Gender Gender { get; private set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> IVs { get; private set; }
    public Dictionary<Stat, int> EVs { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public List<Move> Moves { get; private set; }
    public StatusCondition StatusCondition { get; private set; }
    public List<StatusCondition> VolatileStatusConditions { get; private set; }
    public Dictionary<StatusCondition, int> RemainingStatusTime { get; private set; }
    public Dictionary<StatusCondition, int> StatusTimeCount { get; private set; }

    private ScriptableMove moveToLearn;

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
        TotalExperiencePoints = GrowthRateDB.Level2TotalExp(scriptablePokemon.GrowthRate, level);
        Owner = owner;
        Gender = (Random.Range(0, 100) < scriptablePokemon.MalePercentage) ? Gender.Male : Gender.Female;

        StatusCondition = StatusCondition.None;
        VolatileStatusConditions = new List<StatusCondition>();
        RemainingStatusTime = new Dictionary<StatusCondition, int>();
        StatusTimeCount = new Dictionary<StatusCondition, int>();

        InitIVs();
        InitEVs();
        ResetStatBoosts();
        CalculateStats();
        SetInitialMoves();

        CurrentHP = MaxHP;
    }
    
    public Pokemon(PokemonSaveData saveData)
    {
        ScriptablePokemon = PokemonsDB.GetPokemonById(saveData.ScriptablePokemonId);
        Name = saveData.Name;
        TotalExperiencePoints = saveData.ExperiencePoints;
        Level = GrowthRateDB.TotalExp2Level(ScriptablePokemon.GrowthRate, TotalExperiencePoints);
        Owner = saveData.Owner;
        Gender = saveData.Gender;
        StatusCondition = saveData.StatusCondition;
        VolatileStatusConditions = new List<StatusCondition>();
        RemainingStatusTime = new Dictionary<StatusCondition, int>();
        StatusTimeCount = new Dictionary<StatusCondition, int>();
        IVs = saveData.IVs;
        EVs = saveData.EVs;
        ResetStatBoosts();
        CalculateStats();
        
        if (StatusCondition != StatusCondition.None)
        {
            RemainingStatusTime[StatusCondition] = saveData.RemainingStatusTime;
            StatusTimeCount[StatusCondition] = 0;
        }

        Moves = new List<Move>();
        foreach (MoveSaveData moveData in saveData.Moves)
        {
            Moves.Add(new Move(moveData));
        }

        CurrentHP = saveData.HealthPoints;
    }

    public PokemonSaveData GetSaveData()
    {
        List<MoveSaveData> movesData = new List<MoveSaveData>();
        
        foreach (Move move in Moves)
        {
            movesData.Add(move.GetSaveData());
        }

        int remainingStatusTime = 0;
        if (RemainingStatusTime.ContainsKey(StatusCondition))
        {
            remainingStatusTime = RemainingStatusTime[StatusCondition];
        }

        return new PokemonSaveData(
            ScriptablePokemon.Id,
            Name,
            TotalExperiencePoints,
            CurrentHP,
            Owner,
            Gender,
            SerializableDictionary<Stat, int>.FromDictionary(IVs),
            SerializableDictionary<Stat, int>.FromDictionary(EVs),
            movesData,
            StatusCondition,
            remainingStatusTime
            );
    }


    public ConditionAttackModifier OnBeforeMove(Move move)
    {
        ConditionAttackModifier modifier = new ConditionAttackModifier(true, 1);

        if (StatusCondition != StatusCondition.None)
            StatusTimeCount[StatusCondition] += 1;
        if (ConditionsDB.Conditions[StatusCondition].OnBeforeMove != null)
            modifier = modifier.Merge(ConditionsDB.Conditions[StatusCondition].OnBeforeMove(this, move));

        // This executes as if it were after the move
        if (RemainingStatusTime.ContainsKey(StatusCondition))
            RemainingStatusTime[StatusCondition]--;


        foreach (StatusCondition status in VolatileStatusConditions.ToList())
        {
            StatusTimeCount[status] += 1;
            if (ConditionsDB.Conditions[status].OnBeforeMove != null)
                modifier = modifier.Merge(ConditionsDB.Conditions[status].OnBeforeMove(this, move));
            
            // This executes as if it were after the move
            if (RemainingStatusTime.ContainsKey(status))
                RemainingStatusTime[status]--;
        }

        return modifier;
    }
    
    public void OnPokemonSwitchedOut()
    {
        ResetStatBoosts();
        ClearVolatileStatusConditions();
        if (StatusCondition != StatusCondition.None)
            StatusTimeCount[StatusCondition] = 0;
    }

    public void OnBattleTurnEnd()
    {
        if (IsFainted)
            return;
        ConditionsDB.Conditions[StatusCondition].OnBattleTurnEnd?.Invoke(this);

        foreach (StatusCondition status in VolatileStatusConditions)
        {
            ConditionsDB.Conditions[status].OnBattleTurnEnd?.Invoke(this);
        }
    }

    public void OnExitBattle()
    {
        OnPokemonSwitchedOut();
    }

    public void TakeDamage(float damage)
    {
        int roundedDamage = Mathf.Max(1, Mathf.RoundToInt(damage));

        CurrentHP -= roundedDamage;
        if (CurrentHP < 0) CurrentHP = 0;
        if (roundedDamage >= 1) BattleEvents.Instance.PokemonDamaged(this, roundedDamage);
        if (IsFainted)
        {
            BattleEvents.Instance.PokemonFaints(this);
            BattleEvents.Instance.AfterPokemonFainted(this);
        }
    }
    
    public IEnumerator GainExp(int exp)
    {
        while (exp > 0 && Level < MaxLevel)
        {
            int expBeforeLvUp = GrowthRateDB.ExpBeforeLevelUp(this);
            if (exp >= expBeforeLvUp)
            {
                TotalExperiencePoints += expBeforeLvUp;
                exp -= expBeforeLvUp;

                // For pokemons at max level
                int maxExp = GrowthRateDB.Level2TotalExp(ScriptablePokemon.GrowthRate, MaxLevel);
                TotalExperiencePoints = Mathf.Clamp(TotalExperiencePoints, 0, maxExp);

                BattleEvents.Instance.ExpGained(this, expBeforeLvUp);

                yield return BattleUIManager.Instance.WaitWhileBusy();

                Assert.IsTrue(GrowthRateDB.ShouldLevelUp(this));
                yield return LevelUp();
            }
            else
            {
                TotalExperiencePoints += exp;
                BattleEvents.Instance.ExpGained(this, exp);
                exp = 0;
            }
        }
    }

    public void GainEVs(ScriptablePokemon defeatedPokemon)
    {
        int availableEVs = MaxEVs - EVs.Values.Sum();
        foreach (EVYield evYield in defeatedPokemon.EvYield)
        {
            int availableEVforStat = MaxEVsPerStat - EVs[evYield.stat];
            int EVsGained = Mathf.Min(availableEVs, evYield.value, availableEVforStat);
            EVs[evYield.stat] += EVsGained;
            availableEVs -= EVsGained;
        }
    }
    
    public IEnumerator LevelUp()
    {
        if (Level >= MaxLevel)
            yield break;

        // check for missing Exp, for example in the case of rare candies etc.
        if (TotalExperiencePoints < GrowthRateDB.Level2TotalExp(ScriptablePokemon.GrowthRate, Level + 1))
        {
            TotalExperiencePoints += GrowthRateDB.ExpBeforeLevelUp(this);
        }   

        Level += 1;
        CalculateStats();
        BattleEvents.Instance.LevelUp(this);

        // yield return BattleUIManager.Instance.WaitWhileBusy();

        yield return CheckForNewMoves();

        // TODO
        // Check for evolution
    }

    public void ReplaceMove(int index)
    {
        if (index < 0 || index >= Moves.Count)
            return;
        Move oldMove = Moves[index];
        Moves[index] = new Move(moveToLearn);
        BattleEvents.Instance.MoveLearnt(this, oldMove.ScriptableMove, moveToLearn);
    }
    public void LoseMovePP(Move move)
    {
        foreach (Move m in Moves)
        {
            if (m.ScriptableMove.Name == move.ScriptableMove.Name)
                m.CurrentPP--;
        }
    }

    public void HealFull()
    {
        CurrentHP = MaxHP;
        CureStatus();
        ClearVolatileStatusConditions();
        ResetStatBoosts();
        foreach (Move move in Moves)
        {
            move.CurrentPP = move.ScriptableMove.PP;
        }
    }

    public void ApplyBoost(StatBoost statBoost)
    {
        Stat stat = statBoost.stat;
        int boostValue = statBoost.boostValue;
        int oldBoostValue = StatBoosts[stat];
        StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boostValue, -6, 6);
        int newBoostValue = StatBoosts[stat];
        
        BattleEvents.Instance.BoostedPokemonStat(stat, newBoostValue - oldBoostValue, this);
    }

    public void ApplyStatus(StatusCondition status)
    {
        if (status == StatusCondition.None)
            return;

        if (StatusCondition != StatusCondition.None && !ConditionsDB.Conditions[status].IsVolatile)
            return;

        if (VolatileStatusConditions.Contains(status))
            return;

        if (ConditionsDB.Conditions[status].IsVolatile)
        {
            VolatileStatusConditions.Add(status);
        }
        else if (StatusCondition == StatusCondition.None)
        {
            StatusCondition = status;
        }
        StatusTimeCount[status] = 0;
        ConditionsDB.Conditions[status].OnStart?.Invoke(this);
        BattleEvents.Instance.AppliedStatusCondition(status, this);
    }

    public void CureStatus()
    {
        StatusCondition oldStatus = StatusCondition;
        StatusCondition = StatusCondition.None;

        if (RemainingStatusTime.ContainsKey(oldStatus))
            RemainingStatusTime.Remove(oldStatus);
        
        if (oldStatus != StatusCondition.None)
            BattleEvents.Instance.RemovedStatusCondition(oldStatus, this);
    }

    public void CureVolatileStatus(StatusCondition status)
    {
        if (RemainingStatusTime.ContainsKey(status))
            RemainingStatusTime.Remove(status);

        if (VolatileStatusConditions.Contains(status))
        {
            VolatileStatusConditions.Remove(status);
            BattleEvents.Instance.RemovedStatusCondition(status, this);
        }
    }

    private void CalculateStats()
    {
        int HP = Mathf.FloorToInt((2 * ScriptablePokemon.BaseHP + IVs[Stat.MaxHP] + Mathf.FloorToInt(EVs[Stat.MaxHP] / 4.0f)) * Level / 100) + Level + 10;
        int Attack = Mathf.FloorToInt((2 * ScriptablePokemon.BaseAttack + IVs[Stat.Attack] + Mathf.FloorToInt(EVs[Stat.Attack] / 4.0f)) * Level / 100) + 5;
        int Defense = Mathf.FloorToInt((2 * ScriptablePokemon.BaseDefense + IVs[Stat.Defense] + Mathf.FloorToInt(EVs[Stat.Defense] / 4.0f)) * Level / 100) + 5;
        int SpecialAttack = Mathf.FloorToInt((2 * ScriptablePokemon.BaseSpecialAttack + IVs[Stat.SpecialAttack] + Mathf.FloorToInt(EVs[Stat.SpecialAttack] / 4.0f)) * Level / 100) + 5;
        int SpecialDefense = Mathf.FloorToInt((2 * ScriptablePokemon.BaseSpecialDefense + IVs[Stat.SpecialDefense] + Mathf.FloorToInt(EVs[Stat.SpecialDefense] / 4.0f)) * Level / 100) + 5;
        int Speed = Mathf.FloorToInt((2 * ScriptablePokemon.BaseSpeed + IVs[Stat.Speed] + Mathf.FloorToInt(EVs[Stat.Speed] / 4.0f)) * Level / 100) + 5;
        int Accuracy = 100;
        int Evasion = 100;

        Stats = new Dictionary<Stat, int> {
            { Stat.MaxHP, HP },
            { Stat.Attack, Attack },
            { Stat.Defense, Defense },
            { Stat.SpecialAttack, SpecialAttack },
            { Stat.SpecialDefense, SpecialDefense },
            { Stat.Speed, Speed },
            { Stat.Accuracy, Accuracy },
            { Stat.Evasion, Evasion }
        };
    }
    
    private void ResetStatBoosts()
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

    private void ClearVolatileStatusConditions()
    {
        bool hasStatusConditionTime = RemainingStatusTime.ContainsKey(StatusCondition);
        int statusConditionRemainingTime = 0;
        if (hasStatusConditionTime)
            statusConditionRemainingTime = RemainingStatusTime[StatusCondition];
        int statusConditionTimeCount = 0;
        if (StatusCondition != StatusCondition.None)
            statusConditionTimeCount = StatusTimeCount[StatusCondition];

        VolatileStatusConditions.Clear();
        RemainingStatusTime.Clear();
        StatusTimeCount.Clear();
        
        if (hasStatusConditionTime)
            RemainingStatusTime[StatusCondition] = statusConditionRemainingTime;
        if (StatusCondition != StatusCondition.None)
            StatusTimeCount[StatusCondition] = statusConditionTimeCount;
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
        if (Moves.Count > MaxNumberMoves)
        {
            Moves.RemoveRange(0, Moves.Count - MaxNumberMoves);
        }
    }

    private void InitIVs()
    {
        IVs = new Dictionary<Stat, int>() {
            {Stat.MaxHP, Random.Range(0, 32) },
            {Stat.Attack, Random.Range(0, 32) },
            {Stat.Defense, Random.Range(0, 32) },
            {Stat.SpecialAttack, Random.Range(0, 32) },
            {Stat.SpecialDefense, Random.Range(0, 32) },
            {Stat.Speed, Random.Range(0, 32) },
        };
    }

    private void InitEVs()
    {
        EVs = new Dictionary<Stat, int>() {
            {Stat.MaxHP, 0 },
            {Stat.Attack, 0 },
            {Stat.Defense, 0 },
            {Stat.SpecialAttack, 0 },
            {Stat.SpecialDefense, 0 },
            {Stat.Speed, 0 },
        };
    }

    private int GetStat(Stat stat)
    {
        float statValue = Stats[stat];

        // Apply stat boosting
        if (stat != Stat.MaxHP)
        {
            int boost = StatBoosts[stat];
            bool isCombatStat = (stat == Stat.Accuracy || stat == Stat.Evasion);
            Dictionary<int, float> boostValues = (isCombatStat) ? combatStatsBoostValues : baseStatsBoostValues;
            statValue = statValue * boostValues[boost];
        }
        if (ConditionsDB.Conditions[StatusCondition].OnGetStat != null)
            statValue *= ConditionsDB.Conditions[StatusCondition].OnGetStat(this, stat);
        foreach (StatusCondition status in VolatileStatusConditions)
            if (ConditionsDB.Conditions[status].OnGetStat != null)
                statValue *= ConditionsDB.Conditions[status].OnGetStat(this, stat);

        return Mathf.FloorToInt(statValue);
    }

    private IEnumerator CheckForNewMoves()
    {
        foreach (LearnableMove learnableMove in ScriptablePokemon.LearnableMoves)
        {
            if (learnableMove.Level == Level)
            {
                moveToLearn = learnableMove.Move;
                if (Moves.Count < MaxNumberMoves)
                    LearnNewMove(learnableMove.Move);
                else
                    BattleEvents.Instance.ChooseMoveToForget(this, learnableMove.Move);
            }
            yield return new WaitUntil(() => !BattleUIManager.Instance.IsPaused);
        }
        yield break;
    }

    private void LearnNewMove(ScriptableMove newMove)
    {
        if (Moves.Count < MaxNumberMoves)
        {
            Moves.Add(new Move(newMove));
            BattleEvents.Instance.MoveLearnt(this, null, newMove);
        }
    }
}

public enum PokemonOwner {
    Player,
    EnemyTrainer,
    AllyTrainer,
    Wild
}

[System.Serializable]
public enum Gender
{
    None,
    Male,
    Female
}

[System.Serializable]
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