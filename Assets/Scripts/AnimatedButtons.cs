using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimatedButtons : MonoBehaviour {
    public Sprite[] sprites;
    public float animationRate = 0.05f;
    public int currentFrame { get; private set; }
    public bool isEnabled = false;
    private Image image;

    private void Awake() {
        image = this.GetComponent<Image>();
    }

    private void Start() {
        InvokeRepeating(nameof(UpdateFrame), this.animationRate, this.animationRate);
    }

    private void UpdateFrame() {
        if (isEnabled) {
            this.currentFrame++;

            if (this.currentFrame >= this.sprites.Length) {
                this.currentFrame = 0;
            }

            if (this.currentFrame >= 0 && this.currentFrame < this.sprites.Length) {
                image.sprite = this.sprites[currentFrame];
            }
        }
    }

    public void enable(bool enable) {
        this.isEnabled = enable;
        this.currentFrame = 0;
        image.sprite = this.sprites[currentFrame];
    }
}
