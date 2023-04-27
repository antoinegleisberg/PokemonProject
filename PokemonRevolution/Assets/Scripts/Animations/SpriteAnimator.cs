using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    private SpriteRenderer spriteRenderer;
    private List<AnimationFrame> frames;
    private float frameRate;

    private int currentFrame;
    private float timer;

    public float FrameRate {
        get { return frameRate; }
        set { frameRate = value; }
    }

    public SpriteAnimator(SpriteRenderer spriteRenderer, List<AnimationFrame> frames, float frameRate = 0.16f)
    {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.frameRate = frameRate;
    }

    public void Init()
    {
        currentFrame = 0;
        timer = 0.0f;
        UpdateFrame();
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % frames.Count;
            UpdateFrame();
        }
    }

    private void UpdateFrame()
    {
        spriteRenderer.sprite = frames[currentFrame].sprite;
        spriteRenderer.flipX = frames[currentFrame].flipX;
        spriteRenderer.flipY = frames[currentFrame].flipY;
    }
}
