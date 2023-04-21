using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private List<StatBoost> statBoosts;
    [SerializeField] private List<MoveStatusConditionEffect> conditionEffects;

    public List<StatBoost> StatBoosts { get => statBoosts; }
    public List<MoveStatusConditionEffect> ConditionEffects { get => conditionEffects; }
    
    public void ApplyEffects(Pokemon attackingPokemon, Pokemon defendingPokemon)
    {
        foreach (StatBoost statBoost in StatBoosts)
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
        
        foreach (MoveStatusConditionEffect statusEffect in ConditionEffects)
        {
            if (Random.Range(0, 100) >= statusEffect.probability)
                continue;

            List<Pokemon> targetPokemons = new List<Pokemon>();
            if (statusEffect.statusEffectTargets.Contains(MoveTarget.Self))
                targetPokemons.Add(attackingPokemon);
            if (statusEffect.statusEffectTargets.Contains(MoveTarget.Enemy))
                targetPokemons.Add(defendingPokemon);

            foreach (Pokemon target in targetPokemons)
                target.ApplyStatus(statusEffect.statusCondition);
        }
    }
}


[System.Serializable]
public struct MoveStatusConditionEffect
{
    public int probability;
    public StatusCondition statusCondition;
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