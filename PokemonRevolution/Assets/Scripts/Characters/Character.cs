using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private float baseMoveSpeed = 5.0f;
    [SerializeField] private float runningMoveSpeed = 8.0f;
    [SerializeField] private CharacterAnimator characterAnimator;

    private float moveSpeed;
    private bool isMoving;
    private bool isRunning;
    private bool isRunningMovementCoroutine;
    private bool stopAfterCurrentMovement;

    public bool IsMoving {
        get { return isMoving; }
        set
        {
            if (isMoving == value)
                return;
            isMoving = value;
            characterAnimator.IsMoving = value;
        }
    }

    public bool IsRunning {
        get { return isRunning; }
        set
        {
            if (isRunning == value)
                return;
            isRunning = value;
            characterAnimator.IsRunning = value;
            moveSpeed = (isRunning) ? runningMoveSpeed : baseMoveSpeed;
        }
    }

    private void Start()
    {
        IsRunning = false;
        IsMoving = false;
        moveSpeed = baseMoveSpeed;
    }
    
    public void FaceTowards(Direction facingDirection)
    {
        characterAnimator.FacingDirection = facingDirection;
        UpdateFOV(facingDirection);
    }

    public void FaceTowards(Vector3 direction)
    {
        Direction facingDirection = DirectionUtils.GetDirection(direction);
        FaceTowards(facingDirection);
    }

    public void StopMoving()
    {
        IsMoving = false;
        IsRunning = false;
        StopAllCoroutines();
        isRunningMovementCoroutine = false;
    }

    public IEnumerator MoveAndStop(Vector2 moveVector, bool run = false, Action onAfterMovement = null)
    {
        stopAfterCurrentMovement = true;
        IsRunning = run;
        if (!isRunningMovementCoroutine)
        {
            yield return MoveCoroutine(moveVector, onAfterMovement);
        }
    }

    public void MoveContinuous(Vector2 moveVector, bool run = false, Action onAfterMovement = null)
    {
        if (moveVector == Vector2.zero)
        {
            if (!isRunningMovementCoroutine)
                IsMoving = false;
            stopAfterCurrentMovement = true;
            return;
        }

        stopAfterCurrentMovement = false;
        IsRunning = run;
        if (!isRunningMovementCoroutine)
            StartCoroutine(MoveCoroutine(moveVector, onAfterMovement));
    }

    private IEnumerator MoveCoroutine(Vector2 moveVector, Action onAfterMovement = null)
    {
        isRunningMovementCoroutine = true;

        Vector2Int moveDir = GetMoveDirection(moveVector);
        int moveLength = GetMoveDistance(moveVector);

        FaceTowards(DirectionUtils.GetDirection(moveDir));

        for (int i = 0; i < moveLength; i++)
        {
            Vector3 targetPosition = characterTransform.position;
            targetPosition.x += moveDir.x;
            targetPosition.y += moveDir.y;

            if (!IsWalkable(targetPosition))
            {
                isRunningMovementCoroutine = false;
                IsMoving = false;
                yield break;
            }

            IsMoving = true;

            while ((targetPosition - characterTransform.position).sqrMagnitude > Mathf.Epsilon)
            {
                characterTransform.position = Vector3.MoveTowards(characterTransform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            characterTransform.position = targetPosition;

            onAfterMovement?.Invoke();
            yield return new WaitForEndOfFrame();
        }

        isRunningMovementCoroutine = false;
        if (stopAfterCurrentMovement)
            IsMoving = false;
    }
    
    private Vector2Int GetMoveDirection(Vector2 moveVector)
    {
        Vector2Int moveDirection = new Vector2Int(0, 0);
        if (Mathf.Abs(moveVector.x) > Mathf.Abs(moveVector.y))
        {
            moveDirection.x = Mathf.RoundToInt(Mathf.Sign(moveVector.x));
        }
        else
        {
            moveDirection.y = Mathf.RoundToInt(Mathf.Sign(moveVector.y));
        }
        return moveDirection;
    }

    private int GetMoveDistance(Vector2 moveVector)
    {
        int moveLength = 0;
        if (Mathf.Abs(moveVector.x) > Mathf.Abs(moveVector.y))
        {
            moveLength = Mathf.RoundToInt(Mathf.Abs(moveVector.x));
        }
        else
        {
            moveLength = Mathf.RoundToInt(Mathf.Abs(moveVector.y));
        }
        return moveLength;
    }

    private void UpdateFOV(Direction facingDirection)
    {
        Fov fov = transform.parent.GetComponentInChildren<Fov>();
        if (fov != null)
            // minus because z axis faces into the screen
            fov.transform.eulerAngles = new Vector3(0, 0, -DirectionUtils.Rotation(facingDirection));
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, GameLayers.Instance.SolidObjectsCollidersLayer) != null)
        {
            return false;
        }
        return true;
    }
}
