using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "New Type", menuName = "Pokemon/Create new Type")]
public class ScriptableType : ScriptableObject
{
    [SerializeField] private PokemonType type;
    [SerializeField] private Sprite typeIcon;
    [SerializeField] private Color typeColor;
    [SerializeField] private List<PokemonType> resistantTo;
    [SerializeField] private List<PokemonType> weakTo;
    [SerializeField] private List<PokemonType> immuneTo;

    public PokemonType Type { get => type; }
    public Sprite TypeIcon { get => typeIcon; }
    public Color TypeColor { get => typeColor; }
    public List<PokemonType> ResistantTo { get => resistantTo; }
    public List<PokemonType> WeakTo { get => weakTo; }
    public List<PokemonType> ImmuneTo { get => immuneTo; }
}