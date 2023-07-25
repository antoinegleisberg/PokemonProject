using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMoveSelector : MonoBehaviour
{
    [SerializeField] private UINavigator _navigator;
    [SerializeField] private UINavigationSelector _selector;
    [SerializeField] private RectTransform _selectionIndicator;

    [SerializeField] private Color _outOfPPColor;

    [SerializeField] private List<Image> _moves;
    private List<TextMeshProUGUI> _moveNames;
    private List<Image> _moveTypeImages;
    private List<TextMeshProUGUI> _movePPTexts;
    
    [SerializeField] private BattleManager _battleManager;


    private void OnEnable()
    {
        _selectionIndicator.gameObject.SetActive(true);
        UpdateMovesUI(_battleManager.PlayerPokemon);
        UpdateNavigationArrow();

        _selector.OnSelectionChanged += OnSelectionChanged;
        _selector.OnSubmitted += OnSubmitted;
        _navigator.OnCancelled += _battleManager.BattleActionSelectorsUIManager.SetActiveSelectorToActionSelector;
    }

    private void OnDisable()
    {
        _selectionIndicator.gameObject.SetActive(false);

        _selector.OnSelectionChanged -= OnSelectionChanged;
        _selector.OnSubmitted -= OnSubmitted;
        _navigator.OnCancelled -= _battleManager.BattleActionSelectorsUIManager.SetActiveSelectorToActionSelector;
    }

    public void UpdateMovesUI(Pokemon playerPokemon)
    {
        if (playerPokemon.Owner != PokemonOwner.Player)
        {
            return;
        }

        if (_moveNames == null)
        {
            InitButtonsList();
        }

        for (int i = 0; i < playerPokemon.Moves.Count; i++)
        {
            _moves[i].gameObject.SetActive(true);
            _moveNames[i].text = playerPokemon.Moves[i].ScriptableMove.Name;
            _moveTypeImages[i].sprite = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeIcon;
            _movePPTexts[i].text = "PP " + playerPokemon.Moves[i].CurrentPP + "/" + playerPokemon.Moves[i].ScriptableMove.PP;

            if (playerPokemon.Moves[i].CurrentPP == 0)
            {
                _moves[i].color = _outOfPPColor;
            }
            else
            {
                _moves[i].color = TypeUtils.TypeInfo(playerPokemon.Moves[i].ScriptableMove.Type).TypeColor;
            }
        }
        for (int i = playerPokemon.Moves.Count; i < Pokemon.MaxNumberMoves; i++)
        {
            _moves[i].gameObject.SetActive(false);
        }
    }
    
    private void OnSelectionChanged(int oldSelection, int newSelection)
    {
        UpdateNavigationArrow();
    }

    private void UpdateNavigationArrow()
    {
        float targetX = _selector.NavigationItems[_selector.CurrentSelection].GetComponent<RectTransform>().rect.x;
        float targetY = _selector.NavigationItems[_selector.CurrentSelection].GetComponent<RectTransform>().rect.center.y;

        Vector3 targetPos = _selector.NavigationItems[_selector.CurrentSelection].GetComponent<RectTransform>().TransformPoint(new Vector3(targetX, targetY, 0));

        _selectionIndicator.position = new Vector3(targetPos.x, targetPos.y, 0);
        _selectionIndicator.localPosition = new Vector3(_selectionIndicator.localPosition.x, _selectionIndicator.localPosition.y, 0);
    }

    private void OnSubmitted(int selection)
    {
        BattleUIEvents.Instance.MoveSelected(selection);
    }

    private void InitButtonsList()
    {
        _moveNames = new List<TextMeshProUGUI>();
        _moveTypeImages = new List<Image>();
        _movePPTexts = new List<TextMeshProUGUI>();
        foreach (Image image in _moves)
        {
            _moveNames.Add(image.transform.Find("MoveName").GetComponent<TextMeshProUGUI>());
            _moveTypeImages.Add(image.transform.Find("Image").GetComponent<Image>());
            _movePPTexts.Add(image.transform.Find("PPText").GetComponent<TextMeshProUGUI>());
        }
    }
}
