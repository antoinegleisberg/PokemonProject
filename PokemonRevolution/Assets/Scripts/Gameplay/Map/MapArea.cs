using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] private List<WildEncounter> wildEncounters;

    public Pokemon GetRandomWildPokemon()
    {
        int totalProbability = 0;
        foreach (WildEncounter wildEncounter in wildEncounters)
        {
            totalProbability += wildEncounter.probability;
        }

        int randomValue = Random.Range(0, totalProbability);
        int currentProbability = 0;
        foreach (WildEncounter wildEncounter in wildEncounters)
        {
            currentProbability += wildEncounter.probability;
            if (randomValue < currentProbability)
            {
                int level = Random.Range(wildEncounter.minLevel, wildEncounter.maxLevel + 1);
                return new Pokemon(wildEncounter.pokemon, level, PokemonOwner.Wild);
            }
        }

        return null;
    }
}

[System.Serializable]
public class WildEncounter
{
    public ScriptablePokemon pokemon;
    public int minLevel;
    public int maxLevel;
    public int probability;
}