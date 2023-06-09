using UnityEngine;

public class BattleManagerEndTurnState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public override void EnterState()
    {
        battleManager.PlayerPokemon.OnBattleTurnEnd();
        battleManager.EnemyPokemon.OnBattleTurnEnd();

        CheckFaintedPokemons();
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void OnDestroy() {  }

    private void CheckFaintedPokemons()
    {
        bool playerPokemonFainted = battleManager.PlayerPokemon.IsFainted;
        if (playerPokemonFainted)
        {
            if (battleManager.PlayerParty.GetFirstPokemon() == null)
            {
                battleManager.SwitchState(battleManager.EndBattleState);
            }
            else
            {
                battleManager.BattleActionSelectorsUIManager.OpenFaintedPokemonReplacementMenu(OnReplacingPokemonSelected);
            }
        }
        else
        {
            ReplaceEnemyFaintedPokemon();
        }
    }

    private void OnReplacingPokemonSelected(int pokemonIndex)
    {
        Pokemon nextPokemon = battleManager.PlayerParty.Pokemons[pokemonIndex];
        battleManager.SwitchPokemon(battleManager.PlayerPokemon, nextPokemon);

        ReplaceEnemyFaintedPokemon();
    }

    private void ReplaceEnemyFaintedPokemon()
    {
        if (battleManager.EnemyPokemon.IsFainted)
        {
            Pokemon nextPokemon = battleManager.EnemyParty.GetFirstPokemon();
            if (nextPokemon == null)
            {
                battleManager.SwitchState(battleManager.EndBattleState);
                return;
            }
            battleManager.SwitchPokemon(battleManager.EnemyPokemon, nextPokemon);
        }
        battleManager.SwitchState(battleManager.StartTurnState);
    }
}
