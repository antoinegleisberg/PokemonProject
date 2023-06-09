using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectorNavigationManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;


    [SerializeField] private List<Pair<Button, Button>> overrideNavigationOrderRight;
    [SerializeField] private List<Pair<Button, Button>> overrideNavigationOrderLeft;
    [SerializeField] private List<Pair<Button, Button>> overrideNavigationOrderUp;
    [SerializeField] private List<Pair<Button, Button>> overrideNavigationOrderDown;

    private Dictionary<Button, Button> overrideNavigationOrderRightMapping;
    private Dictionary<Button, Button> overrideNavigationOrderLeftMapping;
    private Dictionary<Button, Button> overrideNavigationOrderUpMapping;
    private Dictionary<Button, Button> overrideNavigationOrderDownMapping;

    [SerializeField] private bool canCancel;

    private int currentSelection;
    private RectTransform selectionIndicator;

    private void Awake()
    {
        currentSelection = -1;

        overrideNavigationOrderRightMapping = BuildNavigationMapping(overrideNavigationOrderRight);
        overrideNavigationOrderLeftMapping = BuildNavigationMapping(overrideNavigationOrderLeft);
        overrideNavigationOrderUpMapping = BuildNavigationMapping(overrideNavigationOrderUp);
        overrideNavigationOrderDownMapping = BuildNavigationMapping(overrideNavigationOrderDown);
    }

    private void OnEnable()
    {
        if (currentSelection != -1)
        {
            selectionIndicator.gameObject.SetActive(true);
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        selectionIndicator.gameObject.SetActive(false);
    }

    public void SetSelectionIndicator(RectTransform selectionIndicator)
    {
        this.selectionIndicator = selectionIndicator;
    }

    public void HandleUINavigation(Vector2Int input)
    {
        if (currentSelection == -1)
        {
            UpdateSelection(0);
            return;
        }

        Dictionary<Button, Button> activeOverriding = GetActiveOverriding(input);

        if (activeOverriding != null && activeOverriding.ContainsKey(buttons[currentSelection]))
        {
            int newSelection = buttons.IndexOf(activeOverriding[buttons[currentSelection]]);
            UpdateSelection(newSelection);
            return;
        }

        int offset = InputToOffset(input);
        UpdateSelection(currentSelection + offset);
    }

    public void HandleUISubmit()
    {
        if (currentSelection == -1)
        {
            UpdateSelection(0);
        }
        else if (buttons[currentSelection].interactable)
        {
            buttons[currentSelection].onClick.Invoke();
        }
    }

    public void HandleUICancel()
    {
        if (canCancel)
        {
            buttons[buttons.Count - 1].onClick.Invoke();
        }
    }

    private void UpdateSelection(int newSelection)
    {
        newSelection = Mathf.Clamp(newSelection, 0, buttons.Count - 1);

        bool selectionIncreased = Mathf.Sign(newSelection - currentSelection) > 0;
        int offset = selectionIncreased ? 1 : -1;
        while (0 <= newSelection && newSelection < buttons.Count && !buttons[newSelection].isActiveAndEnabled)
        {
            newSelection += offset;
        }
        if (newSelection < 0 || newSelection >= buttons.Count)
        {
            offset = -offset;
            while (0 <= newSelection && newSelection < buttons.Count && !buttons[newSelection].isActiveAndEnabled)
            {
                newSelection += offset;
            }
        }

        currentSelection = newSelection;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!selectionIndicator.gameObject.activeSelf)
        {
            selectionIndicator.gameObject.SetActive(true);
        }
        RectTransform currentButtonTranform = buttons[currentSelection].GetComponent<RectTransform>();
        selectionIndicator.position = currentButtonTranform.position;
        selectionIndicator.sizeDelta = currentButtonTranform.sizeDelta + new Vector2(15, 15);
    }

    private Dictionary<Button, Button> BuildNavigationMapping(List<Pair<Button, Button>> pairs)
    {
        Dictionary<Button, Button> dict = new Dictionary<Button, Button>();
        foreach (Pair<Button, Button> pair in pairs)
        {
            dict.Add(pair.first, pair.second);
        }
        return dict;
    }

    private int InputToOffset(Vector2Int input)
    {
        switch (input)
        {
            case Vector2Int value when value == Vector2Int.left:
                return -1;
            case Vector2Int value when value == Vector2Int.right:
                return 1;
            case Vector2Int value when value == Vector2Int.up:
                return -2;
            case Vector2Int value when value == Vector2Int.down:
                return 2;
            default:
                return 0;
        }
    }

    private Dictionary<Button, Button> GetActiveOverriding(Vector2Int input)
    {
        switch (input)
        {
            case Vector2Int value when value == Vector2Int.left:
                return overrideNavigationOrderLeftMapping;
            case Vector2Int value when value == Vector2Int.right:
                return overrideNavigationOrderRightMapping;
            case Vector2Int value when value == Vector2Int.up:
                return overrideNavigationOrderUpMapping;
            case Vector2Int value when value == Vector2Int.down:
                return overrideNavigationOrderDownMapping;
            default:
                return null;
        }
    }
}