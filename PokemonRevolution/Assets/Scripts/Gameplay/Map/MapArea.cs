using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{    
    [SerializeField] private List<WildEncounter> _wildEncounters;
    [SerializeField] private int _encounterRate = 10;

    public int EncounterRate { get => _encounterRate; }

    public Pokemon GetRandomWildPokemon()
    {
        int totalProbability = 0;
        foreach (WildEncounter wildEncounter in _wildEncounters)
        {
            totalProbability += wildEncounter.Probability;
        }

        int randomValue = Random.Range(0, totalProbability);
        int currentProbability = 0;
        foreach (WildEncounter wildEncounter in _wildEncounters)
        {
            currentProbability += wildEncounter.Probability;
            if (randomValue < currentProbability)
            {
                int level = Random.Range(wildEncounter.MinLevel, wildEncounter.MaxLevel + 1);
                return new Pokemon(wildEncounter.Pokemon, level, PokemonOwner.Wild);
            }
        }

        return null;
    }
}