using System.Collections;
using UnityEngine;

public class Fov : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] private NPCController _npcController;
    [SerializeField] private Character _character;
    [SerializeField] private GameObject _exclamation;

    private Coroutine _fovCoroutine;
    
    private void Start()
    {
        _exclamation.SetActive(false);
    }

    public void OnPlayerTriggered(PlayerController playerController)
    {
        Transform playerPosition = playerController.transform;
        OnEnterFOV(playerPosition);
    }

    public void OnEnterFOV(Transform source)
    {
        if (_fovCoroutine != null)
            return;
        if (enabled == false)
            return;
        _fovCoroutine = StartCoroutine(OnEnterFOVCoroutine(source));
    }

    private IEnumerator OnEnterFOVCoroutine(Transform source)
    {
        InputManager.Instance.ActivateUIActionMap();
        _npcController.StopMoving();

        yield return ShowExclamation();

        yield return MoveToTarget(source);

        _npcController.Interact(source);
    }

    private IEnumerator ShowExclamation()
    {
        _exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _exclamation.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        Vector3 diff = target.position - transform.position;
        Vector2 movement = new Vector2(Mathf.Round(diff.x), Mathf.Round(diff.y));
        movement -= movement.normalized;

        yield return new WaitUntil(() => !_character.IsMoving);

        yield return _character.MoveAndStop(movement);

        yield return new WaitUntil(() => !_character.IsMoving);
    }
}
