using System.Collections.Generic;
using UnityEngine;

public static class ConditionsDB
{
    private static Dictionary<StatusCondition, StatusConditionData> _conditions;

    public static StatusConditionData GetCondition(StatusCondition status)
    {
        return _conditions[status];
    }

    public static void Init()
    {
        _conditions = new Dictionary<StatusCondition, StatusConditionData>()
        {
            // None
            {
                StatusCondition.None,
                new StatusConditionData()
                {
                    CatchRateModifier = 1,
                }
            },
            // Burn
            {
                StatusCondition.Burn,
                new StatusConditionData()
                {
                    Name = "Burn",
                    HUDName = "BRN",
                    HUDColor = new Color(0.9333333f, 0.5058824f, 0.1882353f, 1),
                    Description = "",
                    StartMessage = "has been burned !",
                    EndMessage = "is no longer burned !",
                    IsVolatile = false,
                    CatchRateModifier = 1.5f,
                    OnBeforeMove = (Pokemon pokemon, Move move) =>
                    {
                        if (move.ScriptableMove.Category == MoveCategory.Physical)
                            return new ConditionAttackModifier(true, 0.5f);
                        return new ConditionAttackModifier(true, 1);
                    },
                    OnBattleTurnEnd = (Pokemon pokemon) =>
                    {
                        float damage = (float)pokemon.MaxHP / 16;
                        BattleEvents.Instance.StatusConditionMessage($"{pokemon.Name} was hurt due to his burn");
                        GameManager.Instance.BattleManager.DamagePokemon(pokemon, damage);
                    }
                }
            },
            // Poison
            {
                StatusCondition.Poison,
                new StatusConditionData()
                {
                    Name = "Poison",
                    HUDName = "PSN",
                    HUDColor = new Color(0.6392157f, 0.2431373f, 0.6313726f, 1),
                    Description = "",
                    StartMessage = "has been poisoned !",
                    EndMessage = "has been cured of his poisoning !",
                    IsVolatile = false,
                    CatchRateModifier = 1.5f,
                    OnBattleTurnEnd = (Pokemon pokemon) =>
                    {
                        float damage = (float)pokemon.MaxHP / 8;
                        BattleEvents.Instance.StatusConditionMessage($"{pokemon.Name} was hurt due to poisoning");
                        GameManager.Instance.BattleManager.DamagePokemon(pokemon, damage);
                    }
                }
            },
            // Bad Poison
            {
                StatusCondition.BadPoison,
                new StatusConditionData()
                {
                    Name = "Bad Poison",
                    HUDName = "PSN",
                    HUDColor = new Color(0.6392157f, 0.2431373f, 0.6313726f, 1),
                    Description = "",
                    StartMessage = "has been badly poisoned !",
                    EndMessage = "has been cured of his poisoning !",
                    IsVolatile = false,
                    CatchRateModifier = 1.5f,
                    OnBattleTurnEnd = (Pokemon pokemon) =>
                    {
                        float damage = (float)pokemon.MaxHP / 16 * (pokemon.StatusTimeCount[StatusCondition.BadPoison]);
                        BattleEvents.Instance.StatusConditionMessage($"{pokemon.Name} was hurt due to bad poisoning");
                        GameManager.Instance.BattleManager.DamagePokemon(pokemon, damage);
                    }
                }
            },
            // Paralysis
            {
                StatusCondition.Paralysis,
                new StatusConditionData()
                {
                    Name = "Paralysis",
                    HUDName = "PAR",
                    HUDColor = new Color(0.9686275f, 0.8156863f, 0.172549f, 1),
                    Description = "",
                    StartMessage = "has been paralyzed !",
                    EndMessage = "is no longer paralyzed !",
                    IsVolatile = false,
                    CatchRateModifier = 1.5f,
                    OnGetStat = (Pokemon pokemon, Stat stat) =>
                    {
                        if (stat == Stat.Speed)
                            return 0.25f;
                        return 1.0f;
                    },
                    OnBeforeMove = (Pokemon pokemon, Move move) =>
                    {
                        bool canAttack = Random.Range(0,4) != 0;
                        if (!canAttack)
                            BattleEvents.Instance.StatusConditionMessage($"{pokemon.Name} is paralyzed and can't attack !");
                        return new ConditionAttackModifier(canAttack, 1);
                    }
                }
            },
            // Freeze
            {
                StatusCondition.Freeze,
                new StatusConditionData()
                {
                    Name = "Freeze",
                    HUDName = "FRZ",
                    HUDColor = new Color(0.5882353f, 0.8509804f, 0.8392157f, 1),
                    Description = "",
                    StartMessage = "has been frozen !",
                    EndMessage = "thawed out !",
                    IsVolatile = false,
                    CatchRateModifier = 2,
                    OnBeforeMove = (Pokemon pokemon, Move move) =>
                    {
                        bool canAttack = false;
                        if (Random.Range(0,5) != 0)
                        {
                            pokemon.CureStatus();
                            canAttack = true;
                        }
                        if (!canAttack)
                            BattleEvents.Instance.StatusConditionMessage($"{pokemon.Name} is frozen solid !");
                        return new ConditionAttackModifier(canAttack, 1);
                    }
                }
            },
            // Sleep
            {
                StatusCondition.Sleep,
                new StatusConditionData()
                {
                    Name = "Sleep",
                    HUDName = "SLP",
                    HUDColor = new Color(0.58f, 0.58f, 0.58f, 1),
                    Description = "",
                    StartMessage = "fell asleep !",
                    EndMessage = "woke up !",
                    IsVolatile = false,
                    CatchRateModifier = 2,
                    OnStart = (Pokemon pokemon) =>
                    {
                        pokemon.RemainingStatusTime[StatusCondition.Sleep] = Random.Range(1, 4);
                    },
                    OnBeforeMove = (Pokemon pokemon, Move move) =>
                    {
                        bool canAttack = false;
                        if (pokemon.RemainingStatusTime[StatusCondition.Sleep] == 0)
                        {
                            pokemon.CureStatus();
                            canAttack = true;
                        }
                        if (!canAttack)
                            BattleEvents.Instance.StatusConditionMessage($"{pokemon.Name} is fast asleep !");
                        return new ConditionAttackModifier(canAttack, 1);
                    }
                }
            },

            // Volatile Conditions
            // Confusion
            {
                StatusCondition.Confusion,
                new StatusConditionData()
                {
                    Name = "Confusion",
                    Description = "",
                    StartMessage = "has been confused !",
                    EndMessage = "snapped out of confusion !",
                    IsVolatile = true,
                    OnStart = (Pokemon pokemon) =>
                    {
                        pokemon.RemainingStatusTime[StatusCondition.Confusion] = Random.Range(1, 5);
                        Debug.Log($"Confused for {pokemon.RemainingStatusTime[StatusCondition.Confusion]} turns");
                    },
                    OnBeforeMove = (Pokemon pokemon, Move move) =>
                    {
                        bool canAttack = true;
                        if (pokemon.RemainingStatusTime[StatusCondition.Confusion] == 0)
                        {
                            pokemon.CureVolatileStatus(StatusCondition.Confusion);
                        }
                        else
                        {
                            string msg = $"{pokemon.Name} is confused.";
                            BattleEvents.Instance.StatusConditionMessage(msg);

                            bool hitHimself = Random.Range(0,2) == 0;
                            if (hitHimself)
                            {
                                canAttack = false;
                                msg = "He hurt himself due to confusion";
                                BattleEvents.Instance.StatusConditionMessage(msg);

                                int level = pokemon.Level;
                                int attack = pokemon.Attack;
                                int defense = pokemon.Defense;
                                int power = 40;
                                float damage = ((2 * (float)level / 5 + 2) * attack * power / defense / 50) + 2;
                                GameManager.Instance.BattleManager.DamagePokemon(pokemon, damage);
                            }
                        }

                        return new ConditionAttackModifier(canAttack, 1);
                    }
                }
            }

        };
    }
}
