using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemons", menuName = "Pokemon/Create new Pokemon")]
public class ScriptablePokemon : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private string species;

    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private float height;
    [SerializeField] private float weight;

    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite iconSprite;

    [SerializeField] private PokemonType type1;
    [SerializeField] private PokemonType type2;

    [SerializeField] private int baseHP;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseDefense;
    [SerializeField] private int baseSpecialAttack;
    [SerializeField] private int baseSpecialDefense;
    [SerializeField] private int baseSpeed;

    [SerializeField] GrowthRate growthRate;
    [SerializeField] private int baseFriendship;
    [SerializeField] private int catchRate;

    [SerializeField] private int experienceYield;
    [SerializeField] private List<EVYield> evYield;

    [SerializeField] private List<LearnableMove> learnableMoves;
    [SerializeField] private List<ScriptableMove> eggMoves;
    [SerializeField] private List<ScriptableMove> tmMoves;

    [SerializeField] private ScriptableAbility ability1;
    [SerializeField] private ScriptableAbility ability2;
    [SerializeField] private ScriptableAbility hiddenAbility;

    // [SerializeField] List<EggGroup> eggGroups;
    [SerializeField] private int eggCycles;
    [SerializeField] private float malePercentage;

    public int Id { get => id; }
    public string Name { get => name; }
    public string Species { get => species; }
    public string Description { get => description; }
    public float Height { get => height; }
    public float Weight { get => weight; }
    public Sprite FrontSprite { get => frontSprite; }
    public Sprite BackSprite { get => backSprite; }
    public Sprite IconSprite { get => iconSprite; }
    public PokemonType Type1 { get => type1; }
    public PokemonType Type2 { get => type2; }
    public int BaseHP { get => baseHP; }
    public int BaseAttack { get => baseAttack; }
    public int BaseDefense { get => baseDefense; }
    public int BaseSpecialAttack { get => baseSpecialAttack; }
    public int BaseSpecialDefense { get => baseSpecialDefense; }
    public int BaseSpeed { get => baseSpeed; }
    public GrowthRate GrowthRate { get => growthRate; }
    public int BaseFriendship { get => baseFriendship; }
    public int CatchRate { get => catchRate; }
    public int ExperienceYield { get => experienceYield; }
    public List<EVYield> EvYield { get => evYield; }
    public List<LearnableMove> LearnableMoves { get => learnableMoves; }
    public List<ScriptableMove> EggMoves { get => eggMoves; }
    public List<ScriptableMove> TmMoves { get => tmMoves; }
    public ScriptableAbility Ability1 { get => ability1; }
    public ScriptableAbility Ability2 { get => ability2; }
    public ScriptableAbility HiddenAbility { get => hiddenAbility; }
    public int EggCycles { get => eggCycles; }
    public float MalePercentage { get => malePercentage; }
}