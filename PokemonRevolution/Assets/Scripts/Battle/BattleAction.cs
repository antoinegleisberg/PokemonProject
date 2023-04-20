using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
    
    public BattleActionInfo(BattleAction battleAction)
    {
        Assert.IsTrue(battleAction == BattleAction.Run);
        BattleAction = battleAction;
        ActionParameter = 0;
        SourcePokemon = null;
        TargetPokemon = null;
        TargetPokemonPosition = 0;
    }
    
    public BattleActionInfo(BattleAction battleAction, int actionParameter)
    {
        Assert.IsTrue(battleAction == BattleAction.SwitchPokemon);
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = null;
        TargetPokemon = null;
        TargetPokemonPosition = 0;
    }
    
    public BattleActionInfo(BattleAction battleAction, int actionParameter, Pokemon targetPokemon)
    {
        Assert.IsTrue(battleAction == BattleAction.UseMedicine || battleAction == BattleAction.UseStatusHealer || battleAction == BattleAction.UsePokeball);
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = null;
        TargetPokemon = targetPokemon;
        TargetPokemonPosition = 0;
    }
    
    public BattleActionInfo(BattleAction battleAction, int actionParameter, Pokemon sourcePokemon, int targetPokemonPosition)
    {
        Assert.IsTrue(battleAction == BattleAction.Attack);
        BattleAction = battleAction;
        ActionParameter = actionParameter;
        SourcePokemon = sourcePokemon;
        TargetPokemon = null;
        TargetPokemonPosition = targetPokemonPosition;
    }
}