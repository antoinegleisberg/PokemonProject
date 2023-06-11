using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private List<AnimationFrame> _walkDownSprites;
    [SerializeField] private List<AnimationFrame> _walkUpSprites;
    [SerializeField] private List<AnimationFrame> _walkLeftSprites;
    [SerializeField] private List<AnimationFrame> _walkRightSprites;

    private Direction _facingDirection;
    private bool _isMoving;
    private bool _isRunning;

    private float _runningSpeedup = 1.5f;

    private SpriteRenderer _spriteRenderer;

    private SpriteAnimator _currentAnimator;
    private SpriteAnimator _idleDown;
    private SpriteAnimator _idleUp;
    private SpriteAnimator _idleLeft;
    private SpriteAnimator _idleRight;
    private SpriteAnimator _walkDown;
    private SpriteAnimator _walkUp;
    private SpriteAnimator _walkLeft;
    private SpriteAnimator _walkRight;

    private Dictionary<Direction, SpriteAnimator> _walkAnimations;
    private Dictionary<Direction, SpriteAnimator> _idleAnimations;

    public Direction FacingDirection {
        get { return _facingDirection; }
        set {
            if (_facingDirection == value)
                return;
            _facingDirection = value;
            UpdateAnimator();
        }
    }
    
    public bool IsMoving {
        get { return _isMoving; }
        set
        {
            if (_isMoving == value)
                return;
            _isMoving = value;
            UpdateAnimator();
        }
    }

    public bool IsRunning {
        get { return _isRunning; }
        set
        {
            if (_isRunning == value)
                return;
            _isRunning = value;
            if (_isRunning)
                _currentAnimator.FrameRate /= _runningSpeedup;
            else
                _currentAnimator.FrameRate *= _runningSpeedup;
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _idleDown = new SpriteAnimator(_spriteRenderer, new List<AnimationFrame>() { _walkDownSprites[0] });
        _idleUp = new SpriteAnimator(_spriteRenderer, new List<AnimationFrame>() { _walkUpSprites[0] });
        _idleLeft = new SpriteAnimator(_spriteRenderer, new List<AnimationFrame>() { _walkLeftSprites[0] });
        _idleRight = new SpriteAnimator(_spriteRenderer, new List<AnimationFrame>() { _walkRightSprites[0] });
        _walkDown = new SpriteAnimator(_spriteRenderer, _walkDownSprites);
        _walkUp = new SpriteAnimator(_spriteRenderer, _walkUpSprites);
        _walkLeft = new SpriteAnimator(_spriteRenderer, _walkLeftSprites);
        _walkRight = new SpriteAnimator(_spriteRenderer, _walkRightSprites);

        _walkAnimations = new Dictionary<Direction, SpriteAnimator>()
        {
            { Direction.Down, _walkDown },
            { Direction.Up, _walkUp },
            { Direction.Left, _walkLeft },
            { Direction.Right, _walkRight }
        };

        _idleAnimations = new Dictionary<Direction, SpriteAnimator>()
        {
            { Direction.Down, _idleDown },
            { Direction.Up, _idleUp },
            { Direction.Left, _idleLeft },
            { Direction.Right, _idleRight }
        };

        FacingDirection = Direction.Down;
        UpdateAnimator();
    }

    private void Update()
    {
        _currentAnimator.Update();
    }
    
    private void UpdateAnimator()
    {
        bool wasRunning = IsRunning;
        // Reset the frame rate to the default value
        IsRunning = false;
        if (IsMoving)
            _currentAnimator = _walkAnimations[FacingDirection];
        else
            _currentAnimator = _idleAnimations[FacingDirection];
        // Restore the frame rate to the running value if necessary
        IsRunning = wasRunning;

        _currentAnimator.Init();
    }
}
