using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgetMoveSelectorUIManager : MonoBehaviour
{
    [SerializeField] private List<Button> _moveButtons;
    [SerializeField] private Button _newMoveButton;

    private List<TextMeshProUGUI> _moveTexts;
    private List<Image> _typeIcons;
    private List<TextMeshProUGUI> _ppTexts;
    private List<Image> _buttonImages;

    private TextMeshProUGUI _newMoveText;
    private Image _newMoveTypeIcon;
    private TextMeshProUGUI _newMovePPText;
    private Image _newMoveButtonImage;

    private void Awake()
    {
        _moveTexts = new List<TextMeshProUGUI>();
        _typeIcons = new List<Image>();
        _ppTexts = new List<TextMeshProUGUI>();
        _buttonImages = new List<Image>();

        foreach (Button button in _moveButtons)
        {
            _moveTexts.Add(button.transform.Find("MoveName").GetComponent<TextMeshProUGUI>());
            _typeIcons.Add(button.transform.Find("Image").GetComponent<Image>());
            _ppTexts.Add(button.transform.Find("PPText").GetComponent<TextMeshProUGUI>());
            _buttonImages.Add(button.GetComponent<Image>());
        }

        _newMoveText = _newMoveButton.transform.Find("MoveName").GetComponent<TextMeshProUGUI>();
        _newMoveTypeIcon = _newMoveButton.transform.Find("Image").GetComponent<Image>();
        _newMovePPText = _newMoveButton.transform.Find("PPText").GetComponent<TextMeshProUGUI>();
        _newMoveButtonImage = _newMoveButton.GetComponent<Image>();
    }

    public void UpdateUI(Pokemon pokemon, ScriptableMove newMove)
    {
        for (int i = 0; i < pokemon.Moves.Count; i++)
        {
            _moveTexts[i].text = pokemon.Moves[i].ScriptableMove.Name;
            _typeIcons[i].sprite = TypeUtils.TypeInfo(pokemon.Moves[i].ScriptableMove.Type).TypeIcon;
            _ppTexts[i].text = "PP " + pokemon.Moves[i].CurrentPP + "/" + pokemon.Moves[i].ScriptableMove.PP;
            _buttonImages[i].color = TypeUtils.TypeInfo(pokemon.Moves[i].ScriptableMove.Type).TypeColor;
        }

        _newMoveText.text = newMove.Name;
        _newMoveTypeIcon.sprite = TypeUtils.TypeInfo(newMove.Type).TypeIcon;
        _newMovePPText.text = "PP : " + newMove.PP;
        _newMoveButtonImage.color = TypeUtils.TypeInfo(newMove.Type).TypeColor;
    }
}
