using UnityEngine;

public class BattleManagerEndTurnState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        _battleManager = battleManager;
        BattleUIEvents.Instance.OnReplacePokemonSelected += OnFaintedPokemonReplacementSelected;
    }

    public override void EnterState()
    {
        _battleManager.PlayerPokemon.OnBattleTurnEnd();
        _battleManager.EnemyPokemon.OnBattleTurnEnd();

        CheckFaintedPokemons();
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void OnDestroy()
    {
        BattleUIEvents.Instance.OnReplacePokemonSelected -= OnFaintedPokemonReplacementSelected;
    }

    private void CheckFaintedPokemons()
    {
        bool playerPokemonFainted = _battleManager.PlayerPokemon.IsFainted;
        if (playerPokemonFainted)
        {
            if (_battleManager.PlayerParty.GetFirstPokemon() == null)
            {
                _battleManager.SwitchState(_battleManager.EndBattleState);
            }
            else
            {
                _battleManager.BattleActionSelectorsUIManager.OpenFaintedPokemonReplacementMenu();
            }
        }
        else
        {
            ReplaceEnemyFaintedPokemon();
        }
    }

    private void OnReplacingPokemonSelected(int pokemonIndex)
    {
        Pokemon nextPokemon = _battleManager.PlayerParty.Pokemons[pokemonIndex];
        _battleManager.SwitchPokemon(_battleManager.PlayerPokemon, nextPokemon);

        ReplaceEnemyFaintedPokemon();
    }

    private void ReplaceEnemyFaintedPokemon()
    {
        if (_battleManager.EnemyPokemon.IsFainted)
        {
            Pokemon nextPokemon = _battleManager.EnemyParty.GetFirstPokemon();
            if (nextPokemon == null)
            {
                _battleManager.SwitchState(_battleManager.EndBattleState);
                return;
            }
            _battleManager.SwitchPokemon(_battleManager.EnemyPokemon, nextPokemon);
        }
        _battleManager.SwitchState(_battleManager.StartTurnState);
    }

    private void OnFaintedPokemonReplacementSelected(int pokemonIndex)
    {
        OnReplacingPokemonSelected(pokemonIndex);
    }
}
