using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimatedButtons : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float animationRate = 0.05f;
    [SerializeField] private int currentFrame;
    [SerializeField] private bool isEnabled = false;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        InvokeRepeating(nameof(UpdateFrame), animationRate, animationRate);
    }

    private void UpdateFrame()
    {
        if (isEnabled) {
            currentFrame++;

            if (currentFrame >= sprites.Length)
            {
                currentFrame = 0;
            }

            if (currentFrame >= 0 && currentFrame < sprites.Length)
            {
                image.sprite = sprites[currentFrame];
            }
        }
    }

    public void enable(bool enable)
    {
        isEnabled = enable;
        currentFrame = 0;
        image.sprite = sprites[currentFrame];
    }
}
