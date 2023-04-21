using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static Dictionary<StatusCondition, StatusConditionData> Conditions { get; set; } = new Dictionary<StatusCondition, StatusConditionData>()
    {
        {
            StatusCondition.None,
            new StatusConditionData()
        },
        {
            StatusCondition.Poison, 
            new StatusConditionData()
            {
                Name = "Poison",
                Description = "",
                StartMessage = "has been poisoned !",
                EndMessage = "has been cured of his poisoning !",
                IsVolatile = false,
                OnBattleTurnEnd = (Pokemon pokemon) =>
                {
                    int damage = Mathf.FloorToInt((float)pokemon.MaxHP / 8);
                    pokemon.TakeDamage(damage);
                    // UIManager.Instance.WriteDialogueText($"{pokemon.Name} was hurt due to his poisoning");
                    Debug.Log($"{pokemon.Name} was hurt due to poisoning");
                }
            }
        },
        {
            StatusCondition.Burn,
            new StatusConditionData()
            {
                Name = "Burn",
                Description = "",
                StartMessage = "has been burned !",
                EndMessage = "is no longer burned !",
                IsVolatile = false,
                OnBattleTurnEnd = (Pokemon pokemon) =>
                {
                    int damage = Mathf.FloorToInt((float)pokemon.MaxHP / 16);
                    pokemon.TakeDamage(damage);
                    // UIManager.Instance.WriteDialogueText($"{pokemon.Name} was hurt due to his poisoning");
                    Debug.Log($"{pokemon.Name} was hurt due to his burn");
                }
            }
        }
    };
}
