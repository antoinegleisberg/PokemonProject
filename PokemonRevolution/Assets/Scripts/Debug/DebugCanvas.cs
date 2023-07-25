using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    public static DebugCanvas Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Clear()
    {
        bool first = true;
        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (first)
            {
                first = false;
                continue;
            }
            Destroy(text.gameObject);
        }
        GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    public void WriteText(string text)
    {
        TextMeshProUGUI textContainer = GetComponentInChildren<TextMeshProUGUI>();
        if (textContainer.text == "")
        {
            textContainer.text = text;
        }
        else
        {
            TextMeshProUGUI newText = Instantiate(textContainer, textContainer.transform.parent);
            newText.text = text;
        }
    }
}
