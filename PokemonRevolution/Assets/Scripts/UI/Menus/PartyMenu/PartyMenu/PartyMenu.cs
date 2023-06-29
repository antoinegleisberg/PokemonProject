using System;
using UnityEngine;

public class PartyMenu : MonoBehaviour
{
    [SerializeField] private UINavigator _partyNavigator;
    [SerializeField] private UINavigationSelector _partyNavigationSelector;

    [SerializeField] private PartyMenuUI _partyMenuUI;
    [SerializeField] private PartyMenuPokemonOverview _partyMenuOverview;

    [SerializeField] private PartyMenuManager _partyMenuManager;


    private Action<int> _onSelectedOverride;
    private Action _onCancelledOverride;
    
    
    private void OnEnable()
    {
        _partyNavigationSelector.OnSelectionChanged += OnSelectionChanged;
        _partyNavigationSelector.OnSubmitted += OnSubmitted;
        _partyNavigator.OnCancelled += OnCancelled;

        _partyMenuOverview.gameObject.SetActive(true);
        _partyMenuOverview.UpdateUI(_partyNavigationSelector.CurrentSelection);
        _partyMenuOverview.ShowPokemonNavigationArrows(false);
    }

    private void OnDisable()
    {
        _partyNavigationSelector.OnSelectionChanged -= OnSelectionChanged;
        _partyNavigationSelector.OnSubmitted -= OnSubmitted;
        _partyNavigator.OnCancelled -= OnCancelled;

        _partyMenuOverview.gameObject.SetActive(false);
    }

    public void OverrideCallbacks(Action<int> onSelected, Action onCancelled)
    {
        _onSelectedOverride = onSelected;
        _onCancelledOverride = onCancelled;
    }

    private void OnSelectionChanged(int oldSelection, int newSelection)
    {
        _partyMenuOverview.UpdateUI(newSelection);
    }

    private void OnSubmitted(int selection)
    {
        if (_onSelectedOverride != null)
        {
            _onSelectedOverride.Invoke(selection);
            _partyMenuUI.UpdateUI(selection);
        }
        else
        {
            _partyMenuManager.OpenPokemonScreen(selection);
        }
    }

    private void OnCancelled()
    {
        if (_onCancelledOverride != null)
        {
            _onCancelledOverride.Invoke();
        }
        else
        {
            UIManager.Instance.OpenPauseMenu();
        }
    }
}
