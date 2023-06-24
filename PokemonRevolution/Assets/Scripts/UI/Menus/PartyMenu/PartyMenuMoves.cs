using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuMoves : MonoBehaviour
{
    [SerializeField] private List<Image> _moveImages;
    private List<TextMeshProUGUI> _moveNames;
    private List<Image> _moveTypeIcons;
    private List<TextMeshProUGUI> _movePPTexts;

    private PokemonParty _playerParty;

    private PokemonParty PlayerParty {
        get
        {
            if (_playerParty == null)
            {
                _playerParty = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty;
            }
            return _playerParty;
        }
    }

    public void UpdateMoves(int pokemonIdx)
    {
        if (_moveNames == null)
        {
            InitMoveLists();
        }

        List<Move> moves = PlayerParty.Pokemons[pokemonIdx].Moves;

        for (int i = 0; i < _moveImages.Count; i++)
        {
            if (i >= PlayerParty.Pokemons[pokemonIdx].Moves.Count)
            {
                _moveImages[i].color = new Color(1, 1, 1, 0);
                _moveNames[i].text = "";
                _movePPTexts[i].text = "";
                _moveTypeIcons[i].sprite = null;
                continue;
            }
            
            Move move = moves[i];
            _moveImages[i].color = TypeUtils.TypeInfo(move.ScriptableMove.Type).TypeColor;
            _moveNames[i].text = move.ScriptableMove.Name;
            _movePPTexts[i].text = $"PP {move.CurrentPP} / {move.ScriptableMove.PP}";
            _moveTypeIcons[i].sprite = TypeUtils.TypeInfo(move.ScriptableMove.Type).TypeIcon;
        }
    }
    
    private void InitMoveLists()
    {
        _moveNames = new List<TextMeshProUGUI>();
        _movePPTexts = new List<TextMeshProUGUI>();
        _moveTypeIcons = new List<Image>();
        foreach (Image moveImage in _moveImages)
        {
            Transform imgTransform = moveImage.transform;
            _moveNames.Add(imgTransform.Find("Name").GetComponent<TextMeshProUGUI>());
            _movePPTexts.Add(imgTransform.Find("PP").GetComponent<TextMeshProUGUI>());
            _moveTypeIcons.Add(imgTransform.Find("Type").GetComponent<Image>());
        }
    }
}
