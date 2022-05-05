using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Animated sprites for players/enemies.
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
        //Repeatedly calls the UpdateFrame method.
        InvokeRepeating("UpdateFrame", 0.0f, this.animationRate);
    }

    //Upon player death, stop current animation, disable looping set to frame 0 and change the sprites it uses and then restart animation.
    public void KillPlayer(Sprite[] spritesToUse)
    {
        enable(false);
        loop = false;
        currentFrame = 0;
        sprites = spritesToUse;

        UpdateAnimationRate(0.5f);

        enable(true);
    }

    //When player is revived for new round reset back to previous values.
    public void RevivePlayer(Sprite[] spritesToUse)
    {
        loop = true;
        currentFrame = 0;
        sprites = spritesToUse;

        UpdateAnimationRate(0.05f);
    }

    //Sets the sprites to be currently used.
    public void SetSprites(Sprite[] spritesToUse)
    {
        sprites = spritesToUse;
    }

    //Sets the individual sprite to be used if no animated sprites are required.
    public void SetSprite(Sprite spriteToUse) {
        spriteRenderer.sprite = spriteToUse;
    }

    //Update the rate at which the animation plays.
    public void UpdateAnimationRate(float newRate)
    {
        CancelInvoke("UpdateFrame");

        animationRate = newRate;

        InvokeRepeating("UpdateFrame", 0.0f, animationRate);
    }

    //Updates the sprite used.
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

    //Sets if the animation should be enabled or not.
    public void enable(bool enable)
    {
        isEnabled = enable;
    }
}
