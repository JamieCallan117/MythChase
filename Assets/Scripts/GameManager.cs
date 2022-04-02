using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {
    public GameObject player;
    public GameObject enemyOne;
    public GameObject enemyTwo;
    public GameObject enemyThree;
    public GameObject enemyFour;
    public GameObject lifeOne;
    public GameObject lifeTwo;
    public GameObject lifeThree;
    public GameObject powerUp;
    public GameObject scoreValue;
    public GameObject readyText;
    private Enemy enemyOneAtr;
    private Enemy enemyTwoAtr;
    private Enemy enemyThreeAtr;
    private Enemy enemyFourAtr;
    private SpriteRenderer playerSprite;
    private SpriteRenderer enemyOneSprite;
    private SpriteRenderer enemyTwoSprite;
    private SpriteRenderer enemyThreeSprite;
    private SpriteRenderer enemyFourSprite;
    private Image lifeOneImage;
    private Image lifeTwoImage;
    private Image lifeThreeImage;
    private Image powerUpImage;
    private TextMeshProUGUI scoreText;
    private AnimatedSprites playerAniSprites;
    private AnimatedSprites enemyOneAniSprites;
    private AnimatedSprites enemyTwoAniSprites;
    private AnimatedSprites enemyThreeAniSprites;
    private AnimatedSprites enemyFourAniSprites;
    private AudioSource audioSource;
    private int score;
    private List<Pellet> pellets = new List<Pellet>();

    private void Awake() {
        playerAniSprites = player.GetComponent(typeof(AnimatedSprites)) as AnimatedSprites;
        enemyOneAniSprites = enemyOne.GetComponent(typeof(AnimatedSprites)) as AnimatedSprites;
        enemyTwoAniSprites = enemyTwo.GetComponent(typeof(AnimatedSprites)) as AnimatedSprites;
        enemyThreeAniSprites = enemyThree.GetComponent(typeof(AnimatedSprites)) as AnimatedSprites;
        enemyFourAniSprites = enemyFour.GetComponent(typeof(AnimatedSprites)) as AnimatedSprites;
        playerSprite = player.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        enemyOneSprite = enemyOne.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        enemyTwoSprite = enemyTwo.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        enemyThreeSprite = enemyThree.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        enemyFourSprite = enemyFour.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        lifeOneImage = lifeOne.GetComponent(typeof(Image)) as Image;
        lifeTwoImage = lifeTwo.GetComponent(typeof(Image)) as Image;
        lifeThreeImage = lifeThree.GetComponent(typeof(Image)) as Image;
        powerUpImage = powerUp.GetComponent(typeof(Image)) as Image;
        enemyOneAtr = enemyOne.GetComponent(typeof(Enemy)) as Enemy;
        enemyTwoAtr = enemyTwo.GetComponent(typeof(Enemy)) as Enemy;
        enemyThreeAtr = enemyThree.GetComponent(typeof(Enemy)) as Enemy;
        enemyFourAtr = enemyFour.GetComponent(typeof(Enemy)) as Enemy;
        scoreText = scoreValue.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {  
        SetScore(0);      
        LoadPlayerSprites();  

        StartCoroutine(newRoundWait());

        audioSource.Play();
    }

    private void Update() {
        scoreText.text = score.ToString();

        bool foundEnabledPellet = false;

        for (int i = 0; i < pellets.Count; i++) {
            if (pellets[i].gameObject.activeInHierarchy == true && foundEnabledPellet == false) {
                foundEnabledPellet = true;
                break;
            }
        }

        if (foundEnabledPellet == false) {
                playerAniSprites.isEnabled = false;
                enemyOneAniSprites.isEnabled = false;
                enemyTwoAniSprites.isEnabled = false;
                enemyThreeAniSprites.isEnabled = false;
                enemyFourAniSprites.isEnabled = false;
                NewRound();
            }
    }

    private IEnumerator newRoundWait() {
        yield return new WaitForSecondsRealtime(5);

        startGame();
    }

    private IEnumerator roundEndWait() {
        for (int i = 0; i < pellets.Count; i++) {
            pellets[i].gameObject.SetActive(true);
        }
        
        yield return new WaitForSecondsRealtime(2);

        StartCoroutine(newRoundWait());
    }

    private void startGame() {
        Movement playerMovement = player.GetComponent(typeof(Movement)) as Movement;

        playerMovement.movementEnabled = true;
        readyText.SetActive(false);
    }

    private void NewRound() {
        Movement playerMovement = player.GetComponent(typeof(Movement)) as Movement;

        playerMovement.movementEnabled = false;

        readyText.SetActive(true);

        player.transform.position = new Vector3(0, -3.5f, -5.0f);
        enemyOne.transform.position = new Vector3(0, 2.5f, -5.0f);
        enemyTwo.transform.position = new Vector3(-2, -0.5f, -5.0f);
        enemyThree.transform.position = new Vector3(0, -0.5f, -5.0f);
        enemyFour.transform.position = new Vector3(2, -0.5f, -5.0f);

        StartCoroutine(roundEndWait());
    }

    public void EatPellet(Pellet pellet) {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);
    }

    public void EatLargePellet(LargePellet pellet) {
        EatPellet(pellet);

        VulnerableEnemies();
    }

    public void AddPellet(Pellet pellet) {
        pellets.Add(pellet);
    }

    private void SetScore(int newScore) {
        this.score = newScore;
    }

    private void VulnerableEnemies() {
        enemyOneAtr.SetVulnerable(true);
        enemyTwoAtr.SetVulnerable(true);
        enemyThreeAtr.SetVulnerable(true);
        enemyFourAtr.SetVulnerable(true);
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

                playerAniSprites.sprites = playerSprites;

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 3, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);
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

                playerAniSprites.sprites = playerSprites;

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);
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

                playerAniSprites.sprites = playerSprites;

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 3, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);
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

                playerAniSprites.sprites = playerSprites;

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 3, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 4, enemyFourAtr);
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

                playerAniSprites.sprites = playerSprites;

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 2, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 3, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);
                break;
        }
    }

    private void LoadEnemySprites(SpriteRenderer sprite, AnimatedSprites aniSprites, int character, Enemy enemy) {
        Sprite[] enemySprites = new Sprite[4];
        Sprite[] vulnerableSprites = new Sprite[4];

        switch(character) {
            case 1:
                sprite.sprite = Resources.Load<Sprite>("Ina_Normal_01");

                enemySprites[0] = Resources.Load<Sprite>("Ina_Normal_01");
                enemySprites[1] = Resources.Load<Sprite>("Ina_Normal_02");
                enemySprites[2] = Resources.Load<Sprite>("Ina_Normal_03");
                enemySprites[3] = Resources.Load<Sprite>("Ina_Normal_02");

                vulnerableSprites[0] = Resources.Load<Sprite>("Ina_Vulnerable_01");
                vulnerableSprites[1] = Resources.Load<Sprite>("Ina_Vulnerable_02");
                vulnerableSprites[2] = Resources.Load<Sprite>("Ina_Vulnerable_03");
                vulnerableSprites[3] = Resources.Load<Sprite>("Ina_Vulnerable_02");

                aniSprites.sprites = enemySprites;
                enemy.regularSprites = enemySprites;
                enemy.vulnerableSprites = vulnerableSprites;
                break;
            case 2:
                sprite.sprite = Resources.Load<Sprite>("Kiara_Normal_01");

                enemySprites[0] = Resources.Load<Sprite>("Kiara_Normal_01");
                enemySprites[1] = Resources.Load<Sprite>("Kiara_Normal_02");
                enemySprites[2] = Resources.Load<Sprite>("Kiara_Normal_03");
                enemySprites[3] = Resources.Load<Sprite>("Kiara_Normal_02");

                vulnerableSprites[0] = Resources.Load<Sprite>("Kiara_Vulnerable_01");
                vulnerableSprites[1] = Resources.Load<Sprite>("Kiara_Vulnerable_02");
                vulnerableSprites[2] = Resources.Load<Sprite>("Kiara_Vulnerable_03");
                vulnerableSprites[3] = Resources.Load<Sprite>("Kiara_Vulnerable_02");

                aniSprites.sprites = enemySprites;
                enemy.regularSprites = enemySprites;
                enemy.vulnerableSprites = vulnerableSprites;
                break;
            case 3:
                sprite.sprite = Resources.Load<Sprite>("Ame_Normal_01");

                enemySprites[0] = Resources.Load<Sprite>("Ame_Normal_01");
                enemySprites[1] = Resources.Load<Sprite>("Ame_Normal_02");
                enemySprites[2] = Resources.Load<Sprite>("Ame_Normal_03");
                enemySprites[3] = Resources.Load<Sprite>("Ame_Normal_02");

                vulnerableSprites[0] = Resources.Load<Sprite>("Ame_Vulnerable_01");
                vulnerableSprites[1] = Resources.Load<Sprite>("Ame_Vulnerable_02");
                vulnerableSprites[2] = Resources.Load<Sprite>("Ame_Vulnerable_03");
                vulnerableSprites[3] = Resources.Load<Sprite>("Ame_Vulnerable_02");

                aniSprites.sprites = enemySprites;
                enemy.regularSprites = enemySprites;
                enemy.vulnerableSprites = vulnerableSprites;
                break;
            case 4:
                sprite.sprite = Resources.Load<Sprite>("Calli_Normal_01");

                enemySprites[0] = Resources.Load<Sprite>("Calli_Normal_01");
                enemySprites[1] = Resources.Load<Sprite>("Calli_Normal_02");
                enemySprites[2] = Resources.Load<Sprite>("Calli_Normal_03");
                enemySprites[3] = Resources.Load<Sprite>("Calli_Normal_02");

                vulnerableSprites[0] = Resources.Load<Sprite>("Calli_Vulnerable_01");
                vulnerableSprites[1] = Resources.Load<Sprite>("Calli_Vulnerable_02");
                vulnerableSprites[2] = Resources.Load<Sprite>("Calli_Vulnerable_03");
                vulnerableSprites[3] = Resources.Load<Sprite>("Calli_Vulnerable_02");

                aniSprites.sprites = enemySprites;
                enemy.regularSprites = enemySprites;
                enemy.vulnerableSprites = vulnerableSprites;
                break;
            case 5:
                sprite.sprite = Resources.Load<Sprite>("Gura_Normal_01");

                enemySprites[0] = Resources.Load<Sprite>("Gura_Normal_01");
                enemySprites[1] = Resources.Load<Sprite>("Gura_Normal_02");
                enemySprites[2] = Resources.Load<Sprite>("Gura_Normal_03");
                enemySprites[3] = Resources.Load<Sprite>("Gura_Normal_02");

                vulnerableSprites[0] = Resources.Load<Sprite>("Gura_Vulnerable_01");
                vulnerableSprites[1] = Resources.Load<Sprite>("Gura_Vulnerable_02");
                vulnerableSprites[2] = Resources.Load<Sprite>("Gura_Vulnerable_03");
                vulnerableSprites[3] = Resources.Load<Sprite>("Gura_Vulnerable_02");

                aniSprites.sprites = enemySprites;
                enemy.regularSprites = enemySprites;
                enemy.vulnerableSprites = vulnerableSprites;
                break;
        }
    }
}
