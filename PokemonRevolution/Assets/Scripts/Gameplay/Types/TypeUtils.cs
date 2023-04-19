using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TypeUtils
{
    private static Dictionary<PokemonType, ScriptableType> types;

    public static ScriptableType TypeInfo(PokemonType type)
    {
        if (types == null)
        {
            types = new Dictionary<PokemonType, ScriptableType>();
            Object[] typesList = Resources.LoadAll("Types");
            foreach (Object t in typesList)
            {
                ScriptableType loadedType = (ScriptableType)t;
                types[loadedType.Type] = loadedType;
            }
        }
        return types[type];
    }

    public static float TypeModifier(ScriptableMove attackingMove, ScriptablePokemon defendingPokemon)
    {
        float modifier = TypeModifier(attackingMove.Type, defendingPokemon.Type1);
        if (defendingPokemon.Type2 != PokemonType.None)
        {
            modifier *= TypeModifier(attackingMove.Type, defendingPokemon.Type2);
        }
        return modifier;
    }

    private static float TypeModifier(PokemonType attackingType, PokemonType defendingType)
    {
        float modifier = 1f;
        ScriptableType defendingTypeInfo = TypeInfo(defendingType);
        if (defendingTypeInfo.WeakTo.Contains(attackingType))
        {
            modifier *= 2f;
        }
        else if (defendingTypeInfo.ResistantTo.Contains(attackingType))
        {
            modifier *= 0.5f;
        }
        else if (defendingTypeInfo.ImmuneTo.Contains(attackingType))
        {
            modifier *= 0f;
        }
        return modifier;
    }
}
