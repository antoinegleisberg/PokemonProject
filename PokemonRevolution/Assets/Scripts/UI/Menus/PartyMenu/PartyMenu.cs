using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _partyMenuNavigator;

    private PokemonParty _playerParty;

    private void Awake()
    {
        _playerParty = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty;
    }
}
