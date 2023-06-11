using System.Collections.Generic;
using UnityEngine;

public class BattleManagerActionSelectionState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        _battleManager = battleManager;

        BattleUIEvents.Instance.OnRunButtonPressed += OnRunSelected;
        BattleUIEvents.Instance.OnMoveSelected += OnMoveSelected;
        BattleUIEvents.Instance.OnSwitchPokemonSelected += OnSwitchPokemonSelected;
        BattleUIEvents.Instance.OnPokeballButtonPressed += OnPokeballSelected;
        BattleUIEvents.Instance.OnMedicineButtonPressed += OnMedicineSelected;
        BattleUIEvents.Instance.OnStatusHealerButtonPressed += OnStatusHealerSelected;

    }

    public override void EnterState()
    {
        _battleManager.BattleActionSelectorsUIManager.SetActiveSelectorToActionSelector();
        _battleManager.BattleDialogueUIManager.OnEnterActionSelection();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        _battleManager.NextEnemyAction = GetEnemyAction();
        
        _battleManager.BattleActionSelectorsUIManager.DeactivateAllSelectors();
    }

    public override void OnDestroy()
    {
        BattleUIEvents.Instance.OnRunButtonPressed -= OnRunSelected;
        BattleUIEvents.Instance.OnMoveSelected -= OnMoveSelected;
        BattleUIEvents.Instance.OnSwitchPokemonSelected -= OnSwitchPokemonSelected;
        BattleUIEvents.Instance.OnPokeballButtonPressed -= OnPokeballSelected;
        BattleUIEvents.Instance.OnMedicineButtonPressed -= OnMedicineSelected;
        BattleUIEvents.Instance.OnStatusHealerButtonPressed -= OnStatusHealerSelected;
    }

    private BattleActionInfo GetEnemyAction()
    {
        int randomEnemyMove = GetRandomEnemyMove();

        if (randomEnemyMove == -1)
        {
            Debug.Log("The enemy pokemon ran !");
            return new BattleActionInfo(BattleAction.Run);
        }
        
        return new BattleActionInfo(BattleAction.Attack, randomEnemyMove, _battleManager.EnemyPokemon, 0);
    }

    private int GetRandomEnemyMove()
    {
        List<int> enemyMovesIndexes = new List<int>();
        for (int i = 0; i < _battleManager.EnemyPokemon.Moves.Count; i++)
            enemyMovesIndexes.Add(i);

        int randomEnemyMove = -1;
        while (enemyMovesIndexes.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyMovesIndexes.Count);
            if (_battleManager.EnemyPokemon.Moves[randomIndex].CurrentPP > 0)
            {
                randomEnemyMove = randomIndex;
                break;
            }
            else
            {
                enemyMovesIndexes.RemoveAt(randomIndex);
            }
        }
        return randomEnemyMove;
    }

    private void OnRunSelected()
    {
        _battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.Run);
        _battleManager.SwitchState(_battleManager.PerformMovesState);
    }

    private void OnMoveSelected(int moveIndex)
    {
        _battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.Attack, moveIndex, _battleManager.PlayerPokemon, 1);
        _battleManager.SwitchState(_battleManager.PerformMovesState);
    }

    private void OnSwitchPokemonSelected(int pokemonIndex)
    {
        _battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.SwitchPokemon, pokemonIndex);
        _battleManager.SwitchState(_battleManager.PerformMovesState);
    }

    private void OnPokeballSelected(int pokeballIndex)
    {
        _battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.UsePokeball, pokeballIndex, _battleManager.EnemyPokemon);
        _battleManager.SwitchState(_battleManager.PerformMovesState);
    }

    private void OnMedicineSelected(int medicineIndex)
    {
        _battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.UseMedicine, medicineIndex, _battleManager.PlayerPokemon);
        _battleManager.SwitchState(_battleManager.PerformMovesState);
    }

    private void OnStatusHealerSelected(int statusHealerIndex)
    {
        _battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.UseStatusHealer, statusHealerIndex, _battleManager.PlayerPokemon);
        _battleManager.SwitchState(_battleManager.PerformMovesState);
    }
}
