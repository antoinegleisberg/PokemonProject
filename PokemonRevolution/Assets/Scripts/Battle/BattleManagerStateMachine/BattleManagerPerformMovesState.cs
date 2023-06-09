using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerPerformMovesState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager) => this.battleManager = battleManager;

    public override void EnterState()
    {
        PerformTurnActions();
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void OnDestroy() { }

    private void PerformTurnActions()
    {
        Queue<BattleActionInfo> actions = GetBattleActionsInOrder();

        battleManager.StartCoroutine(PerformTurnActions(actions));
    }

    private Queue<BattleActionInfo> GetBattleActionsInOrder()
    {
        Queue<BattleActionInfo> actions = new Queue<BattleActionInfo>();

        // Check for action priorities
        if (battleManager.NextPlayerAction.BattleAction != BattleAction.Attack)
        {
            actions.Enqueue(battleManager.NextPlayerAction);
            actions.Enqueue(battleManager.NextEnemyAction);
        }
        else if (battleManager.NextEnemyAction.BattleAction != BattleAction.Attack)
        {
            actions.Enqueue(battleManager.NextEnemyAction);
            actions.Enqueue(battleManager.NextPlayerAction);
        }
        // None of the teams used priority action; check for pokemon speed
        else
        {
            int playerSpeed = battleManager.NextPlayerAction.SourcePokemon.Speed;
            int enemySpeed = battleManager.NextEnemyAction.SourcePokemon.Speed;

            int playerMoveIndex = battleManager.NextPlayerAction.ActionParameter;
            Move playerMove = battleManager.NextPlayerAction.SourcePokemon.Moves[playerMoveIndex];
            int playerPriority = playerMove.ScriptableMove.Priority;

            int enemyMoveIndex = battleManager.NextEnemyAction.ActionParameter;
            Move enemyMove = battleManager.NextEnemyAction.SourcePokemon.Moves[enemyMoveIndex];
            int enemyPriority = enemyMove.ScriptableMove.Priority;

            bool playerGoesFirst;
            if (enemyPriority > playerPriority)
                playerGoesFirst = false;
            else if (enemyPriority < playerPriority)
                playerGoesFirst = true;
            else if (enemySpeed < playerSpeed)
                playerGoesFirst = true;
            else if (enemySpeed > playerSpeed)
                playerGoesFirst = false;
            else if (Random.Range(0, 2) == 0)
                playerGoesFirst = true;
            else
                playerGoesFirst = false;

            if (playerGoesFirst)
            {
                actions.Enqueue(battleManager.NextPlayerAction);
                actions.Enqueue(battleManager.NextEnemyAction);
            }
            else
            {
                actions.Enqueue(battleManager.NextEnemyAction);
                actions.Enqueue(battleManager.NextPlayerAction);
            }
        }

        return actions;
    }

    private IEnumerator PerformTurnActions(Queue<BattleActionInfo> actions)
    {
        bool continueBattle = true;
        while (actions.Count > 0 && continueBattle)
        {
            BattleActionInfo nextAction = actions.Dequeue();

            if (nextAction.BattleAction == BattleAction.Attack &&
                nextAction.SourcePokemon.IsFainted)
            {
                // The attack faild because attacker is fainted
                continue;
            }

            continueBattle = PerformAction(nextAction);

            yield return BattleUIManager.Instance.WaitWhileBusy();
        }

        if (continueBattle)
            battleManager.SwitchState(battleManager.EndTurnState);
        else
            battleManager.SwitchState(battleManager.EndBattleState);
    }

    // return wether the battle continues or not
    private bool PerformAction(BattleActionInfo battleActionInfo)
    {
        switch (battleActionInfo.BattleAction)
        {
            case BattleAction.Attack:
                int moveIndex = battleActionInfo.ActionParameter;
                Move move = battleActionInfo.SourcePokemon.Moves[moveIndex];
                Pokemon targetPokemon = (battleActionInfo.TargetPokemonPosition == 0) ? battleManager.PlayerPokemon : battleManager.EnemyPokemon;
                battleManager.PerformMove(battleActionInfo.SourcePokemon, targetPokemon, move);
                return true;

            case BattleAction.SwitchPokemon:
                int pokemonIndex = battleActionInfo.ActionParameter;
                Pokemon newPokemon = battleManager.PlayerParty.Pokemons[pokemonIndex];
                battleManager.SwitchPokemon(battleManager.PlayerPokemon, newPokemon);
                return true;

            case BattleAction.Run:
                bool succededToRun = battleManager.CanRunFromBattle();
                return !succededToRun;

            case BattleAction.UsePokeball:
                bool coughtPokemon = battleManager.CanCatchPokemon();
                return !coughtPokemon;

            default:
                Debug.Log("Used something else than attack, switch , pokeball or run : not implemented yet");
                return true;
        }
    }
}
