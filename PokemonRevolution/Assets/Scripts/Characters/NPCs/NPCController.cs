using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Character character;
    [SerializeField] private List<NPCMovementPattern> movementPatterns;

    private bool isInDialogue;
    private Coroutine moveCoroutine;

    public void Interact(Transform source)
    {
        if (character.IsMoving)
            return;

        isInDialogue = true;
        Vector3 faceTowards = source.position - transform.position;
        character.FaceTowards(DirectionUtils.GetDirection(faceTowards));
        // make the player face the NPC too
        source.GetComponentInChildren<Character>()?.FaceTowards(DirectionUtils.GetDirection(-faceTowards));
        DialogueManager.Instance.ShowDialogue(dialogue);
    }

    public void StopMoving()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
    }

    private IEnumerator Move()
    {
        int currentPatternIdx = 0;
        while (true)
        {
            NPCMovementPattern currentPattern = movementPatterns[currentPatternIdx];
            Vector3 targetPosition = transform.position + new Vector3(currentPattern.movement.x, currentPattern.movement.y);
            while ((transform.position - targetPosition).sqrMagnitude > Mathf.Epsilon)
            {
                Vector3 movementVector = targetPosition - transform.position;
                yield return new WaitUntil(() => !isInDialogue);
                character.MoveAndStop(new Vector2(movementVector.x, movementVector.y), currentPattern.run, () => GameManager.Instance.CheckForNPCs());
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(currentPattern.waitTime);
            currentPatternIdx = (currentPatternIdx + 1) % movementPatterns.Count;
        }
    }

    private void Start()
    {
        GameEvents.Instance.OnAfterDialogueExited += OnAfterDialogueExited;
        isInDialogue = false;
        if (movementPatterns != null && movementPatterns.Count > 0)
            moveCoroutine = StartCoroutine(Move());
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnAfterDialogueExited -= OnAfterDialogueExited;
    }

    private void OnAfterDialogueExited() {
        if (isInDialogue)
        {
            if (GetComponent<Trainer>() != null)
            {
                GameManager.Instance.TriggerTrainerBattle(GetComponent<Trainer>().PokemonPartyManager);
            }
        }
        isInDialogue = false;
    }
}

[System.Serializable]
public struct NPCMovementPattern
{
    public Vector2Int movement;
    public bool run;
    public float waitTime;
}