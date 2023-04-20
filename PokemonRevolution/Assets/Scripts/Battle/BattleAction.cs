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
    public int TargetPokemonPosition; // for now, 0 for player, 1 for enemy

    // For BattleAction.Run
    public BattleActionInfo(BattleAction battleAction)
    {
        BattleAction = battleAction;
        ActionParameter = 0;
        SourcePokemon = null;
        TargetPokemon = null;
        TargetPokemonPosition = 0;
    }

    // For BattleAction.SwitchPokemon
    public BattleActionInfo(BattleAction battleAction, int actionParameter)
    {
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = null;
        TargetPokemon = null;
        TargetPokemonPosition = 0;
    }

    // For BattleAction.UseMedicine, BattleAction.UseStatusHealer and BattleAction.UsePokeball
    public BattleActionInfo(BattleAction battleAction, int actionParameter, Pokemon targetPokemon)
    {
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = null;
        TargetPokemon = targetPokemon;
        TargetPokemonPosition = 0;
    }

    // For BattleAction.Attack
    public BattleActionInfo(BattleAction battleAction, int actionParameter, Pokemon sourcePokemon, int targetPokemonPosition)
    {
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = sourcePokemon;
        TargetPokemon = null;
        TargetPokemonPosition = targetPokemonPosition;
    }
}