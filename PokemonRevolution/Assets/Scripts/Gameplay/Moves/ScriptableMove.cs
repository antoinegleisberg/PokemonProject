using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new Move")]
public class ScriptableMove : ScriptableObject
{
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;

    [SerializeField] MoveCategory category;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    // [SerializeField] bool isSpecial;
    // [SerializeField] effect;
    // [SerializeField] effectProbability;

    // target : ally / allies / ennemy / ennemies

    // makesContact

    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public MoveCategory Category { get => category; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }
}