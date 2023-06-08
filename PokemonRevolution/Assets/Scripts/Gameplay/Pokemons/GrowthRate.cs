using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrowthRate
{
    Erratic,
    Fast,
    MediumFast,
    MediumSlow,
    Slow,
    Fluctuating
}

public static class GrowthRateDB
{
    public static int Level2TotalExp(GrowthRate growthRate, int level)
    {
        if (level == 1)
            return 0;

        switch (growthRate)
        {
            case GrowthRate.Erratic:
                if (level < 50)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (100 - level) / 50);
                if (level < 68)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (150 - level) / 100);
                if (level < 98)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * Mathf.FloorToInt((1911 - 10 * level) / 3) / 500);
                if (level <= 100)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (160 - level) / 100);
                if (level <= 200)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * 3 / 5);
                else
                    return 600000;

            case GrowthRate.Fast:
                return Mathf.FloorToInt(4 * Mathf.Pow(level, 3) / 5);

            case GrowthRate.MediumFast:
                return Mathf.FloorToInt(Mathf.Pow(level, 3));

            case GrowthRate.MediumSlow:
                return Mathf.FloorToInt(6 * Mathf.Pow(level, 3) / 5 - 15 * level * level + 100 * level - 140);

            case GrowthRate.Slow:
                return Mathf.FloorToInt(Mathf.Pow(level, 3) * 5 / 4);

            case GrowthRate.Fluctuating:
                if (level < 15)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.FloorToInt((level + 1) / 3) + 24) / 50);
                if (level < 36)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (level + 14) / 50);
                if (level <= 100)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.FloorToInt(level / 2) + 32) / 50);
                if (level <= 200)
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * 8.0f / 5);
                else
                    return 1640000;

            default:
                Debug.LogError("Unknown growth rate !");
                return 1000000;
        }
    }

    public static int TotalExp2Level(GrowthRate growthRate, int exp)
    {
        int level = 1;
        while (Level2TotalExp(growthRate, level) <= exp)
        {
            level++;
        }
        level--;
        return level;
    }

    public static int Exp2NextLevel(GrowthRate growthRate, int currentLevel)
    {
        if (currentLevel == 100)
            return 0;
        return Level2TotalExp(growthRate, currentLevel + 1) - Level2TotalExp(growthRate, currentLevel);
    }

    public static int ExpBeforeLevelUp(Pokemon pokemon)
    {
        int nextLevelExp = Level2TotalExp(pokemon.ScriptablePokemon.GrowthRate, pokemon.Level + 1);
        int expToNextLevel = nextLevelExp - pokemon.TotalExperiencePoints;
        
        return Mathf.Max(expToNextLevel, 0);
    }

    public static bool ShouldLevelUp(Pokemon pokemon)
    {
        if (pokemon.Level >= Pokemon.MaxLevel)
            return false;

        return ExpBeforeLevelUp(pokemon) <= 0;
    }
}