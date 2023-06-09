using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable, ISaveable
{
    [SerializeField] private Direction defaultFacingDirection;

    [SerializeField] private List<Dialogue> dialogues;
    [SerializeField] private Character character;
    [SerializeField] private List<NPCMovementPattern> movementPatterns;

    private bool isInDialogue;
    private int dialogueIdx;

    private Coroutine movementPatternCoroutine;

    private void Start()
    {
        isInDialogue = false;
        dialogueIdx = 0;
        
        character.FaceTowards(defaultFacingDirection);
        if (movementPatterns != null && movementPatterns.Count > 0)
            movementPatternCoroutine = StartCoroutine(MovementPatternCoroutine());
    }

    public void Interact(Transform source)
    {
        if (character.IsMoving)
            return;

        DeactivateFov();

        isInDialogue = true;
        
        Vector3 faceDirection = source.position - transform.position;
        character.FaceTowards(faceDirection);
        source.GetComponentInChildren<Character>()?.FaceTowards(-faceDirection);
        
        DialogueManager.Instance.ShowDialogue(dialogues[dialogueIdx], OnAfterDialogueExited);
        if (dialogueIdx < dialogues.Count - 1)
            dialogueIdx++;
    }

    public void StopMoving()
    {
        if (movementPatternCoroutine != null)
        {
            StopAllCoroutines();
            character.StopMoving();
        }
    }

    public void SaveData(ref GameData data)
    {
        Fov FOV = transform.parent.GetComponentInChildren<Fov>();
        GuidHolder guidHolder = GetComponent<GuidHolder>();
        string sceneName = gameObject.scene.name;

        if (GetComponent<GuidHolder>() == null)
        {
            return;
        }
        if ((dialogues == null || dialogues.Count <= 1) && FOV == null)
        {
            return;
        }


        string uid = guidHolder.UniqueId;
        bool fovIsEnabled = false;
        if (FOV != null && FOV.isActiveAndEnabled)
        {
            fovIsEnabled = true;
        }

        if (!data.ScenesData.ContainsKey(sceneName))
        {
            data.ScenesData.Add(sceneName, new SceneSaveData(null));
        }
        if (data.ScenesData[sceneName].NPCsSaveData.ContainsKey(uid))
        {
            data.ScenesData[sceneName].NPCsSaveData.Remove(uid);
        }
        
        data.ScenesData[sceneName].NPCsSaveData[uid] = new NPCSaveData(fovIsEnabled, dialogueIdx);
    }

    public void LoadData(GameData data)
    {
        if (GetComponent<GuidHolder>() == null)
        {
            return;
        }

        Fov FOV = transform.parent.GetComponentInChildren<Fov>();

        string uid = GetComponent<GuidHolder>().UniqueId;
        string sceneName = gameObject.scene.name;

        if (!data.ScenesData.ContainsKey(sceneName))
        {
            return;
        }
        if (!data.ScenesData[sceneName].NPCsSaveData.ContainsKey(uid))
        {
            return;
        }

        NPCSaveData npcData = data.ScenesData[sceneName].NPCsSaveData[uid];

        if (FOV != null)
        {
            FOV.enabled = npcData.FovIsEnabled;
        }
        dialogueIdx = npcData.DialogueIndex;
    }

    private void DeactivateFov()
    {
        Fov FOV = transform.parent.GetComponentInChildren<Fov>();
        if (FOV != null)
        {
            FOV.enabled = false;
        }
    }

    private IEnumerator MovementPatternCoroutine()
    {
        int currentPatternIdx = 0;
        while (true)
        {
            NPCMovementPattern currentPattern = movementPatterns[currentPatternIdx];
            if (currentPattern.movement != Vector2Int.zero)
            {
                yield return Move(currentPattern);
            }
            else
            {
                character.FaceTowards(currentPattern.lookTowards);
            }
            yield return new WaitForSeconds(currentPattern.waitTime);
            currentPatternIdx = (currentPatternIdx + 1) % movementPatterns.Count;
        }
    }
    
    private IEnumerator Move(NPCMovementPattern currentPattern)
    {
        Vector3 targetPosition = transform.position + new Vector3(currentPattern.movement.x, currentPattern.movement.y);
        while ((transform.position - targetPosition).sqrMagnitude > Mathf.Epsilon)
        {
            Vector3 movementVector = targetPosition - transform.position;
            yield return new WaitUntil(() => !isInDialogue);
            yield return character.MoveAndStop(new Vector2(movementVector.x, movementVector.y), currentPattern.run, () => GameManager.Instance.CheckForNPCs());
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnAfterDialogueExited() {
        isInDialogue = false;

        Trainer trainer = GetComponent<Trainer>();
        bool isTrainer = trainer != null;
        if (!isTrainer)
        {
            return;
        }
        if (!trainer.CanBattle)
        {
            return;
        }
        
        trainer.PokemonPartyManager.PokemonParty.HealAll();
        GameManager.Instance.StartBattle(trainer.PokemonPartyManager.PokemonParty, trainer);
    }
}