using UnityEngine;
using UnityEngine.UI;

public class AchievementStar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite unlockedSprite;

    public void UnlockAchievement()
    {
        image.sprite = unlockedSprite;
    }
}
