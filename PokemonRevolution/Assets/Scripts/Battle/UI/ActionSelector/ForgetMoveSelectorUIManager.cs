using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgetMoveSelectorUIManager : MonoBehaviour
{
    [SerializeField] private List<Button> moveButtons;
    [SerializeField] private Button newMoveButton;

    public void UpdateUI(Pokemon pokemon, ScriptableMove newMove)
    {
        for (int i = 0; i < pokemon.Moves.Count; i++)
        {
            moveButtons[i].transform.Find("MoveName").GetComponent<TextMeshProUGUI>().text = pokemon.Moves[i].ScriptableMove.Name;
            moveButtons[i].transform.Find("Image").GetComponent<Image>().sprite = TypeUtils.TypeInfo(pokemon.Moves[i].ScriptableMove.Type).TypeIcon;
            moveButtons[i].transform.Find("PPText").GetComponent<TextMeshProUGUI>().text = "PP " + pokemon.Moves[i].CurrentPP + "/" + pokemon.Moves[i].ScriptableMove.PP;
            moveButtons[i].GetComponent<Image>().color = TypeUtils.TypeInfo(pokemon.Moves[i].ScriptableMove.Type).TypeColor;
        }

        newMoveButton.transform.Find("MoveName").GetComponent<TextMeshProUGUI>().text = newMove.Name;
        newMoveButton.transform.Find("Image").GetComponent<Image>().sprite = TypeUtils.TypeInfo(newMove.Type).TypeIcon;
        newMoveButton.transform.Find("PPText").GetComponent<TextMeshProUGUI>().text = "PP : " + newMove.PP;
        newMoveButton.GetComponent<Image>().color = TypeUtils.TypeInfo(newMove.Type).TypeColor;
    }
}
