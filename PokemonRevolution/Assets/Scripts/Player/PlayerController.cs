using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float runningMoveSpeed;
    [SerializeField] private Animator animator;
    
    [SerializeField] private LayerMask solidObjectsCollidersLayer;
    [SerializeField] private LayerMask tallGrassLayer; // unused
    [SerializeField] private LayerMask interactableLayer;
    
    [SerializeField] private PokemonPartyManager playerPartyManager;
    
    private float moveSpeed;
    private Vector2Int lastInput;
    private bool isMoving;
    private Vector2Int facing;

    private void Start()
    {
        lastInput = InputManager.Instance.MovementInput;
        isMoving = false;
        moveSpeed = baseMoveSpeed;

        InputEvents.Instance.OnInteract += Interact;
    }

    private void OnDestroy()
    {
        InputEvents.Instance.OnInteract -= Interact;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!isMoving)
        {
            lastInput = InputManager.Instance.MovementInput;

            if (lastInput != Vector2Int.zero)
            {
                animator.SetFloat("moveX", lastInput.x);
                animator.SetFloat("moveY", lastInput.y);
                facing = lastInput;

                Vector3 targetPosition = playerTransform.position;
                targetPosition.x += lastInput.x;
                targetPosition.y += lastInput.y;

                if (IsWalkable(targetPosition))
                    StartCoroutine(Move(targetPosition));
            }
        }

        bool isRunning = InputManager.Instance.IsRunning;
        if (isRunning) moveSpeed = runningMoveSpeed;
        else moveSpeed = baseMoveSpeed;
        animator.SetBool("isMoving", isMoving && !isRunning);
        animator.SetBool("isRunning", isMoving && isRunning);
    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - playerTransform.position).sqrMagnitude > Mathf.Epsilon)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        playerTransform.position = targetPosition;
        isMoving = false;
        GameManager.Instance.CheckForEncounters(playerTransform.position);
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsCollidersLayer) != null)
        {
            return false;
        }
        return true;
    }
    
    private void Interact()
    {
        Vector2 interactPos = new Vector2(playerTransform.position.x, playerTransform.position.y) + facing;

        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            IInteractable interactable = collider.GetComponentInParent<Transform>().GetComponentInChildren<IInteractable>();
            interactable.Interact();
        }
    }
}