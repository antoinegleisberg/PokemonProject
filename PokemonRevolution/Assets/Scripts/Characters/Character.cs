using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Transform _characterTransform;
    [SerializeField] private float _baseMoveSpeed = 5.0f;
    [SerializeField] private float _runningMoveSpeed = 8.0f;
    [SerializeField] private CharacterAnimator _characterAnimator;

    private float _moveSpeed;
    private bool _isMoving;
    private bool _isRunning;
    private bool _isRunningMovementCoroutine;
    private bool _stopAfterCurrentMovement;

    public bool IsMoving {
        get { return _isMoving; }
        set
        {
            if (_isMoving == value)
                return;
            _isMoving = value;
            _characterAnimator.IsMoving = value;
        }
    }

    public bool IsRunning {
        get { return _isRunning; }
        set
        {
            if (_isRunning == value)
                return;
            _isRunning = value;
            _characterAnimator.IsRunning = value;
            _moveSpeed = (_isRunning) ? _runningMoveSpeed : _baseMoveSpeed;
        }
    }

    private void Start()
    {
        IsRunning = false;
        IsMoving = false;
        _moveSpeed = _baseMoveSpeed;
    }
    
    public void FaceTowards(Direction facingDirection)
    {
        _characterAnimator.FacingDirection = facingDirection;
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
        _isRunningMovementCoroutine = false;
    }

    public IEnumerator MoveAndStop(Vector2 moveVector, bool run = false, Action onAfterMovement = null)
    {
        _stopAfterCurrentMovement = true;
        IsRunning = run;
        if (!_isRunningMovementCoroutine)
        {
            yield return MoveCoroutine(moveVector, onAfterMovement);
        }
    }

    public void MoveContinuous(Vector2 moveVector, bool run = false, Action onAfterMovement = null)
    {
        if (moveVector == Vector2.zero)
        {
            if (!_isRunningMovementCoroutine)
                IsMoving = false;
            _stopAfterCurrentMovement = true;
            return;
        }

        _stopAfterCurrentMovement = false;
        IsRunning = run;
        if (!_isRunningMovementCoroutine)
            StartCoroutine(MoveCoroutine(moveVector, onAfterMovement));
    }

    private IEnumerator MoveCoroutine(Vector2 moveVector, Action onAfterMovement = null)
    {
        _isRunningMovementCoroutine = true;

        Vector2Int moveDir = GetMoveDirection(moveVector);
        int moveLength = GetMoveDistance(moveVector);

        FaceTowards(DirectionUtils.GetDirection(moveDir));

        for (int i = 0; i < moveLength; i++)
        {
            Vector3 targetPosition = _characterTransform.position;
            targetPosition.x += moveDir.x;
            targetPosition.y += moveDir.y;

            if (!IsWalkable(targetPosition))
            {
                _isRunningMovementCoroutine = false;
                IsMoving = false;
                yield break;
            }

            IsMoving = true;

            while ((targetPosition - _characterTransform.position).sqrMagnitude > Mathf.Epsilon)
            {
                _characterTransform.position = Vector3.MoveTowards(_characterTransform.position, targetPosition, _moveSpeed * Time.deltaTime);
                yield return null;
            }

            _characterTransform.position = targetPosition;

            onAfterMovement?.Invoke();
            yield return new WaitForEndOfFrame();
        }

        _isRunningMovementCoroutine = false;
        if (_stopAfterCurrentMovement)
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
