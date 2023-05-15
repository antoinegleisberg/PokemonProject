using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private List<AnimationFrame> walkDownSprites;
    [SerializeField] private List<AnimationFrame> walkUpSprites;
    [SerializeField] private List<AnimationFrame> walkLeftSprites;
    [SerializeField] private List<AnimationFrame> walkRightSprites;

    private Direction facingDirection;
    private bool isMoving;
    private bool isRunning;

    private float runningSpeedup = 1.5f;

    private SpriteRenderer spriteRenderer;

    private SpriteAnimator currentAnimator;
    private SpriteAnimator IdleDown;
    private SpriteAnimator IdleUp;
    private SpriteAnimator IdleLeft;
    private SpriteAnimator IdleRight;
    private SpriteAnimator WalkDown;
    private SpriteAnimator WalkUp;
    private SpriteAnimator WalkLeft;
    private SpriteAnimator WalkRight;

    private Dictionary<Direction, SpriteAnimator> walkAnimations;
    private Dictionary<Direction, SpriteAnimator> idleAnimations;

    public Direction FacingDirection {
        get { return facingDirection; }
        set {
            if (facingDirection == value)
                return;
            facingDirection = value;
            UpdateAnim();
        }
    }
    
    public bool IsMoving {
        get { return isMoving; }
        set
        {
            if (isMoving == value)
                return;
            isMoving = value;
            UpdateAnim();
        }
    }

    public bool IsRunning {
        get { return isRunning; }
        set
        {
            if (isRunning == value)
                return;
            isRunning = value;
            if (isRunning)
                currentAnimator.FrameRate /= runningSpeedup;
            else
                currentAnimator.FrameRate *= runningSpeedup;
        }
    }

    private void Awake()
    {
        FacingDirection = Direction.Down;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        IdleDown = new SpriteAnimator(spriteRenderer, new List<AnimationFrame>() { walkDownSprites[0] });
        IdleUp = new SpriteAnimator(spriteRenderer, new List<AnimationFrame>() { walkUpSprites[0] });
        IdleLeft = new SpriteAnimator(spriteRenderer, new List<AnimationFrame>() { walkLeftSprites[0] });
        IdleRight = new SpriteAnimator(spriteRenderer, new List<AnimationFrame>() { walkRightSprites[0] });
        WalkDown = new SpriteAnimator(spriteRenderer, walkDownSprites);
        WalkUp = new SpriteAnimator(spriteRenderer, walkUpSprites);
        WalkLeft = new SpriteAnimator(spriteRenderer, walkLeftSprites);
        WalkRight = new SpriteAnimator(spriteRenderer, walkRightSprites);

        walkAnimations = new Dictionary<Direction, SpriteAnimator>()
        {
            { Direction.Down, WalkDown },
            { Direction.Up, WalkUp },
            { Direction.Left, WalkLeft },
            { Direction.Right, WalkRight }
        };

        idleAnimations = new Dictionary<Direction, SpriteAnimator>()
        {
            { Direction.Down, IdleDown },
            { Direction.Up, IdleUp },
            { Direction.Left, IdleLeft },
            { Direction.Right, IdleRight }
        };

        UpdateAnim();
    }

    private void UpdateAnim()
    {
        bool wasRunning = IsRunning;
        // Reset the frame rate to the default value
        IsRunning = false;
        if (IsMoving)
            currentAnimator = walkAnimations[FacingDirection];
        else
            currentAnimator = idleAnimations[FacingDirection];
        // Restore the frame rate to the running value if necessary
        IsRunning = wasRunning;

        currentAnimator.Init();
    }

    private void Update()
    {
        currentAnimator.Update();
    }
}
