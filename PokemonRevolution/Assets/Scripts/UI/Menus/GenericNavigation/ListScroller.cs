using UnityEngine;
using UnityEngine.UI;

public class ListScroller : MonoBehaviour
{
    [SerializeField] private UINavigator _navigator;
    [SerializeField] private UINavigationSelector _navigationSelector;

    [SerializeField] private RectTransform _itemsContainer;

    [SerializeField] private RectTransform _upArrow;
    [SerializeField] private RectTransform _downArrow;

    private int _nItems => _itemsContainer.transform.childCount;
    private float _childHeight => _itemsContainer.GetChild(0).GetComponent<RectTransform>().rect.height;
    private float _containerHeight =>
        _itemsContainer.GetComponent<VerticalLayoutGroup>().padding.top +
        _itemsContainer.GetComponent<VerticalLayoutGroup>().padding.bottom +
        _itemsContainer.GetComponent<RectTransform>().rect.height;
    private float _displayHeight => _itemsContainer.parent.GetComponent<RectTransform>().rect.height;

    private void OnEnable()
    {
        _navigator.OnNavigationInput += OnNavigated;

        UpdateNavigationArrows();
    }

    private void OnDisable()
    {
        _navigator.OnNavigationInput -= OnNavigated;
    }

    private void OnNavigated(Vector2Int input)
    {
        HandleScrolling(input);
        UpdateNavigationArrows();
    }

    private void HandleScrolling(Vector2Int input)
    {
        if (_nItems == 0)
        {
            return;
        }
        if (_displayHeight >= _nItems * _childHeight)
        {
            return;
        }

        int numberOfItemsBeforeScrolling = Mathf.RoundToInt(_displayHeight / _childHeight / 2);

        float minYOffset = 0;
        float maxYOffset = _containerHeight - _displayHeight;

        float containerTargetY = _itemsContainer.localPosition.y - input.y * _childHeight;
        containerTargetY = Mathf.Clamp(containerTargetY, minYOffset, maxYOffset);
        Vector2 containerTargetPosition = new Vector2(_itemsContainer.localPosition.x, containerTargetY);

        if (_navigationSelector.CurrentSelection < numberOfItemsBeforeScrolling)
        {
            // Reached the top
            containerTargetPosition.y = minYOffset;
        }

        if (_nItems - _navigationSelector.CurrentSelection < numberOfItemsBeforeScrolling)
        {
            // Reached the bottom
            containerTargetPosition.y = maxYOffset;
        }

        _itemsContainer.localPosition = containerTargetPosition;
    }

    private void UpdateNavigationArrows()
    {
        if (_nItems == 0)
        {
            _upArrow.gameObject.SetActive(false);
            _downArrow.gameObject.SetActive(false);
            return;
        }

        float minYOffset = 0;
        float maxYOffset = _containerHeight - _displayHeight;

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
