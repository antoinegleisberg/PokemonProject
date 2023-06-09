using System.Collections.Generic;
using UnityEngine;

public class BattleManagerActionSelectionState : BattleManagerBaseState
{
    public override void InitState(BattleManager battleManager)
    {
        this.battleManager = battleManager;

        BattleUIEvents.Instance.OnRunButtonPressed += OnRunSelected;
        BattleUIEvents.Instance.OnMoveSelected += OnMoveSelected;
        BattleUIEvents.Instance.OnSwitchPokemonSelected += OnSwitchPokemonSelected;
        BattleUIEvents.Instance.OnPokeballButtonPressed += OnPokeballSelected;
        BattleUIEvents.Instance.OnMedicineButtonPressed += OnMedicineSelected;
        BattleUIEvents.Instance.OnStatusHealerButtonPressed += OnStatusHealerSelected;

    }

    public override void EnterState()
    {
        battleManager.BattleActionSelectorsUIManager.SetActiveSelectorToActionSelector();
        battleManager.BattleDialogueUIManager.OnEnterActionSelection();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        battleManager.NextEnemyAction = GetEnemyAction();
        
        battleManager.BattleActionSelectorsUIManager.DeactivateAllSelectors();
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
        
        return new BattleActionInfo(BattleAction.Attack, randomEnemyMove, battleManager.EnemyPokemon, 0);
    }

    private int GetRandomEnemyMove()
    {
        List<int> enemyMovesIndexes = new List<int>();
        for (int i = 0; i < battleManager.EnemyPokemon.Moves.Count; i++)
            enemyMovesIndexes.Add(i);

        int randomEnemyMove = -1;
        while (enemyMovesIndexes.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyMovesIndexes.Count);
            if (battleManager.EnemyPokemon.Moves[randomIndex].CurrentPP > 0)
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
        battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.Run);
        battleManager.SwitchState(battleManager.PerformMovesState);
    }

    private void OnMoveSelected(int moveIndex)
    {
        battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.Attack, moveIndex, battleManager.PlayerPokemon, 1);
        battleManager.SwitchState(battleManager.PerformMovesState);
    }

    private void OnSwitchPokemonSelected(int pokemonIndex)
    {
        battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.SwitchPokemon, pokemonIndex);
        battleManager.SwitchState(battleManager.PerformMovesState);
    }

    private void OnPokeballSelected(int pokeballIndex)
    {
        battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.UsePokeball, pokeballIndex, battleManager.EnemyPokemon);
        battleManager.SwitchState(battleManager.PerformMovesState);
    }

    private void OnMedicineSelected(int medicineIndex)
    {
        battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.UseMedicine, medicineIndex, battleManager.PlayerPokemon);
        battleManager.SwitchState(battleManager.PerformMovesState);
    }

    private void OnStatusHealerSelected(int statusHealerIndex)
    {
        battleManager.NextPlayerAction = new BattleActionInfo(BattleAction.UseStatusHealer, statusHealerIndex, battleManager.PlayerPokemon);
        battleManager.SwitchState(battleManager.PerformMovesState);
    }
}
