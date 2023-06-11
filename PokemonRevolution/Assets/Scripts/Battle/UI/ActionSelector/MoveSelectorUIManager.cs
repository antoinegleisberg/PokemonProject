using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelectorUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _moveButtonsContainer;

    [SerializeField] private Color _outOfPPColor;

    private List<Button> _moveButtons;
    private List<TextMeshProUGUI> _moveNames;
    private List<Image> _moveTypeImages;
    private List<TextMeshProUGUI> _movePPTexts;

    private void InitButtonsList()
    {
        _moveButtons = new List<Button>();
        _moveNames = new List<TextMeshProUGUI>();
        _moveTypeImages = new List<Image>();
        _movePPTexts = new List<TextMeshProUGUI>();
        foreach (Button button in _moveButtonsContainer.GetComponentsInChildren<Button>())
        {
            _moveButtons.Add(button);
            _moveNames.Add(button.transform.Find("MoveName").GetComponent<TextMeshProUGUI>());
            _moveTypeImages.Add(button.transform.Find("Image").GetComponent<Image>());
            _movePPTexts.Add(button.transform.Find("PPText").GetComponent<TextMeshProUGUI>());
        }
    }

    public void UpdateMovesUI(Pokemon playerPokemon)
    {
        if (playerPokemon.Owner != PokemonOwner.Player)
            return;

        if (_moveButtons == null)
            InitButtonsList();

        for (int i = 0; i < playerPokemon.Moves.Count; i++)
        {
            _moveButtons[i].gameObject.SetActive(true);
            _moveNames[i].text = playerPokemon.Moves[i].ScriptableMove.Name;
            _moveTypeImages[i].sprite = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeIcon;
            _movePPTexts[i].text = "PP " + playerPokemon.Moves[i].CurrentPP + "/" + playerPokemon.Moves[i].ScriptableMove.PP;

            if (playerPokemon.Moves[i].CurrentPP == 0)
            {
                _moveButtons[i].interactable = false;
                _moveButtons[i].GetComponent<Image>().color = _outOfPPColor;
            }
            else
            {
                _moveButtons[i].interactable = true;
                _moveButtons[i].GetComponent<Image>().color = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeColor;
            }
        }
        for (int i = playerPokemon.Moves.Count; i < Pokemon.MaxNumberMoves; i++)
        {
            _moveButtons[i].gameObject.SetActive(false);
        }
    }
}
