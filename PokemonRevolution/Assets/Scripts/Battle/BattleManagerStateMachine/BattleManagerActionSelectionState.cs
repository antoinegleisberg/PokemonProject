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
        BattleEvents.Instance.EnterActionSelectionState();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        int randomEnemyMove = Random.Range(0, battleManager.EnemyPokemon.Moves.Count);
        battleManager.NextEnemyAction = new BattleActionInfo(BattleAction.Attack, randomEnemyMove, battleManager.EnemyPokemon, 0);

        BattleEvents.Instance.ExitActionSelectionState();
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
