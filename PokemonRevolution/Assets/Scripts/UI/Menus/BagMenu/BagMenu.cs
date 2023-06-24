using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class BagMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _bagMenuNavigator;
    [SerializeField] private BagItem _itemUIPrefab;
    [SerializeField] private RectTransform _itemsContainer;
    [SerializeField] private BagItemDescriptionUI _bagItemDescriptionUI;
    [SerializeField] private RectTransform _upArrow;
    [SerializeField] private RectTransform _downArrow;

    private int _numberOfItems { get => _playerInventory.Slots.Count; }

    private Inventory _playerInventory { get => GameManager.Instance.PlayerController.Inventory; }

    private void Awake()
    {
        _bagMenuNavigator.OnStarted += UpdateDescription;
    }

    private void Start()
    {
        _bagMenuNavigator.OnCancelled += UIManager.Instance.OpenPauseMenu;
        _bagMenuNavigator.OnNavigated += OnNavigated;
        _bagMenuNavigator.OnNavigationInput += OnNavigated;
        _bagMenuNavigator.OnSubmitted += OnSelected;
    }

    private void OnDestroy()
    {
        _bagMenuNavigator.OnCancelled -= UIManager.Instance.OpenPauseMenu;
        _bagMenuNavigator.OnNavigated -= OnNavigated;
        _bagMenuNavigator.OnNavigationInput -= OnNavigated;
        _bagMenuNavigator.OnStarted -= UpdateDescription;
        _bagMenuNavigator.OnSubmitted -= OnSelected;
    }

    private void OnEnable()
    {
        UpdateItemSlots();
        UpdateNavigationArrows();
    }

    private void OnNavigated(Vector2Int input)
    {
        HandleScrolling(input);
        UpdateNavigationArrows();
    }

    private void OnNavigated(int selection)
    {
        UpdateDescription();
    }

    private void OnSelected(int itemIdx)
    {
        Action<int> onSelected = (int pokemonIdx) =>
        {
            Pokemon targetPokemon = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[pokemonIdx];
            _playerInventory.UseItem(itemIdx, targetPokemon);
        };

        Action onCancelled = () =>
        {
            UIManager.Instance.OpenBagMenu();
        };

        UIManager.Instance.OpenPartyMenuActionSelector(onSelected, onCancelled);
    }

    private void HandleScrolling(Vector2Int input)
    {
        const int minInventoryItemsBeforeScroll = 6;
        const int numberOfItemsBeforeScrolling = 5;
        if (_playerInventory.Slots.Count < minInventoryItemsBeforeScroll)
        {
            return;
        }
        if (_bagMenuNavigator.CurrentSelection < numberOfItemsBeforeScrolling ||
            _numberOfItems - _bagMenuNavigator.CurrentSelection < numberOfItemsBeforeScrolling)
        {
            return;
        }

        float itemSlotUIHeight = _itemsContainer.GetComponentInChildren<BagItem>().GetComponent<RectTransform>().rect.height;

        float containerHeight =
            _itemsContainer.GetComponent<VerticalLayoutGroup>().padding.top +
            _itemsContainer.GetComponent<VerticalLayoutGroup>().padding.bottom +
            _numberOfItems * itemSlotUIHeight;

        float displayHeight = _itemsContainer.parent.GetComponent<RectTransform>().rect.height;

        float minYOffset = 0;
        float maxYOffset = containerHeight - displayHeight;
        
        float containerTargetY = _itemsContainer.localPosition.y + input.x * itemSlotUIHeight;
        containerTargetY = Mathf.Clamp(containerTargetY, minYOffset, maxYOffset);
        Vector2 containerTargetPosition = new Vector2(_itemsContainer.localPosition.x, containerTargetY);
        _itemsContainer.localPosition = containerTargetPosition;

        _bagMenuNavigator.UpdateUI();
    }

    private void UpdateDescription()
    {
        ItemBase selectedItem = _playerInventory.Slots[_bagMenuNavigator.CurrentSelection].Item;
        _bagItemDescriptionUI.UpdateUI(selectedItem);
    }
    
    private void UpdateItemSlots()
    {
        foreach (BagItem child in _itemsContainer.GetComponentsInChildren<BagItem>())
        {
            Destroy(child.gameObject);
        }
        _bagMenuNavigator.NavigationItems.Clear();

        foreach (ItemSlot itemSlot in _playerInventory.Slots)
        {
            BagItem itemUI = Instantiate(_itemUIPrefab, _itemsContainer);
            itemUI.UpdateUI(itemSlot);
            _bagMenuNavigator.NavigationItems.Add(itemUI.GetComponent<NavigationItem>());
        }
    }

    private void UpdateNavigationArrows()
    {
        float itemSlotUIHeight = _itemsContainer.GetComponentInChildren<BagItem>().GetComponent<RectTransform>().rect.height;

        float containerHeight =
            _itemsContainer.GetComponent<VerticalLayoutGroup>().padding.top +
            _itemsContainer.GetComponent<VerticalLayoutGroup>().padding.bottom +
            _numberOfItems * itemSlotUIHeight;

        float displayHeight = _itemsContainer.parent.GetComponent<RectTransform>().rect.height;

        float minYOffset = 0;
        float maxYOffset = containerHeight - displayHeight;

        float containerYPosition = _itemsContainer.localPosition.y;

        _upArrow.gameObject.SetActive(true);
        _downArrow.gameObject.SetActive(true);
        if (containerYPosition < minYOffset + 1)
        {
            _upArrow.gameObject.SetActive(false);
        }
        if (containerYPosition > maxYOffset - 1)
        {
            _downArrow.gameObject.SetActive(false);
        }
    }
}
