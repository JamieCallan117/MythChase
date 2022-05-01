using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprites : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float animationRate = 0.05f;
    public int currentFrame;
    public bool loop = true;
    public bool isEnabled = true;

    private void Awake() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        InvokeRepeating("UpdateFrame", 0.0f, this.animationRate);
    }

    public void UpdateAnimationRate(float newRate) {
        CancelInvoke("UpdateFrame");

        animationRate = newRate;

        InvokeRepeating("UpdateFrame", 0.0f, this.animationRate);
    }

    private void UpdateFrame() {
        if (isEnabled) {
            this.currentFrame++;

            if (this.currentFrame >= this.sprites.Length && this.loop) {
                this.currentFrame = 0;
            }

            if (this.currentFrame >= 0 && this.currentFrame < this.sprites.Length) {
                this.spriteRenderer.sprite = this.sprites[currentFrame];
            }
        }
    }

    public void enable(bool enable) {
        this.isEnabled = enable;
    }
}
