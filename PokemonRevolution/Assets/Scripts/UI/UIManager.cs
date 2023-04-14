using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Current;

    [SerializeField] private int textSpeed = 30;

    private void Awake()
    {
        Current = this;
    }

    public void WriteDialogueText(TextMeshProUGUI TMPText, string text)
    {
        StartCoroutine(WriteDialogueTextCoroutine(TMPText, text));
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
