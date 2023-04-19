using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Current;

    [SerializeField] private int textSpeed;

    private Coroutine currentDialogue;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        Current = this;
    }

    public void WriteDialogueText(TextMeshProUGUI TMPText, string text)
    {
        if (currentDialogue != null) StopCoroutine(currentDialogue);
        currentDialogue = StartCoroutine(WriteDialogueTextsCoroutine(TMPText, new List<string>() { text } ));
    }
    
    public void WriteDialogueTexts(TextMeshProUGUI TMPText, List<string> texts)
    {
        if (currentDialogue != null) StopCoroutine(currentDialogue);
        currentDialogue = StartCoroutine(WriteDialogueTextsCoroutine(TMPText, texts));
    }

    private IEnumerator WriteDialogueTextsCoroutine(TextMeshProUGUI TMPText, List<string> texts)
    {
        IsBusy = true;
        foreach (string msg in texts)
        {
            yield return StartCoroutine(WriteDialogueTextCoroutine(TMPText, msg));
            yield return new WaitForSeconds(1.0f);
        }
        IsBusy = false;
    }

    private IEnumerator WriteDialogueTextCoroutine(TextMeshProUGUI TMPText, string text)
    {
        TMPText.text = "";
        foreach (var letter in text.ToCharArray())
        {
            TMPText.text += letter;
            yield return new WaitForSeconds(1.0f / textSpeed);
        }
    }
}
