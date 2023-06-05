using System.Collections.Generic;
using UnityEngine;

public class LongGrass : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        CheckForEncounter();
    }

    private void CheckForEncounter()
    {
        if (Random.Range(0, 100) < GameManager.Instance.CurrentArea.EncounterRate)
        {
            Pokemon enemyPokemon = GameManager.Instance.CurrentArea.GetRandomWildPokemon();
            PokemonParty enemyParty = new PokemonParty(new List<Pokemon>() { enemyPokemon });
            GameManager.Instance.StartBattle(enemyParty, null);
        }
    }
}
