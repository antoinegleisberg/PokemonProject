using System;
using System.Collections.Generic;
using UnityEngine;

public class BagMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _bagMenuNavigator;
    [SerializeField] private UINavigationSelector _bagItemSelector;
    [SerializeField] private UINavigationSelector _bagCategorySelector;
    
    [SerializeField] private BagItem _itemUIPrefab;
    [SerializeField] private RectTransform _itemsContainer;
    [SerializeField] private BagItemDescriptionUI _bagItemDescriptionUI;

    private Action<BagCategory, int> _onItemSelectedOverride;
    private Action _onCancelledOverride;

    private BagCategory _currentCategory => (BagCategory)_bagCategorySelector.CurrentSelection;

    private Inventory _playerInventory { get => GameManager.Instance.PlayerController.Inventory; }

    private void OnEnable()
    {
        UpdateItemSlots();
        _bagItemSelector.UpdateUI();
        UpdateDescription();
    }

    private void Start()
    {
        _bagMenuNavigator.OnCancelled += OnCancelled;
        _bagItemSelector.OnSelectionChanged += OnNavigated;
        _bagItemSelector.OnSubmitted += OnSelected;
        _bagCategorySelector.OnSelectionChanged += OnCategoryChanged;
    }

    private void OnDestroy()
    {
        _bagMenuNavigator.OnCancelled -= OnCancelled;
        _bagItemSelector.OnSelectionChanged -= OnNavigated;
        _bagItemSelector.OnSubmitted -= OnSelected;
        _bagCategorySelector.OnSelectionChanged -= OnCategoryChanged;
    }

    public void OverrideCallbacks(Action<BagCategory, int> onSubmitted, Action onCancelled)
    {
        _onItemSelectedOverride = onSubmitted;
        _onCancelledOverride = onCancelled;
    }

    private void OnNavigated(int oldSelection, int newSelection)
    {
        UpdateDescription();
    }

    private void OnSelected(int itemIdx)
    {
        if (_onItemSelectedOverride != null)
        {
            _onItemSelectedOverride.Invoke((BagCategory)_bagCategorySelector.CurrentSelection, itemIdx);
            return;
        }

        Action<int> onSelected = (int pokemonIdx) =>
        {
            Pokemon targetPokemon = GameManager.Instance.PlayerController.PokemonPartyManager.PokemonParty.Pokemons[pokemonIdx];
            _playerInventory.UseItem(_currentCategory, itemIdx, targetPokemon);
        };

        Action onCancelled = () =>
        {
            UIManager.Instance.OpenBagMenu();
        };

        UIManager.Instance.OpenPartyMenu(onSelected, onCancelled);
    }

    private void OnCancelled()
    {
        if (_onCancelledOverride != null)
        {
            _onCancelledOverride.Invoke();
            return;
        }

        UIManager.Instance.OpenPauseMenu();
    }

    private void OnCategoryChanged(int oldCategory, int newCategory)
    {
        UpdateItemSlots();
        _bagItemSelector.UpdateUI();
        UpdateDescription();
    }

    private void UpdateDescription()
    {
        List<ItemSlot> slots = _playerInventory.GetSlots(_currentCategory);
        ItemBase selectedItem = null;
        if (slots.Count > 0)
        {
            selectedItem = slots[_bagItemSelector.CurrentSelection].Item;
        }
        _bagItemDescriptionUI.UpdateUI(selectedItem);
    }
    
    private void UpdateItemSlots()
    {
        foreach (BagItem child in _itemsContainer.GetComponentsInChildren<BagItem>())
        {
            Destroy(child.gameObject);
        }
        _bagItemSelector.NavigationItems.Clear();

        foreach (ItemSlot itemSlot in _playerInventory.GetSlots(_currentCategory))
        {
            BagItem itemUI = Instantiate(_itemUIPrefab, _itemsContainer);
            itemUI.UpdateUI(itemSlot);
            _bagItemSelector.NavigationItems.Add(itemUI.GetComponent<NavigationItem>());
        }
    }
}
