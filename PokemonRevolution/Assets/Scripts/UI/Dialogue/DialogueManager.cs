using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private int _textSpeed;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private Dialogue _currentDialogue;
    private int _currentLine;
    private bool _isTyping;
    private Action _onDialogueExited;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        Instance = this;
        _isTyping = false;
        IsBusy = false;
    }

    public void HandleUISubmit()
    {
        ShowNextLine();
    }

    public void HandleUICancel()
    {
        ShowNextLine();
    }

    public void ShowDialogue(Dialogue dialogue, Action onDialogueExited = null)
    {
        _onDialogueExited = onDialogueExited;
        _dialogueBox.SetActive(true);
        _currentDialogue = dialogue;
        _currentLine = 0;
        IsBusy = true;
        GameManager.Instance.SwitchState(GameManager.Instance.DialogueState);
        StartCoroutine(TypeDialogue(_currentDialogue.Lines[_currentLine]));
    }

    private void ShowNextLine()
    {
        if (_isTyping)
            return;

        if (_currentDialogue == null)
            return;

        _currentLine++;
        if (_currentLine < _currentDialogue.Lines.Count)
        {
            StartCoroutine(TypeDialogue(_currentDialogue.Lines[_currentLine]));
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        _dialogueBox.SetActive(false);
        IsBusy = false;
        _currentDialogue = null;
        GameManager.Instance.SwitchState(GameManager.Instance.FreeRoamState);
        _onDialogueExited?.Invoke();
    }

    private IEnumerator TypeDialogue(string msg)
    {
        _isTyping = true;
        _dialogueText.text = "";
        foreach (var letter in msg.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(1.0f / _textSpeed);
        }
        _isTyping = false;
    }
}
