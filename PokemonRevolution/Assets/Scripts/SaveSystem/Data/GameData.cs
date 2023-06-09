using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public PlayerSaveData PlayerData;
    public SerializableDictionary<string, SceneSaveData> ScenesData;

    public GameData(object _)
    {
        // Adding a dummy argument is necessary because as GameData is Serializable,
        // the constructor is called on game startup, and the PokemonBuilder is not
        // ready yet as it needs to load some data first


        Vector3 defaultPosition = new Vector3(1.5f, 0.5f, 0);
        List<PokemonSaveData> defaultParty = new List<PokemonSaveData>();

        PokemonBuilder pokemon1 = new PokemonBuilder()
        {
            scriptablePokemonId = 1,
            level = 14,
            name = "Bulb",
            owner = PokemonOwner.Player,
        };
        PokemonBuilder pokemon2 = new PokemonBuilder()
        {
            scriptablePokemonId = 4,
            level = 14,
            name = "Charmy",
            owner = PokemonOwner.Player,
        };


        defaultParty.Add(pokemon1.BuildPokemon().GetSaveData());
        defaultParty.Add(pokemon2.BuildPokemon().GetSaveData());

        PlayerData = new PlayerSaveData(defaultPosition, defaultParty);
        ScenesData = new SerializableDictionary<string, SceneSaveData>();
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 PlayerPosition;
    public List<PokemonSaveData> PokemonsSaveData;

    public PlayerSaveData(Vector3 position, List<PokemonSaveData> pokemonsSaveData)
    {
        PlayerPosition = position;
        PokemonsSaveData = pokemonsSaveData;
    }
}

[System.Serializable]
public struct SceneSaveData
{
    public SerializableDictionary<string, TrainerSaveData> TrainersSaveData;
    public SerializableDictionary<string, NPCSaveData> NPCsSaveData;

    public SceneSaveData(object _)
    {
        TrainersSaveData = new SerializableDictionary<string, TrainerSaveData>();
        NPCsSaveData = new SerializableDictionary<string, NPCSaveData>();
    }
}

[System.Serializable]
public struct TrainerSaveData
{
    public bool CanBattle;

    public TrainerSaveData(bool canBattle)
    {
        CanBattle = canBattle;
    }
}

[System.Serializable]
public struct NPCSaveData
{
    public bool FovIsEnabled;
    public int DialogueIndex;

    public NPCSaveData(bool fovIsActive, int dialogueIndex)
    {
        FovIsEnabled = fovIsActive;
        DialogueIndex = dialogueIndex;
    }
}

[System.Serializable]
public struct PokemonSaveData
{
    public int ScriptablePokemonId;
    public string Name;
    public int ExperiencePoints;
    public int HealthPoints;
    public PokemonOwner Owner;
    public Gender Gender;
    public SerializableDictionary<Stat, int> IVs;
    public SerializableDictionary<Stat, int> EVs;
    public List<MoveSaveData> Moves;
    public StatusCondition StatusCondition;
    public int RemainingStatusTime;
    
    public PokemonSaveData(
        int scriptablePokemonId, string name, int exp, int hp, PokemonOwner owner, Gender gender, 
        SerializableDictionary<Stat, int> ivs,
        SerializableDictionary<Stat, int> evs,
        List<MoveSaveData> moves,
        StatusCondition statusCondition,
        int remainingStatusTime)
    {
        ScriptablePokemonId = scriptablePokemonId;
        Name = name;
        ExperiencePoints = exp;
        HealthPoints = hp;
        Owner = owner;
        Gender = gender;
        IVs = ivs;
        EVs = evs;
        Moves = moves;
        StatusCondition = statusCondition;
        RemainingStatusTime = remainingStatusTime;
    }
}

[System.Serializable]
public struct MoveSaveData
{
    public string Name;
    public int PP;

    public MoveSaveData(string name, int pp)
    {
        Name = name;
        PP = pp;
    }
}