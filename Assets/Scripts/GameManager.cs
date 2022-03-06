using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public GameObject player;
    public GameObject lifeOne;
    public GameObject lifeTwo;
    public GameObject lifeThree;
    public GameObject powerUp;
    public GameObject scoreValue;
    private SpriteRenderer playerSprite;
    private Image lifeOneImage;
    private Image lifeTwoImage;
    private Image lifeThreeImage;
    private Image powerUpImage;
    private TextMeshProUGUI scoreText;
    private AnimatedSprites aniSprites;
    private int score;

    private void Awake() {
        aniSprites = player.GetComponent(typeof(AnimatedSprites)) as AnimatedSprites;
        playerSprite = player.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        lifeOneImage = lifeOne.GetComponent(typeof(Image)) as Image;
        lifeTwoImage = lifeTwo.GetComponent(typeof(Image)) as Image;
        lifeThreeImage = lifeThree.GetComponent(typeof(Image)) as Image;
        powerUpImage = powerUp.GetComponent(typeof(Image)) as Image;
        scoreText = scoreValue.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }

    private void Start() {  
        SetScore(0);      
        LoadPlayerSprites();
    }

    private void Update() {
        scoreText.text = score.ToString();
    }

    public void EatPellet(Pellet pellet) {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);

    }

    private void SetScore(int newScore) {
        this.score = newScore;
    }

    private void LoadPlayerSprites() {
        Sprite[] playerSprites = new Sprite[4];

        switch(PlayerStats.character) {
            case 2:
                playerSprite.sprite = Resources.Load<Sprite>("Kiara_Normal_01");
                lifeOneImage.sprite = Resources.Load<Sprite>("Kiara_Normal_01");
                lifeTwoImage.sprite = Resources.Load<Sprite>("Kiara_Normal_01");
                lifeThreeImage.sprite = Resources.Load<Sprite>("Kiara_Normal_01");
                powerUpImage.sprite = Resources.Load<Sprite>("Powerup_Kotori");

                playerSprites[0] = Resources.Load<Sprite>("Kiara_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Kiara_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Kiara_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Kiara_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            case 3:
                playerSprite.sprite = Resources.Load<Sprite>("Ame_Normal_01");
                lifeOneImage.sprite = Resources.Load<Sprite>("Ame_Normal_01");
                lifeTwoImage.sprite = Resources.Load<Sprite>("Ame_Normal_01");
                lifeThreeImage.sprite = Resources.Load<Sprite>("Ame_Normal_01");
                powerUpImage.sprite = Resources.Load<Sprite>("Powerup_Bubba");

                playerSprites[0] = Resources.Load<Sprite>("Ame_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Ame_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Ame_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Ame_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            case 4:
                playerSprite.sprite = Resources.Load<Sprite>("Calli_Normal_01");
                lifeOneImage.sprite = Resources.Load<Sprite>("Calli_Normal_01");
                lifeTwoImage.sprite = Resources.Load<Sprite>("Calli_Normal_01");
                lifeThreeImage.sprite = Resources.Load<Sprite>("Calli_Normal_01");
                powerUpImage.sprite = Resources.Load<Sprite>("Powerup_DeathSensei");

                playerSprites[0] = Resources.Load<Sprite>("Calli_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Calli_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Calli_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Calli_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            case 5:
                playerSprite.sprite = Resources.Load<Sprite>("Gura_Normal_01");
                lifeOneImage.sprite = Resources.Load<Sprite>("Gura_Normal_01");
                lifeTwoImage.sprite = Resources.Load<Sprite>("Gura_Normal_01");
                lifeThreeImage.sprite = Resources.Load<Sprite>("Gura_Normal_01");
                powerUpImage.sprite = Resources.Load<Sprite>("Powerup_Bloop");

                playerSprites[0] = Resources.Load<Sprite>("Gura_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Gura_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Gura_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Gura_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            default:
                playerSprite.sprite = Resources.Load<Sprite>("Ina_Normal_01");
                lifeOneImage.sprite = Resources.Load<Sprite>("Ina_Normal_01");
                lifeTwoImage.sprite = Resources.Load<Sprite>("Ina_Normal_01");
                lifeThreeImage.sprite = Resources.Load<Sprite>("Ina_Normal_01");
                powerUpImage.sprite = Resources.Load<Sprite>("Powerup_Takodachi");

                playerSprites[0] = Resources.Load<Sprite>("Ina_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Ina_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Ina_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Ina_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
        }
    }
}
