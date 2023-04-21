using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelectorUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform moveButtonsContainer;

    [SerializeField] private Color outOfPPColor;

    private List<Button> moveButtons;

    private void InitButtonsList()
    {
        moveButtons = new List<Button>();
        foreach (Button button in moveButtonsContainer.GetComponentsInChildren<Button>())
        {
            moveButtons.Add(button);
        }
    }

    public void UpdateMovesUI(Pokemon playerPokemon)
    {
        if (playerPokemon.Owner != PokemonOwner.Player)
            return;

        if (moveButtons == null)
            InitButtonsList();

        for (int i = 0; i < playerPokemon.Moves.Count; i++)
        {
            moveButtons[i].gameObject.SetActive(true);
            moveButtons[i].transform.Find("MoveName").GetComponent<TextMeshProUGUI>().text = playerPokemon.Moves[i].ScriptableMove.Name;
            moveButtons[i].transform.Find("Image").GetComponent<Image>().sprite = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeIcon;
            moveButtons[i].transform.Find("PPText").GetComponent<TextMeshProUGUI>().text = "PP " + playerPokemon.Moves[i].CurrentPP + "/" + playerPokemon.Moves[i].ScriptableMove.PP;
            moveButtons[i].GetComponent<Image>().color = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeColor;

            if (playerPokemon.Moves[i].CurrentPP == 0)
            {
                moveButtons[i].interactable = false;
                moveButtons[i].GetComponent<Image>().color = outOfPPColor;
            }
            else
            {
                moveButtons[i].interactable = true;
            }
        }
        for (int i = playerPokemon.Moves.Count; i < 4; i++)
        {
            moveButtons[i].gameObject.SetActive(false);
        }
    }
}
