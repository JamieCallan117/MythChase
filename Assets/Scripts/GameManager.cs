using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Random=System.Random;

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
    public GameObject powerUpItem;
    private Enemy enemyOneAtr;
    private Enemy enemyTwoAtr;
    private Enemy enemyThreeAtr;
    private Enemy enemyFourAtr;
    private Player playerAtr;
    private PowerUp powerUpAtr;
    private SpriteRenderer playerSprite;
    private SpriteRenderer enemyOneSprite;
    private SpriteRenderer enemyTwoSprite;
    private SpriteRenderer enemyThreeSprite;
    private SpriteRenderer enemyFourSprite;
    private SpriteRenderer powerUpSprite;
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
    private int lives;
    private int powerUpID;
    private bool powerUpOwned;
    private List<Pellet> pellets = new List<Pellet>();
    private Leaderboard leaderboard;

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
        powerUpSprite = powerUpItem.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        lifeOneImage = lifeOne.GetComponent(typeof(Image)) as Image;
        lifeTwoImage = lifeTwo.GetComponent(typeof(Image)) as Image;
        lifeThreeImage = lifeThree.GetComponent(typeof(Image)) as Image;
        powerUpImage = powerUp.GetComponent(typeof(Image)) as Image;
        enemyOneAtr = enemyOne.GetComponent(typeof(Enemy)) as Enemy;
        enemyTwoAtr = enemyTwo.GetComponent(typeof(Enemy)) as Enemy;
        enemyThreeAtr = enemyThree.GetComponent(typeof(Enemy)) as Enemy;
        enemyFourAtr = enemyFour.GetComponent(typeof(Enemy)) as Enemy;
        playerAtr = player.GetComponent(typeof(Player)) as Player;
        powerUpAtr = powerUpItem.GetComponent(typeof(PowerUp)) as PowerUp;
        scoreText = scoreValue.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        audioSource = GetComponent<AudioSource>();

        powerUpOwned = false;

        SaveFile();
        //LoadFile();
    }

    private void Start() {  
        SetScore(0);
        SetLives(3);
        LoadPlayerSprites(); 
        powerUp.SetActive(false);

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

    private IEnumerator releaseEnemyTimer(int time, Enemy enemy) {
        yield return new WaitForSecondsRealtime(time);

        enemy.Release();
    }

    private void startGame() {
        Movement playerMovement = player.GetComponent(typeof(Movement)) as Movement;
        Movement enemyOneMovement = enemyOne.GetComponent(typeof(Movement)) as Movement;
        Movement enemyTwoMovement = enemyTwo.GetComponent(typeof(Movement)) as Movement;
        Movement enemyThreeMovement = enemyThree.GetComponent(typeof(Movement)) as Movement;
        Movement enemyFourMovement = enemyFour.GetComponent(typeof(Movement)) as Movement;

        playerMovement.movementEnabled = true;
        enemyOneMovement.movementEnabled = true;
        enemyTwoMovement.movementEnabled = true;
        enemyThreeMovement.movementEnabled = true;
        enemyFourMovement.movementEnabled = true;

        Random rand = new Random();
        int randInt = rand.Next(0, 2);

        if (randInt == 0) {
            enemyOneMovement.Move(Vector2.left);
            enemyThreeMovement.Move(Vector2.left);
        } else {
            enemyOneMovement.Move(Vector2.right);
            enemyThreeMovement.Move(Vector2.right);
        }

        enemyTwoMovement.Move(Vector2.right);
        enemyFourMovement.Move(Vector2.left);

        readyText.SetActive(false);

        StartCoroutine(releaseEnemyTimer(5, enemyTwoAtr));
        StartCoroutine(releaseEnemyTimer(10, enemyThreeAtr));
        StartCoroutine(releaseEnemyTimer(15, enemyFourAtr));
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

    private void SetLives(int newLives) {
        this.lives = newLives;

        switch (this.lives) {
            case 1:
                lifeOne.SetActive(true);
                lifeTwo.SetActive(false);
                lifeThree.SetActive(false);
                break;
            case 2:
                lifeOne.SetActive(true);
                lifeTwo.SetActive(true);
                lifeThree.SetActive(false);
                break;
            case 3:
                lifeOne.SetActive(true);
                lifeTwo.SetActive(true);
                lifeThree.SetActive(true);
                break;
            default:
                lifeOne.SetActive(false);
                lifeTwo.SetActive(false);
                lifeThree.SetActive(false);
                break;
        }
    }

    public PowerUp PickUpPowerUp() {
        powerUp.SetActive(true);

        powerUpOwned = true;

        return powerUpAtr;
    }

    private void VulnerableEnemies() {
        if (enemyOneAtr.eaten == false) {
            enemyOneAtr.SetVulnerable(true);
        }

        if (enemyTwoAtr.eaten == false) {
            enemyTwoAtr.SetVulnerable(true);
        }

        if (enemyThreeAtr.eaten == false) {
            enemyThreeAtr.SetVulnerable(true);
        }

        if (enemyFourAtr.eaten == false) {
            enemyFourAtr.SetVulnerable(true);
        }
    }

    public void PlayerHit() {
        Movement playerMovement = player.GetComponent(typeof(Movement)) as Movement;

        playerMovement.movementEnabled = false;

        playerAtr.ResetPosition();
        enemyOneAtr.ResetPosition();
        enemyTwoAtr.ResetPosition();
        enemyThreeAtr.ResetPosition();
        enemyFourAtr.ResetPosition();

        SetLives(this.lives - 1);
    }

    public Vector3 GetPlayerPos() {
        return player.transform.position;
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
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Kotori");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 3, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.type = 2;
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
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Bubba");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.type = 3;
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
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_DeathSensei");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 3, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.type = 4;
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
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Bloop");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 3, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 4, enemyFourAtr);

                powerUpAtr.type = 5;
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
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Takodachi");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 2, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 3, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.type = 1;
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
                enemy.eatenSprite = Resources.Load<Sprite>("Ina_Eaten");
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
                enemy.eatenSprite = Resources.Load<Sprite>("Kiara_Eaten");
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
                enemy.eatenSprite = Resources.Load<Sprite>("Ame_Eaten");
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
                enemy.eatenSprite = Resources.Load<Sprite>("Calli_Eaten");
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
                enemy.eatenSprite = Resources.Load<Sprite>("Gura_Eaten");
                break;
        }
    }

    private void SaveFile() {
        string path = Application.persistentDataPath + "/leaderboard" + PlayerStats.level + ".dat";
        FileStream file;

        if (File.Exists(path)) {
            file = File.OpenWrite(path);
        } else {
            file = File.Create(path);
        }
    }

    private void LoadFile() {
        string path = Application.persistentDataPath + "/leaderboard" + PlayerStats.level + ".dat";
        FileStream file = File.OpenRead(path);

        BinaryFormatter bf = new BinaryFormatter();
        Leaderboard lb = (Leaderboard) bf.Deserialize(file);
        file.Close();

        leaderboard = lb;
    }
}
