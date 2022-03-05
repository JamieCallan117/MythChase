using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprites : MonoBehaviour {
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;
    public float animationRate = 0.05f;
    public int currentFrame { get; private set; }
    public bool loop = true;
    public bool isEnabled = true;

    private void Awake() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        InvokeRepeating(nameof(UpdateFrame), this.animationRate, this.animationRate);
    }

    private void UpdateFrame() {
        if (enabled) {
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
