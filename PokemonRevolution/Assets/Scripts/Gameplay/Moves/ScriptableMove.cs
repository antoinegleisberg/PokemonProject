using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new Move")]
public class ScriptableMove : ScriptableObject
{
    [SerializeField] private string moveName;

    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private PokemonType type;

    [SerializeField] private MoveCategory category;

    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int pp;
    [SerializeField] private int priority;
    [SerializeField] private bool alwaysHits;

    [SerializeField] private bool makesContact;

    [SerializeField] private List<MoveTarget> moveTargets;
    
    [SerializeField] private MoveEffects moveEffects;

    public string Name { get => moveName; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public MoveCategory Category { get => category; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }
    public int Priority { get => priority; }
    public bool AlwaysHits { get => alwaysHits; }
    public bool MakesContact { get => makesContact; }
    public List<MoveTarget> MoveTargets { get => moveTargets; }
    public MoveEffects MoveEffects { get => moveEffects; }
}