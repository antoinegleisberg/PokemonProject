using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuMovesDetailsScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _category;
    [SerializeField] private Image _type;
    [SerializeField] private TextMeshProUGUI _power;
    [SerializeField] private TextMeshProUGUI _accuracy;
    [SerializeField] private TextMeshProUGUI _description;

    private int _displayedMove = 0;

    public int DisplayedMove => _displayedMove;

    public void UpdateUI(Vector2Int input, List<Move> moves)
    {
        _displayedMove = (_displayedMove - input.y + moves.Count) % moves.Count;

        UpdateMoveUI(moves[_displayedMove]);
    }
    
    private void UpdateMoveUI(Move move)
    {
        ScriptableMove sMove = move.ScriptableMove;

        _name.text = sMove.Name;
        // _category.sprite = 
        _type.sprite = TypeUtils.TypeInfo(sMove.Type).TypeIcon;
        _power.text = sMove.Category == MoveCategory.Status ? "---" : sMove.Power.ToString();
        _accuracy.text = sMove.AlwaysHits ? "---" : sMove.Accuracy.ToString();
        _description.text = sMove.Description;
    }
}
