using System.Collections;
using UnityEngine;

public class Fov : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] private NPCController npcController;
    [SerializeField] private Character character;
    [SerializeField] private GameObject exclamation;

    private Coroutine fovCoroutine;

    public void OnPlayerTriggered(PlayerController playerController)
    {
        Transform playerPosition = playerController.transform;
        OnEnterFOV(playerPosition);
    }

    public void OnEnterFOV(Transform source)
    {
        if (fovCoroutine != null)
            return;
        GameEvents.Instance.EnterNpcFov();
        npcController.StopMoving();
        fovCoroutine = StartCoroutine(OnEnterFOVCoroutine(source));
    }

    private IEnumerator OnEnterFOVCoroutine(Transform source)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        Vector3 diff = source.position - transform.position;
        Vector2 movement = new Vector2(Mathf.Round(diff.x), Mathf.Round(diff.y));

        while (character.IsMoving)
        {
            yield return new WaitForEndOfFrame();
        }

        character.MoveAndStop(movement);

        while (character.IsMoving)
        {
            yield return new WaitForEndOfFrame();
        }

        npcController.Interact(source);
    }

    private void Start()
    {
        exclamation.SetActive(false);
    }
}
