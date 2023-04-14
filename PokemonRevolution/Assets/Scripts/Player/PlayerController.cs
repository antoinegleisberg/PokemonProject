using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float runningMoveSpeedMultiplier = 1.75f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask solidObjectsCollidersLayer;
    [SerializeField] private LayerMask tallGrassLayer;
    [SerializeField] private int encouterRate = 10;
    private float moveSpeed;
    private Vector2Int lastInput;
    private bool isMoving;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        lastInput = InputManager.Instance.MovementInput;
        isMoving = false;
        moveSpeed = baseMoveSpeed;
    }

    // Update is called once per frame
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

                Vector3 targetPosition = transform.position;
                targetPosition.x += lastInput.x;
                targetPosition.y += lastInput.y;

                if (IsWalkable(targetPosition))
                    StartCoroutine(Move(targetPosition));
            }
        }

        bool isRunning = InputManager.Instance.IsRunning;
        if (isRunning) moveSpeed = baseMoveSpeed * runningMoveSpeedMultiplier;
        else moveSpeed = baseMoveSpeed;
        animator.SetBool("isMoving", isMoving && !isRunning);
        animator.SetBool("isRunning", isMoving && isRunning);

    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsCollidersLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, tallGrassLayer) != null)
        {
            if (Random.Range(0, 100) < encouterRate) Debug.Log("Encounter!");
        }
    }
}
