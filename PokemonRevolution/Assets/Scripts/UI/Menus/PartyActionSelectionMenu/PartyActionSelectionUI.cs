using System;
using UnityEngine;

public class PartyActionSelectionUI : MonoBehaviour
{
    [SerializeField] private UINavigator _partyActionSelectionMenu;

    private Action<int> _onSelected;
    private Action _onCancelled;

    public void SetCallbacks(Action<int> onSelected, Action onCancelled)
    {
        _onSelected = onSelected;
        _onCancelled = onCancelled;

        _partyActionSelectionMenu.OnSubmitted += _onSelected;
        _partyActionSelectionMenu.OnCancelled += _onCancelled;
    }

    private void OnDestroy()
    {
        _partyActionSelectionMenu.OnSubmitted -= _onSelected;
        _partyActionSelectionMenu.OnCancelled -= _onCancelled;
    }
}
