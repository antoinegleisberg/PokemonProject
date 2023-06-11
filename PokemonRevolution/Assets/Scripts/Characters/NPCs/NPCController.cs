using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable, ISaveable
{
    [SerializeField] private Direction _defaultFacingDirection;
    [SerializeField] private List<Dialogue> _dialogues;
    [SerializeField] private Character _character;
    [SerializeField] private List<NPCMovementPattern> _movementPatterns;
    [SerializeField] private Fov _fov;

    private bool _isInDialogue;
    private int _dialogueIdx;

    private Coroutine _movementPatternCoroutine;

    private void Start()
    {
        _isInDialogue = false;
        _dialogueIdx = 0;
        
        _character.FaceTowards(_defaultFacingDirection);
        if (_movementPatterns != null && _movementPatterns.Count > 0)
            _movementPatternCoroutine = StartCoroutine(MovementPatternCoroutine());
    }

    public void Interact(Transform source)
    {
        if (_character.IsMoving)
            return;

        DeactivateFov();

        _isInDialogue = true;
        
        Vector3 faceDirection = source.position - transform.position;
        _character.FaceTowards(faceDirection);
        source.GetComponentInChildren<Character>()?.FaceTowards(-faceDirection);
        
        DialogueManager.Instance.ShowDialogue(_dialogues[_dialogueIdx], OnAfterDialogueExited);
        if (_dialogueIdx < _dialogues.Count - 1)
            _dialogueIdx++;
    }

    public void StopMoving()
    {
        if (_movementPatternCoroutine != null)
        {
            StopAllCoroutines();
            _character.StopMoving();
        }
    }

    public void SaveData(ref GameData data)
    {
        GuidHolder guidHolder = GetComponent<GuidHolder>();
        string sceneName = gameObject.scene.name;

        if (GetComponent<GuidHolder>() == null)
        {
            return;
        }
        if ((_dialogues == null || _dialogues.Count <= 1) && _fov == null)
        {
            return;
        }


        string uid = guidHolder.UniqueId;
        bool fovIsEnabled = false;
        if (_fov != null && _fov.isActiveAndEnabled)
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
        
        data.ScenesData[sceneName].NPCsSaveData[uid] = new NPCSaveData(fovIsEnabled, _dialogueIdx);
    }

    public void LoadData(GameData data)
    {
        if (GetComponent<GuidHolder>() == null)
        {
            return;
        }

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

        if (_fov != null)
        {
            _fov.enabled = npcData.FovIsEnabled;
        }
        _dialogueIdx = npcData.DialogueIndex;
    }

    private void DeactivateFov()
    {
        if (_fov != null)
        {
            _fov.enabled = false;
        }
    }

    private IEnumerator MovementPatternCoroutine()
    {
        int currentPatternIdx = 0;
        while (true)
        {
            NPCMovementPattern currentPattern = _movementPatterns[currentPatternIdx];
            if (currentPattern.Movement != Vector2Int.zero)
            {
                yield return Move(currentPattern);
            }
            else
            {
                _character.FaceTowards(currentPattern.LookTowards);
            }
            yield return new WaitForSeconds(currentPattern.WaitTime);
            currentPatternIdx = (currentPatternIdx + 1) % _movementPatterns.Count;
        }
    }
    
    private IEnumerator Move(NPCMovementPattern currentPattern)
    {
        Vector3 targetPosition = transform.position + new Vector3(currentPattern.Movement.x, currentPattern.Movement.y);
        while ((transform.position - targetPosition).sqrMagnitude > Mathf.Epsilon)
        {
            Vector3 movementVector = targetPosition - transform.position;
            yield return new WaitUntil(() => !_isInDialogue);
            yield return _character.MoveAndStop(new Vector2(movementVector.x, movementVector.y), currentPattern.Run, () => GameManager.Instance.CheckForNPCs());
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnAfterDialogueExited() {
        _isInDialogue = false;

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