using System.Collections;
using UnityEngine;

public class Fov : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] private NPCController npcController;
    [SerializeField] private Character character;
    [SerializeField] private GameObject exclamation;

    private Coroutine fovCoroutine;
    
    private void Start()
    {
        exclamation.SetActive(false);
    }

    public void OnPlayerTriggered(PlayerController playerController)
    {
        Transform playerPosition = playerController.transform;
        OnEnterFOV(playerPosition);
    }

    public void OnEnterFOV(Transform source)
    {
        if (fovCoroutine != null)
            return;
        fovCoroutine = StartCoroutine(OnEnterFOVCoroutine(source));
    }

    private IEnumerator OnEnterFOVCoroutine(Transform source)
    {
        InputManager.Instance.ActivateUIActionMap();
        npcController.StopMoving();

        yield return ShowExclamation();

        yield return MoveToTarget(source);

        npcController.Interact(source);
    }

    private IEnumerator ShowExclamation()
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        Vector3 diff = target.position - transform.position;
        Vector2 movement = new Vector2(Mathf.Round(diff.x), Mathf.Round(diff.y));

        yield return new WaitUntil(() => !character.IsMoving);

        character.MoveAndStop(movement);

        yield return new WaitUntil(() => !character.IsMoving);
    }
}
