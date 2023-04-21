using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> statBoosts;
    [SerializeField] List<AppliedVolatileStatusEffect> appliedvolatileStatusEffects;
    [SerializeField] List<AppliedNonVolatileStatusEffect> appliedNonVolatileStatusEffects;

    public List<StatBoost> StatBoosts { get => statBoosts; }
    public List<AppliedVolatileStatusEffect> AppliedVolatileStatusEffects { get => appliedvolatileStatusEffects; }
    public List<AppliedNonVolatileStatusEffect> AppliedNonVolatileStatusEffects
    {
        get => appliedNonVolatileStatusEffects;
    }

    public void ApplyEffects(Pokemon attackingPokemon, Pokemon defendingPokemon)
    {
        foreach (StatBoost statBoost in statBoosts)
        {
            if (Random.Range(0, 100) >= statBoost.probability)
                continue;

            List<Pokemon> targetPokemons = new List<Pokemon>();
            if (statBoost.statBoostTargets.Contains(MoveTarget.Self))
                targetPokemons.Add(attackingPokemon);
            if (statBoost.statBoostTargets.Contains(MoveTarget.Enemy))
                targetPokemons.Add(defendingPokemon);

            foreach (Pokemon target in targetPokemons)
                target.ApplyBoost(statBoost);
        }
        
        foreach (AppliedNonVolatileStatusEffect effect in AppliedNonVolatileStatusEffects)
        {
            if (Random.Range(0, 100) >= effect.probability)
                continue;

            List<Pokemon> targetPokemons = new List<Pokemon>();
            if (effect.statusEffectTargets.Contains(MoveTarget.Self))
                targetPokemons.Add(attackingPokemon);
            if (effect.statusEffectTargets.Contains(MoveTarget.Enemy))
                targetPokemons.Add(defendingPokemon);

            foreach (Pokemon target in targetPokemons)
                target.ApplyNonVolatileStatus(effect.nonVolatileStatus);
        }
        
        foreach (AppliedVolatileStatusEffect effect in AppliedVolatileStatusEffects)
        {
            if (Random.Range(0, 100) >= effect.probability)
                continue;

            List<Pokemon> targetPokemons = new List<Pokemon>();
            if (effect.statusEffectTargets.Contains(MoveTarget.Self))
                targetPokemons.Add(attackingPokemon);
            if (effect.statusEffectTargets.Contains(MoveTarget.Enemy))
                targetPokemons.Add(defendingPokemon);

            foreach (Pokemon target in targetPokemons)
                target.ApplyVolatileStatus(effect.volatileStatus);
        }
    }
}


[System.Serializable]
public struct AppliedNonVolatileStatusEffect
{
    public int probability;
    public NonVolatileStatus nonVolatileStatus;
    public List<MoveTarget> statusEffectTargets;
}

[System.Serializable]
public struct AppliedVolatileStatusEffect
{
    public int probability;
    public VolatileStatus volatileStatus;
    public List<MoveTarget> statusEffectTargets;
}

[System.Serializable]
public struct StatBoost
{
    public int probability;
    public Stat stat;
    public int boostValue;
    public List<MoveTarget> statBoostTargets;
}