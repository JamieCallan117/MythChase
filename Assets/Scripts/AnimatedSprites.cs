using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprites : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private float animationRate = 0.05f;
    private int currentFrame;
    private bool loop = true;
    private bool isEnabled = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating("UpdateFrame", 0.0f, this.animationRate);
    }

    public void KillPlayer(Sprite[] spritesToUse)
    {
        enable(false);
        loop = false;
        currentFrame = 0;
        sprites = spritesToUse;

        UpdateAnimationRate(0.5f);

        enable(true);
    }

    public void RevivePlayer(Sprite[] spritesToUse)
    {
        loop = true;
        currentFrame = 0;
        sprites = spritesToUse;

        UpdateAnimationRate(0.05f);
    }

    public void SetSprites(Sprite[] spritesToUse)
    {
        sprites = spritesToUse;
    }

    public void SetSprite(Sprite spriteToUse) {
        spriteRenderer.sprite = spriteToUse;
    }

    public void UpdateAnimationRate(float newRate)
    {
        CancelInvoke("UpdateFrame");

        animationRate = newRate;

        InvokeRepeating("UpdateFrame", 0.0f, animationRate);
    }

    private void UpdateFrame()
    {
        if (isEnabled)
        {
            currentFrame++;

            if (currentFrame >= sprites.Length && loop)
            {
                currentFrame = 0;
            }

            if (currentFrame >= 0 && currentFrame < sprites.Length)
            {
                spriteRenderer.sprite = sprites[currentFrame];
            }
        }
    }

    public void enable(bool enable)
    {
        isEnabled = enable;
    }
}
