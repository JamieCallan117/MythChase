using UnityEngine;
using UnityEngine.UI;

//The star icon on the achievements scene.
public class AchievementStar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite unlockedSprite;

    //Set the sprite as the unlocked sprite.
    public void UnlockAchievement()
    {
        image.sprite = unlockedSprite;
    }
}
