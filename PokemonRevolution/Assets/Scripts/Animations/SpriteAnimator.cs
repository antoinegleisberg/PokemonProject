using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    private SpriteRenderer _spriteRenderer;
    private List<AnimationFrame> _frames;

    private int _currentFrame;
    private float _timer;

    public float FrameRate { get; set; }

    public SpriteAnimator(SpriteRenderer spriteRenderer, List<AnimationFrame> frames, float frameRate = 0.16f)
    {
        _spriteRenderer = spriteRenderer;
        _frames = frames;
        FrameRate = frameRate;
    }

    public void Init()
    {
        _currentFrame = 0;
        _timer = 0.0f;
        UpdateFrame();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= FrameRate)
        {
            _timer -= FrameRate;
            _currentFrame = (_currentFrame + 1) % _frames.Count;
            UpdateFrame();
        }
    }
    
    private void UpdateFrame()
    {
        _spriteRenderer.sprite = _frames[_currentFrame].Sprite;
        _spriteRenderer.flipX = _frames[_currentFrame].FlipX;
        _spriteRenderer.flipY = _frames[_currentFrame].FlipY;
    }
}
