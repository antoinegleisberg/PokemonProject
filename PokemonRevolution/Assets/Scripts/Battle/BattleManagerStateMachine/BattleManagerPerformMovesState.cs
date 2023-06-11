using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerPerformMovesState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

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

        _battleManager.StartCoroutine(PerformTurnActions(actions));
    }

    private Queue<BattleActionInfo> GetBattleActionsInOrder()
    {
        Queue<BattleActionInfo> actions = new Queue<BattleActionInfo>();

        // Check for action priorities
        if (_battleManager.NextPlayerAction.BattleAction != BattleAction.Attack)
        {
            actions.Enqueue(_battleManager.NextPlayerAction);
            actions.Enqueue(_battleManager.NextEnemyAction);
        }
        else if (_battleManager.NextEnemyAction.BattleAction != BattleAction.Attack)
        {
            actions.Enqueue(_battleManager.NextEnemyAction);
            actions.Enqueue(_battleManager.NextPlayerAction);
        }
        // None of the teams used priority action; check for pokemon speed
        else
        {
            int playerSpeed = _battleManager.NextPlayerAction.SourcePokemon.Speed;
            int enemySpeed = _battleManager.NextEnemyAction.SourcePokemon.Speed;

            int playerMoveIndex = _battleManager.NextPlayerAction.ActionParameter;
            Move playerMove = _battleManager.NextPlayerAction.SourcePokemon.Moves[playerMoveIndex];
            int playerPriority = playerMove.ScriptableMove.Priority;

            int enemyMoveIndex = _battleManager.NextEnemyAction.ActionParameter;
            Move enemyMove = _battleManager.NextEnemyAction.SourcePokemon.Moves[enemyMoveIndex];
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
                actions.Enqueue(_battleManager.NextPlayerAction);
                actions.Enqueue(_battleManager.NextEnemyAction);
            }
            else
            {
                actions.Enqueue(_battleManager.NextEnemyAction);
                actions.Enqueue(_battleManager.NextPlayerAction);
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

            continueBattle = PerformAction(nextAction).ContinueBattle;

            yield return BattleUIManager.Instance.WaitWhileBusy();
        }

        if (continueBattle)
            _battleManager.SwitchState(_battleManager.EndTurnState);
        else
            _battleManager.SwitchState(_battleManager.EndBattleState);
    }
    
    private PerformedActionInfo PerformAction(BattleActionInfo battleActionInfo)
    {
        switch (battleActionInfo.BattleAction)
        {
            case BattleAction.Attack:
                int moveIndex = battleActionInfo.ActionParameter;
                Move move = battleActionInfo.SourcePokemon.Moves[moveIndex];
                Pokemon targetPokemon = (battleActionInfo.TargetPokemonPosition == 0) ? _battleManager.PlayerPokemon : _battleManager.EnemyPokemon;
                _battleManager.PerformMove(battleActionInfo.SourcePokemon, targetPokemon, move);
                return new PerformedActionInfo(true);

            case BattleAction.SwitchPokemon:
                int pokemonIndex = battleActionInfo.ActionParameter;
                Pokemon newPokemon = _battleManager.PlayerParty.Pokemons[pokemonIndex];
                _battleManager.SwitchPokemon(_battleManager.PlayerPokemon, newPokemon);
                return new PerformedActionInfo(true);

            case BattleAction.Run:
                bool succededToRun = _battleManager.CanRunFromBattle();
                return new PerformedActionInfo(!succededToRun);

            case BattleAction.UsePokeball:
                bool coughtPokemon = _battleManager.CanCatchPokemon();
                return new PerformedActionInfo(!coughtPokemon);

            default:
                Debug.Log("Used something else than attack, switch , pokeball or run : not implemented yet");
                return new PerformedActionInfo(true);
        }
    }
}

public struct PerformedActionInfo
{
    public bool ContinueBattle;

    public PerformedActionInfo(bool continueBattle)
    {
        ContinueBattle = continueBattle;
    }
}