using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private int textSpeed;

    private Coroutine currentDialogue;
    private Queue<string> messagesQueue;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        Instance = this;
        messagesQueue = new Queue<string>();
    }

    public void WriteDialogueText(TextMeshProUGUI TMPText, string text)
    {
        messagesQueue.Enqueue(text);
        
        if (currentDialogue == null)
            currentDialogue = StartCoroutine(WriteDialogueTextsCoroutine(TMPText));
    }
    
    public void WriteDialogueTexts(TextMeshProUGUI TMPText, List<string> texts)
    {
        foreach (string msg in texts)
        {
            messagesQueue.Enqueue(msg);
        }

        if (currentDialogue == null)
            currentDialogue = StartCoroutine(WriteDialogueTextsCoroutine(TMPText));
        
    }

    private IEnumerator WriteDialogueTextsCoroutine(TextMeshProUGUI TMPText)
    {
        IsBusy = true;
        while (messagesQueue.Count > 0)
        {
            string nextMessage = messagesQueue.Dequeue();
            TMPText.text = "";
            foreach (var letter in nextMessage.ToCharArray())
            {
                TMPText.text += letter;
                yield return new WaitForSeconds(1.0f / textSpeed);
            }

            yield return new WaitForSeconds(0.5f);
        }
        currentDialogue = null;
        IsBusy = false;
    }
}
