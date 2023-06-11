using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance { get; private set; }

    [SerializeField] private int _textSpeed;
    private Queue<string> _messagesQueue;
    
    [SerializeField] private TextMeshProUGUI _battleSystemDialogue;

    [SerializeField] private float _timeBetweenAnimations = 0.25f;
    private Queue<IEnumerator> _animationsQueue;

    private bool _isWriting;
    private bool _isRunningAnimations;
    private bool _isPaused;

    public bool IsBusy
    {
        get { return _isWriting || _isRunningAnimations || _isPaused; }
    }

    public bool IsPaused { get => _isPaused; }
    

    private void Awake()
    {
        Instance = this;
        _isPaused = false;
        _messagesQueue = new Queue<string>();
        _animationsQueue = new Queue<IEnumerator>();
        StartCoroutine(DialogueManager());
        StartCoroutine(AnimationManager());
    }

    private void OnDestroy()
    {
        StopCoroutine("AnimationManager");
        StopCoroutine("DialogueManager");
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Unpause()
    {
        _isPaused = false;
    }

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
        _messagesQueue.Enqueue(text);
    }
    
    public void WriteDialogueTexts(List<string> texts)
    {
        foreach (string msg in texts)
        {
            _messagesQueue.Enqueue(msg);
        }
    }
    
    public void EnqueueAnimation(IEnumerator animation)
    {
        _animationsQueue.Enqueue(animation);
    }

    private IEnumerator DialogueManager()
    {
        while (true)
        {
            while (_messagesQueue.Count > 0)
            {
                _isWriting = true;

                string nextMessage = _messagesQueue.Dequeue();
                _battleSystemDialogue.text = "";
                foreach (var letter in nextMessage.ToCharArray())
                {
                    _battleSystemDialogue.text += letter;
                    yield return new WaitForSeconds(1.0f / _textSpeed);
                }

                yield return new WaitForSeconds(0.5f);

                // yield return new WaitUntil(() => !isPaused);
            }

            _isWriting = false;

            yield return null;
        }
    }

    private IEnumerator AnimationManager()
    {
        while (true)
        {
            while (_animationsQueue.Count > 0)
            {
                _isRunningAnimations = true;
                
                yield return _animationsQueue.Dequeue();
                
                yield return new WaitForSeconds(_timeBetweenAnimations);

                // yield return new WaitUntil(() => !isPaused);
            }

            _isRunningAnimations = false;

            yield return null;
        }
    }
}
