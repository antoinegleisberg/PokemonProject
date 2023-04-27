using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private int textSpeed;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Dialogue currentDialogue;
    private int currentLine;
    private bool isTyping;

    public bool IsBusy { get; private set; }
    
    public void ShowDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        currentDialogue = dialogue;
        currentLine = 0;
        IsBusy = true;
        GameEvents.Instance.EnterDialogue();
        StartCoroutine(TypeDialogue(currentDialogue.Lines[currentLine]));
    }

    private IEnumerator TypeDialogue(string msg)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (var letter in msg.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1.0f / textSpeed);
        }
        isTyping = false;
    }   

    private void ShowNextLine()
    {
        if (isTyping)
            return;

        currentLine++;
        if (currentLine < currentDialogue.Lines.Count)
            StartCoroutine(TypeDialogue(currentDialogue.Lines[currentLine]));
        else
        {
            dialogueBox.SetActive(false);
            IsBusy = false;
            GameEvents.Instance.ExitDialogue();
            GameEvents.Instance.AfterDialogueExited();
        }
    }

    private void Awake()
    {
        Instance = this;
        isTyping = false;
        IsBusy = false;
    }

    private void Start()
    {
        InputEvents.Instance.OnUISubmit += ShowNextLine;
        InputEvents.Instance.OnUICancel += ShowNextLine;
    }

    private void OnDestroy()
    {
        InputEvents.Instance.OnUISubmit -= ShowNextLine;
        InputEvents.Instance.OnUICancel -= ShowNextLine;
    }
}
