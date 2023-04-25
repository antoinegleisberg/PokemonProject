using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue dialogue;

    public void Interact()
    {
        DialogueManager.Instance.ShowDialogue(dialogue);
    }
}
