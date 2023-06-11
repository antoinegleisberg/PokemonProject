using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private List<StatBoost> _statBoosts;
    [SerializeField] private List<MoveStatusConditionEffect> _conditionEffects;

    public List<StatBoost> StatBoosts { get => _statBoosts; }
    public List<MoveStatusConditionEffect> ConditionEffects { get => _conditionEffects; }
    
    public void ApplyEffects(Pokemon attackingPokemon, Pokemon defendingPokemon)
    {
        foreach (StatBoost statBoost in StatBoosts)
        {
            if (Random.Range(0, 100) >= statBoost.Probability)
                continue;

            List<Pokemon> targetPokemons = new List<Pokemon>();
            if (statBoost.StatBoostTargets.Contains(MoveTarget.Self))
                targetPokemons.Add(attackingPokemon);
            if (statBoost.StatBoostTargets.Contains(MoveTarget.Enemy))
                targetPokemons.Add(defendingPokemon);

            foreach (Pokemon target in targetPokemons)
                target.ApplyBoost(statBoost);
        }
        
        foreach (MoveStatusConditionEffect statusEffect in ConditionEffects)
        {
            if (Random.Range(0, 100) >= statusEffect.Probability)
                continue;

            List<Pokemon> targetPokemons = new List<Pokemon>();
            if (statusEffect.StatusEffectTargets.Contains(MoveTarget.Self))
                targetPokemons.Add(attackingPokemon);
            if (statusEffect.StatusEffectTargets.Contains(MoveTarget.Enemy))
                targetPokemons.Add(defendingPokemon);

            foreach (Pokemon target in targetPokemons)
                target.ApplyStatus(statusEffect.StatusCondition);
        }
    }
}


[System.Serializable]
public struct MoveStatusConditionEffect
{
    public int Probability;
    public StatusCondition StatusCondition;
    public List<MoveTarget> StatusEffectTargets;
}

[System.Serializable]
public struct StatBoost
{
    public int Probability;
    public Stat Stat;
    public int BoostValue;
    public List<MoveTarget> StatBoostTargets;
}