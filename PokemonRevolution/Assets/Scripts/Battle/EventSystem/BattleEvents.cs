using System;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Instance;

    private void Awake() => Instance = this;

    public event Action<PokemonParty, PokemonParty> OnBattleStart;
    public event Action OnBattleEnd;

    public event Action OnEnterActionSelection;
    public event Action OnExitActionSelection;
    
    public event Action<Pokemon, Pokemon, Move, AttackInfo> OnPokemonAttack;
    public event Action<Pokemon, int> OnPokemonDamaged;
    public event Action<Pokemon> OnPokemonFainted;

    public event Action<Stat, int, Pokemon> OnPokemonStatBoosted;
    public event Action<StatusCondition, Pokemon> OnStatusConditionApplied;
    public event Action<StatusCondition, Pokemon> OnStatusConditionRemoved;
    public event Action<string> OnStatusConditionMessage;

    public event Action OnReplaceFaintedPokemon;
    public event Action<Pokemon> OnPokemonSwitchedOut;
    public event Action<Pokemon> OnPokemonSwitchedIn;

    public void StartBattle(PokemonParty playerParty, PokemonParty enemyParty) => OnBattleStart?.Invoke(playerParty, enemyParty);
    public void EndBattle() => OnBattleEnd?.Invoke();

    public void EnterActionSelectionState() => OnEnterActionSelection?.Invoke();
    public void ExitActionSelectionState() => OnExitActionSelection?.Invoke();

    public void PokemonAttacks(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo) => OnPokemonAttack?.Invoke(attacker, defender, move, attackInfo);
    public void PokemonDamaged(Pokemon pokemon, int damage) => OnPokemonDamaged?.Invoke(pokemon, damage);
    public void PokemonFaints(Pokemon pokemon) => OnPokemonFainted?.Invoke(pokemon);

    public void BoostedPokemonStat(Stat stat, int boost, Pokemon pokemon) => OnPokemonStatBoosted?.Invoke(stat, boost, pokemon);
    public void AppliedStatusCondition(StatusCondition status, Pokemon pokemon) => OnStatusConditionApplied?.Invoke(status, pokemon);
    public void RemovedStatusCondition(StatusCondition status, Pokemon pokemon) => OnStatusConditionRemoved?.Invoke(status, pokemon);
    public void StatusConditionMessage(string message) => OnStatusConditionMessage?.Invoke(message);

    public void ReplaceFaintedPokemon() => OnReplaceFaintedPokemon?.Invoke();
    public void PokemonSwitchedOut(Pokemon pokemon) => OnPokemonSwitchedOut?.Invoke(pokemon);
    public void PokemonSwitchedIn(Pokemon pokemon) => OnPokemonSwitchedIn?.Invoke(pokemon);
}
