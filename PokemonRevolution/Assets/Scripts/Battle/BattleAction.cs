using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleAction
{
    Attack,
    SwitchPokemon,
    Run,
    UsePokeball,
    UseMedicine,
    UseStatusHealer
}

public struct BattleActionInfo
{
    public BattleAction BattleAction;
    public int ActionParameter;
    public Pokemon SourcePokemon;
    public Pokemon TargetPokemon;

    public BattleActionInfo(BattleAction battleAction, int actionParameter)
    {
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = null;
        TargetPokemon = null;
    }

    public BattleActionInfo(BattleAction battleAction, int actionParameter, Pokemon targetPokemon)
    {
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = null;
        TargetPokemon = targetPokemon;
    }

    public BattleActionInfo(BattleAction battleAction, int actionParameter, Pokemon sourcePokemon, Pokemon targetPokemon)
    {
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = sourcePokemon;
        TargetPokemon = targetPokemon;
    }
}