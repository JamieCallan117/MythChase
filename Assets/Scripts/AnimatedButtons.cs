using UnityEngine;
using UnityEngine.UI;

//For buttons with an animated image.
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
        //Repeatedly call the UpdateFrame method.
        InvokeRepeating(nameof(UpdateFrame), animationRate, animationRate);
    }

    //Updates what sprite is currently shown on the button.
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

    //Sets if the animation should be enabled or not.
    public void enable(bool enable)
    {
        isEnabled = enable;
        currentFrame = 0;
        image.sprite = sprites[currentFrame];
    }
}
