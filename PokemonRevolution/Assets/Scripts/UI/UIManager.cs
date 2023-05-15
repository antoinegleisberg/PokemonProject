using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public void HandleUINavigation(Vector2Int input)
    {

    }

    public void HandleUISubmit()
    {
        DialogueManager.Instance.ShowNextLine();
    }

    public void HandleUICancel()
    {
        DialogueManager.Instance.ShowNextLine();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
