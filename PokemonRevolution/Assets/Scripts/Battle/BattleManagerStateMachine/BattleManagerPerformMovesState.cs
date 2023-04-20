using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerPerformMovesState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager) => this.battleManager = battleManager;

    public override void EnterState()
    {
        battleManager.StartCoroutine(PerformTurnActions());
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void OnDestroy() { }

    private IEnumerator PerformTurnActions()
    {
        PerformAction(battleManager.NextPlayerAction);

        yield return new WaitForEndOfFrame();

        while (UIManager.Instance.IsBusy)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (battleManager.NextPlayerAction.BattleAction != BattleAction.Run)
        {
            if (!battleManager.NextEnemyAction.SourcePokemon.IsFainted)
            {
                PerformAction(battleManager.NextEnemyAction);
            }

            battleManager.SwitchState(battleManager.EndTurnState);
        }
    }

    private void PerformAction(BattleActionInfo battleActionInfo)
    {
        switch (battleActionInfo.BattleAction)
        {
            case BattleAction.Attack:
                int moveIndex = battleActionInfo.ActionParameter;
                Move move = battleActionInfo.SourcePokemon.Moves[moveIndex];
                Pokemon targetPokemon = (battleActionInfo.TargetPokemonPosition == 0) ? battleManager.PlayerPokemon : battleManager.EnemyPokemon;
                battleManager.PerformMove(battleActionInfo.SourcePokemon, targetPokemon, move);
                break;

            case BattleAction.SwitchPokemon:
                int pokemonIndex = battleActionInfo.ActionParameter;
                Pokemon newPokemon = battleManager.PlayerParty.Pokemons[pokemonIndex];
                BattleEvents.Instance.PokemonSwitchedOut(battleManager.PlayerPokemon);
                battleManager.PlayerPokemon = newPokemon;
                BattleEvents.Instance.PokemonSwitchedIn(battleManager.PlayerPokemon);
                break;

            case BattleAction.Run:
                battleManager.SwitchState(battleManager.EndBattleState);
                break;
                
            default:
                Debug.Log("Used something else than an attack move : not implemented yet");
                break;
        }
        
    }
}
