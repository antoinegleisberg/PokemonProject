using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleActionSelectorUIManager : MonoBehaviour
{
    [SerializeField] private GameObject actionSelector;
    [SerializeField] private GameObject moveSelector;
    private List<GameObject> selectors;

    [SerializeField] private GameObject move0Button;
    [SerializeField] private GameObject move1Button;
    [SerializeField] private GameObject move2Button;
    [SerializeField] private GameObject move3Button;
    [SerializeField] private TextMeshProUGUI move0Name;
    [SerializeField] private TextMeshProUGUI move1Name;
    [SerializeField] private TextMeshProUGUI move2Name;
    [SerializeField] private TextMeshProUGUI move3Name;
    [SerializeField] private Image move0Image;
    [SerializeField] private Image move1Image;
    [SerializeField] private Image move2Image;
    [SerializeField] private Image move3Image;
    [SerializeField] private TextMeshProUGUI move0PPText;
    [SerializeField] private TextMeshProUGUI move1PPText;
    [SerializeField] private TextMeshProUGUI move2PPText;
    [SerializeField] private TextMeshProUGUI move3PPText;
    private List<GameObject> moveButtons;
    private List<TextMeshProUGUI> moveNames;
    private List<Image> moveImages;
    private List<TextMeshProUGUI> movePPTexts;


    private void Start()
    {
        selectors = new List<GameObject> { actionSelector, moveSelector };
        moveButtons = new List<GameObject> { move0Button, move1Button, move2Button, move3Button };
        moveNames = new List<TextMeshProUGUI> { move0Name, move1Name, move2Name, move3Name };
        moveImages = new List<Image> { move0Image, move1Image, move2Image, move3Image };
        movePPTexts = new List<TextMeshProUGUI> { move0PPText, move1PPText, move2PPText, move3PPText };

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.Current.OnEnterBattle += OnEnterBattle;

        BattleUIEvents.Current.OnAttackButtonPressed += OnAttackButtonPressed;
        BattleUIEvents.Current.OnSwitchPokemonButtonPressed += OnSwitchPokemonButtonPressed;
        BattleUIEvents.Current.OnBagButtonPressed += OnBagButtonPressed;
        BattleUIEvents.Current.OnRunButtonPressed += DeactivateActionSelectors;

        BattleUIEvents.Current.OnMoveSelected += OnMoveSelected;
        BattleUIEvents.Current.OnCancelMoveSelection += SetActiveSelectorToActionSelector;

        BattleEvents.Current.OnPokemonAttack += OnPokemonAttack;
        BattleEvents.Current.OnEnterPlayerTurn += SetActiveSelectorToActionSelector;
        BattleEvents.Current.OnPokemonSwitched += OnPokemonSwitched;
    }
    private void UnsubscribeToEvents()
    {
        GameEvents.Current.OnEnterBattle -= OnEnterBattle;

        BattleUIEvents.Current.OnAttackButtonPressed -= OnAttackButtonPressed;
        BattleUIEvents.Current.OnSwitchPokemonButtonPressed -= OnSwitchPokemonButtonPressed;
        BattleUIEvents.Current.OnBagButtonPressed -= OnBagButtonPressed;
        BattleUIEvents.Current.OnRunButtonPressed -= DeactivateActionSelectors;

        BattleUIEvents.Current.OnMoveSelected -= OnMoveSelected;
        BattleUIEvents.Current.OnCancelMoveSelection -= SetActiveSelectorToActionSelector;

        BattleEvents.Current.OnPokemonAttack -= OnPokemonAttack;
        BattleEvents.Current.OnEnterPlayerTurn -= SetActiveSelectorToActionSelector;
        BattleEvents.Current.OnPokemonSwitched -= OnPokemonSwitched;
    }

    private void SetActiveSelector(GameObject selector)
    {
        foreach (GameObject s in selectors)
        {
            s.SetActive(false);
        }
        selector?.SetActive(true);
    }

    private void OnEnterBattle(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        SetActiveSelector(null);

        for (int i = 0; i < playerPokemon.Moves.Count; i++)
        {
            moveButtons[i].SetActive(true);
            moveNames[i].text = playerPokemon.Moves[i].ScriptableMove.Name;
            moveImages[i].sprite = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeIcon;
            movePPTexts[i].text = "PP " + playerPokemon.Moves[i].CurrentPP + "/" + playerPokemon.Moves[i].ScriptableMove.PP;
            moveButtons[i].GetComponent<Image>().color = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeColor;
        }
        for (int i = playerPokemon.Moves.Count; i < 4; i++)
        {
            moveButtons[i].SetActive(false);
        }
    }

    private void SetActiveSelectorToActionSelector() => SetActiveSelector(actionSelector);
    private void DeactivateActionSelectors() => SetActiveSelector(null);

    private void OnAttackButtonPressed()
    {
        SetActiveSelector(moveSelector);
    }

    private void OnSwitchPokemonButtonPressed()
    {
        // TODO
    }

    private void OnBagButtonPressed()
    {
        // TODO
    }

    private void OnMoveSelected(int moveIndex)
    {
        SetActiveSelector(null);
    }

    private void OnPokemonAttack(Pokemon attacker, Pokemon defender, Move move, AttackInfo attackInfo)
    {
        if (!(attacker.Owner == PokemonOwner.Player)) return;
        for (int i = 0; i < attacker.Moves.Count; i++)
        {
            movePPTexts[i].text = "PP " + attacker.Moves[i].CurrentPP + "/" + attacker.Moves[i].ScriptableMove.PP;
        }
    }

    private void OnPokemonSwitched(Pokemon oldPokemon, Pokemon newPokemon)
    {
        if (newPokemon.Owner != PokemonOwner.Player)
            return;

        SetActiveSelector(null);

        for (int i = 0; i < newPokemon.Moves.Count; i++)
        {
            moveButtons[i].SetActive(true);
            moveNames[i].text = newPokemon.Moves[i].ScriptableMove.Name;
            moveImages[i].sprite = TypeUtils.TypeInfo(newPokemon.Moves[i].ScriptableMove.Type).TypeIcon;
            movePPTexts[i].text = "PP " + newPokemon.Moves[i].CurrentPP + "/" + newPokemon.Moves[i].ScriptableMove.PP;
            moveButtons[i].GetComponent<Image>().color = TypeUtils.TypeInfo(newPokemon.Moves[i].ScriptableMove.Type).TypeColor;
        }
        for (int i = newPokemon.Moves.Count; i < 4; i++)
        {
            moveButtons[i].SetActive(false);
        }
    }
}