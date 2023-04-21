using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private int textSpeed;
    private Queue<string> messagesQueue;
    private TextMeshProUGUI currentTMPText;

    [SerializeField] private float timeBetweenAnimations = 0.25f;
    private Queue<IEnumerator> animationsQueue;

    public bool IsBusy
    {
        get { return isWriting || isRunningAnimations; }
    }
    private bool isWriting;
    private bool isRunningAnimations;

    private void Awake()
    {
        Instance = this;
        messagesQueue = new Queue<string>();
        animationsQueue = new Queue<IEnumerator>();
        StartCoroutine(DialogueManager());
        StartCoroutine(AnimationManager());
    }

    private void OnDestroy()
    {
        StopCoroutine("AnimationManager");
        StopCoroutine("DialogueManager");
    }

    public void WriteDialogueText(TextMeshProUGUI TMPText, string text)
    {
        currentTMPText = TMPText;
        messagesQueue.Enqueue(text);
    }
    
    public void WriteDialogueTexts(TextMeshProUGUI TMPText, List<string> texts)
    {
        currentTMPText = TMPText;
        foreach (string msg in texts)
        {
            messagesQueue.Enqueue(msg);
        }
        
    }

    private IEnumerator DialogueManager()
    {
        while (true)
        {
            while (messagesQueue.Count > 0)
            {
                isWriting = true;

                string nextMessage = messagesQueue.Dequeue();
                currentTMPText.text = "";
                foreach (var letter in nextMessage.ToCharArray())
                {
                    currentTMPText.text += letter;
                    yield return new WaitForSeconds(1.0f / textSpeed);
                }

                yield return new WaitForSeconds(0.5f);
            }

            isWriting = false;

            yield return null;
        }
    }

    public void EnqueueAnimation(IEnumerator animation)
    {
        animationsQueue.Enqueue(animation);
    }

    private IEnumerator AnimationManager()
    {
        while (true)
        {
            while (animationsQueue.Count > 0)
            {
                isRunningAnimations = true;
                
                yield return animationsQueue.Dequeue();
                
                yield return new WaitForSeconds(timeBetweenAnimations);
            }

            isRunningAnimations = false;

            yield return null;
        }
    }
}
