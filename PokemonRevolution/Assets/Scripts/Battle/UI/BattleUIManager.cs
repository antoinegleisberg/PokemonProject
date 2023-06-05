using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

    [SerializeField] private int textSpeed;
    private Queue<string> messagesQueue;
    
    [SerializeField] private TextMeshProUGUI battleSystemDialogue;

    [SerializeField] private float timeBetweenAnimations = 0.25f;
    private Queue<IEnumerator> animationsQueue;

    public bool IsBusy
    {
        get { return isWriting || isRunningAnimations || isPaused; }
    }
    private bool isWriting;
    private bool isRunningAnimations;

    public bool IsPaused { get => isPaused; }
    private bool isPaused;


    public IEnumerator WaitWhileBusy()
    {
        yield return new WaitForEndOfFrame();
        while (IsBusy)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void WriteDialogueText(string text)
    {
        messagesQueue.Enqueue(text);
    }
    
    public void WriteDialogueTexts(List<string> texts)
    {
        foreach (string msg in texts)
        {
            messagesQueue.Enqueue(msg);
        }
    }
    
    public void EnqueueAnimation(IEnumerator animation)
    {
        animationsQueue.Enqueue(animation);
    }

    private IEnumerator DialogueManager()
    {
        while (true)
        {
            while (messagesQueue.Count > 0)
            {
                isWriting = true;

                string nextMessage = messagesQueue.Dequeue();
                battleSystemDialogue.text = "";
                foreach (var letter in nextMessage.ToCharArray())
                {
                    battleSystemDialogue.text += letter;
                    yield return new WaitForSeconds(1.0f / textSpeed);
                }

                yield return new WaitForSeconds(0.5f);

                // yield return new WaitUntil(() => !isPaused);
            }

            isWriting = false;

            yield return null;
        }
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

                // yield return new WaitUntil(() => !isPaused);
            }

            isRunningAnimations = false;

            yield return null;
        }
    }

    private void Pause()
    {
        Debug.Log("Pausing BattleUIManager");
        isPaused = true;
    }
    
    private void Unpause()
    {
        Debug.Log("Unpausing BattleUIManager");
        isPaused = false;
    }

    private void Awake()
    {
        Instance = this;
        isPaused = false;
        messagesQueue = new Queue<string>();
        animationsQueue = new Queue<IEnumerator>();
        StartCoroutine(DialogueManager());
        StartCoroutine(AnimationManager());
    }

    private void Start()
    {
        BattleEvents.Instance.OnChooseMoveToForget += (Pokemon _, ScriptableMove _) => Pause();
        BattleUIEvents.Instance.OnSelectMoveToForget += (int _) => Unpause();
    }

    private void OnDestroy()
    {
        StopCoroutine("AnimationManager");
        StopCoroutine("DialogueManager");
        
        BattleEvents.Instance.OnChooseMoveToForget -= (Pokemon _, ScriptableMove _) => Pause();
        BattleUIEvents.Instance.OnSelectMoveToForget -= (int _) => Unpause();
    }
}
