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
    public GameObject highScoreValue;
    public GameObject readyText;
    public GameObject powerUpItem;
    public GameObject gameOverPanel;
    public GameObject usernameObj;
    public GameObject finalScore;
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
    private TextMeshProUGUI highScoreText;
    private TextMeshProUGUI finalScoreText;
    private TMP_InputField usernameField;
    private AnimatedSprites playerAniSprites;
    private AnimatedSprites enemyOneAniSprites;
    private AnimatedSprites enemyTwoAniSprites;
    private AnimatedSprites enemyThreeAniSprites;
    private AnimatedSprites enemyFourAniSprites;
    private AudioSource audioSource;
    private int score;
    private int highScore;
    private int lives;
    private int powerUpID;
    private bool powerUpOwned;
    private bool hasHighScore;
    private List<Pellet> pellets = new List<Pellet>();
    private List<CheckPoint> telePoints = new List<CheckPoint>();
    private Leaderboard leaderboard;
    private Achievement achievements;
    public SceneChange sceneChange;

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
        highScoreText = highScoreValue.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        finalScoreText = finalScore.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        usernameField = usernameObj.GetComponent(typeof(TMP_InputField)) as TMP_InputField;
        audioSource = GetComponent<AudioSource>();

        powerUpOwned = false;
        hasHighScore = false;

        LoadFile();
        LoadAchievements();

        GetHighScore();
    }

    private void Start() {  
        SetScore(0);
        SetLives(3);
        LoadPlayerSprites(); 
        powerUp.SetActive(false);
        powerUpItem.SetActive(false);

        StartCoroutine(newRoundWait());

        audioSource.Play();
    }

    private void Update() {
        scoreText.text = score.ToString();

        if (hasHighScore == false && score > highScore) {
            hasHighScore = true;
        }

        if (hasHighScore) {
            highScoreText.text = score.ToString();
        }

        bool foundEnabledPellet = false;

        for (int i = 0; i < pellets.Count; i++) {
            if (pellets[i].gameObject.activeInHierarchy == true && foundEnabledPellet == false) {
                foundEnabledPellet = true;
                break;
            }
        }

        if (foundEnabledPellet == false) {
            NewRound();
        }
    }

    private void SpawnPowerUp() {
        Random rand = new Random();
        int randInt = rand.Next(0, 5);

        if (randInt == 0 && powerUpOwned == false) {
            powerUpItem.SetActive(true);
        }
    }

    private IEnumerator newRoundWait() {
        readyText.SetActive(true);
        yield return new WaitForSecondsRealtime(5);

        startGame();
    }

    private IEnumerator roundEndWait() {
        for (int i = 0; i < pellets.Count; i++) {
            pellets[i].gameObject.SetActive(true);
        }

        Movement playerMovement = player.GetComponent(typeof(Movement)) as Movement;
        Movement enemyOneMovement = enemyOne.GetComponent(typeof(Movement)) as Movement;
        Movement enemyTwoMovement = enemyTwo.GetComponent(typeof(Movement)) as Movement;
        Movement enemyThreeMovement = enemyThree.GetComponent(typeof(Movement)) as Movement;
        Movement enemyFourMovement = enemyFour.GetComponent(typeof(Movement)) as Movement;

        playerMovement.speed += 0.1f;
        enemyOneMovement.speed += 0.1f;
        enemyTwoMovement.speed += 0.1f;
        enemyThreeMovement.speed += 0.1f;
        enemyFourMovement.speed += 0.1f;

        int roundsCompleted;

        switch(PlayerStats.character) {
            case 2:
                roundsCompleted = (int) achievements.achievements["Kiara_Ten_Rounds"];
                achievements.updateAchievement("Kiara_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Kiara_OneHundred_Rounds", roundsCompleted + 1);
                break;
            case 3:
                roundsCompleted = (int) achievements.achievements["Ame_Ten_Rounds"];
                achievements.updateAchievement("Ame_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Ame_OneHundred_Rounds", roundsCompleted + 1);
                break;
            case 4:
                roundsCompleted = (int) achievements.achievements["Calli_Ten_Rounds"];
                achievements.updateAchievement("Calli_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Calli_OneHundred_Rounds", roundsCompleted + 1);
                break;
            case 5:
                roundsCompleted = (int) achievements.achievements["Gura_Ten_Rounds"];
                achievements.updateAchievement("Gura_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Gura_OneHundred_Rounds",  + roundsCompleted + 1);
                break;
            default:
                roundsCompleted = (int) achievements.achievements["Ina_Ten_Rounds"];
                achievements.updateAchievement("Ina_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Ina_OneHundred_Rounds", roundsCompleted + 1);

                break;
        }

        yield return new WaitForSecondsRealtime(2);

        StartCoroutine(newRoundWait());
    }

    private void ReleaseEnemyTwoStart() {
        enemyTwoAtr.Release();
    }

    private void ReleaseEnemyThreeStart() {
        enemyThreeAtr.Release();
    }

    private void ReleaseEnemyFourStart() {
        enemyFourAtr.Release();
    }

    public void ReleaseEnemy(Enemy enemyToRelease) {
        if (enemyToRelease == enemyOneAtr) {
            Invoke("ReleaseEnemyOne", 5.0f);
        } else if (enemyToRelease == enemyTwoAtr) {
            Invoke("ReleaseEnemyTwo", 5.0f);
        } else if (enemyToRelease == enemyThreeAtr) {
            Invoke("ReleaseEnemyThree", 5.0f);
        } else if (enemyToRelease == enemyFourAtr) {
            Invoke("ReleaseEnemyFour", 5.0f);
        }
    }

    private void ReleaseEnemyOne() {
        enemyOneAtr.Release();
        enemyOneAtr.ToggleIgnorePlayer(false, player);
    }

    private void ReleaseEnemyTwo() {
        enemyTwoAtr.Release();
        enemyTwoAtr.ToggleIgnorePlayer(false, player);
    }

    private void ReleaseEnemyThree() {
        enemyThreeAtr.Release();
        enemyThreeAtr.ToggleIgnorePlayer(false, player);
    }

    private void ReleaseEnemyFour() {
        enemyFourAtr.Release();
        enemyFourAtr.ToggleIgnorePlayer(false, player);
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

        Invoke("ReleaseEnemyTwoStart", 5.0f);
        Invoke("ReleaseEnemyThreeStart", 10.0f);
        Invoke("ReleaseEnemyFourStart", 15.0f);

        InvokeRepeating("SpawnPowerUp", 0.0f, 10.0f);
    }

    private void NewRound() {
        Movement playerMovement = player.GetComponent(typeof(Movement)) as Movement;
        Movement enemyOneMovement = enemyOne.GetComponent(typeof(Movement)) as Movement;
        Movement enemyTwoMovement = enemyTwo.GetComponent(typeof(Movement)) as Movement;
        Movement enemyThreeMovement = enemyThree.GetComponent(typeof(Movement)) as Movement;
        Movement enemyFourMovement = enemyFour.GetComponent(typeof(Movement)) as Movement;

        playerMovement.movementEnabled = false;
        enemyOneMovement.movementEnabled = false;
        enemyTwoMovement.movementEnabled = false;
        enemyThreeMovement.movementEnabled = false;
        enemyFourMovement.movementEnabled = false;

        playerMovement.currentDirection = Vector2.zero;
        playerMovement.nextDirection = Vector2.zero;
        enemyOneMovement.currentDirection = Vector2.zero;
        enemyOneMovement.nextDirection = Vector2.zero;
        enemyTwoMovement.currentDirection = Vector2.zero;
        enemyTwoMovement.nextDirection = Vector2.zero;
        enemyThreeMovement.currentDirection = Vector2.zero;
        enemyThreeMovement.nextDirection = Vector2.zero;
        enemyFourMovement.currentDirection = Vector2.zero;
        enemyFourMovement.nextDirection = Vector2.zero;

        readyText.SetActive(true);

        enemyOneAtr.ResetEnemy();
        enemyOneAtr.inHome = false;
        enemyOneAtr.ToggleIgnorePlayer(false, player);

        enemyTwoAtr.ResetEnemy();
        enemyTwoAtr.inHome = true;
        enemyTwoAtr.ToggleIgnorePlayer(false, player);

        enemyThreeAtr.ResetEnemy();
        enemyThreeAtr.inHome = true;
        enemyThreeAtr.ToggleIgnorePlayer(false, player);

        enemyFourAtr.ResetEnemy();
        enemyFourAtr.inHome = true;
        enemyFourAtr.ToggleIgnorePlayer(false, player);

        player.transform.position = new Vector3(0, -3.5f, -5.0f);
        enemyOne.transform.position = new Vector3(0, 2.5f, -5.0f);
        enemyTwo.transform.position = new Vector3(-2, -0.5f, -5.0f);
        enemyThree.transform.position = new Vector3(0, -0.5f, -5.0f);
        enemyFour.transform.position = new Vector3(2, -0.5f, -5.0f);

        CancelInvoke("ReleaseEnemyTwoStart");
        CancelInvoke("ReleaseEnemyThreeStart");
        CancelInvoke("ReleaseEnemyFourStart");
        CancelInvoke("ReleaseEnemyOne");
        CancelInvoke("ReleaseEnemyTwo");
        CancelInvoke("ReleaseEnemyThree");
        CancelInvoke("ReleaseEnemyFour");
        CancelInvoke("EndInvincibility");
        CancelInvoke("ResumeTime");
        CancelInvoke("EndVulnerableEnemies");
        CancelInvoke("UnscareEnemies");
        CancelInvoke("SpawnPowerUp");

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

                StartCoroutine(newRoundWait());
                break;
            case 2:
                lifeOne.SetActive(true);
                lifeTwo.SetActive(true);
                lifeThree.SetActive(false);

                StartCoroutine(newRoundWait());
                break;
            case 3:
                lifeOne.SetActive(true);
                lifeTwo.SetActive(true);
                lifeThree.SetActive(true);

                StartCoroutine(newRoundWait());
                break;
            default:
                lifeOne.SetActive(false);
                lifeTwo.SetActive(false);
                lifeThree.SetActive(false);

                GameOver();
                break;
        }
    }

    public PowerUp PickUpPowerUp() {
        powerUp.SetActive(true);
        powerUpItem.SetActive(false);

        powerUpOwned = true;

        return powerUpAtr;
    }

    public bool HasPowerUp() {
        return powerUpOwned;
    }

    public void AddTeleportPoint(CheckPoint newPoint) {
        telePoints.Add(newPoint);
    }

    public void UsePowerUp(int type) {
        powerUp.SetActive(false);

        powerUpOwned = false;

        int powerUpsUsed = (int) achievements.achievements["Use_PowerUp_One"];

        achievements.updateAchievement("Use_PowerUp_One", powerUpsUsed + 1);
        achievements.updateAchievement("Use_PowerUp_Ten", powerUpsUsed + 1);
        achievements.updateAchievement("Use_PowerUp_OneHundred", powerUpsUsed + 1);

        switch(type) {
            case 1:
                Random rand = new Random();
                int randInt = rand.Next(0, telePoints.Count);
                Movement playerMovement = player.GetComponent(typeof(Movement)) as Movement;

                player.transform.position = telePoints[randInt].transform.position;
                playerMovement.currentDirection = (Vector2.zero);
                playerMovement.nextDirection = (Vector2.zero);
                break;
            case 2:
                enemyOneAtr.ToggleIgnorePlayer(true, player);
                enemyTwoAtr.ToggleIgnorePlayer(true, player);
                enemyThreeAtr.ToggleIgnorePlayer(true, player);
                enemyFourAtr.ToggleIgnorePlayer(true, player);

                Invoke("EndInvincibility", 10.0f);
                break;
            case 3:
                Movement enemyOneMovement = enemyOne.GetComponent(typeof(Movement)) as Movement;
                Movement enemyTwoMovement = enemyTwo.GetComponent(typeof(Movement)) as Movement;
                Movement enemyThreeMovement = enemyThree.GetComponent(typeof(Movement)) as Movement;
                Movement enemyFourMovement = enemyFour.GetComponent(typeof(Movement)) as Movement;

                enemyOneMovement.movementEnabled = false;
                enemyTwoMovement.movementEnabled = false;
                enemyThreeMovement.movementEnabled = false;
                enemyFourMovement.movementEnabled = false;

                Invoke("ResumeTime", 10.0f);
                break;
            case 4:
                enemyOneAtr.powerUpVulnerable = true;
                enemyTwoAtr.powerUpVulnerable = true;
                enemyThreeAtr.powerUpVulnerable = true;
                enemyFourAtr.powerUpVulnerable = true;

                Invoke("EndVulnerableEnemies", 10.0f);
                break;
            case 5:
                enemyOneAtr.scared = true;
                enemyTwoAtr.scared = true;
                enemyThreeAtr.scared = true;
                enemyFourAtr.scared = true;

                Invoke("UnscareEnemies", 10.0f);
                break;
            default:
                sceneChange.moveToScene(0);
                break;
        }
    }

    private void EndInvincibility() {
        enemyOneAtr.ToggleIgnorePlayer(false, player);
        enemyTwoAtr.ToggleIgnorePlayer(false, player);
        enemyThreeAtr.ToggleIgnorePlayer(false, player);
        enemyFourAtr.ToggleIgnorePlayer(false, player);
    }

    private void ResumeTime() {
        Movement enemyOneMovement = enemyOne.GetComponent(typeof(Movement)) as Movement;
        Movement enemyTwoMovement = enemyTwo.GetComponent(typeof(Movement)) as Movement;
        Movement enemyThreeMovement = enemyThree.GetComponent(typeof(Movement)) as Movement;
        Movement enemyFourMovement = enemyFour.GetComponent(typeof(Movement)) as Movement;

        enemyOneMovement.movementEnabled = true;
        enemyTwoMovement.movementEnabled = true;
        enemyThreeMovement.movementEnabled = true;
        enemyFourMovement.movementEnabled = true;
    }

    private void EndVulnerableEnemies() {
        enemyOneAtr.powerUpVulnerable = false;
        enemyTwoAtr.powerUpVulnerable = false;
        enemyThreeAtr.powerUpVulnerable = false;
        enemyFourAtr.powerUpVulnerable = false;
    }

    private void UnscareEnemies() {
        enemyOneAtr.scared = false;
        enemyTwoAtr.scared = false;
        enemyThreeAtr.scared = false;
        enemyFourAtr.scared = false;
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
        Movement enemyOneMovement = enemyOne.GetComponent(typeof(Movement)) as Movement;
        Movement enemyTwoMovement = enemyTwo.GetComponent(typeof(Movement)) as Movement;
        Movement enemyThreeMovement = enemyThree.GetComponent(typeof(Movement)) as Movement;
        Movement enemyFourMovement = enemyFour.GetComponent(typeof(Movement)) as Movement;

        playerMovement.movementEnabled = false;
        enemyOneMovement.movementEnabled = false;
        enemyTwoMovement.movementEnabled = false;
        enemyThreeMovement.movementEnabled = false;
        enemyFourMovement.movementEnabled = false;

        playerMovement.currentDirection = Vector2.zero;
        playerMovement.nextDirection = Vector2.zero;
        enemyOneMovement.currentDirection = Vector2.zero;
        enemyOneMovement.nextDirection = Vector2.zero;
        enemyTwoMovement.currentDirection = Vector2.zero;
        enemyTwoMovement.nextDirection = Vector2.zero;
        enemyThreeMovement.currentDirection = Vector2.zero;
        enemyThreeMovement.nextDirection = Vector2.zero;
        enemyFourMovement.currentDirection = Vector2.zero;
        enemyFourMovement.nextDirection = Vector2.zero;

        enemyOne.SetActive(false);
        enemyTwo.SetActive(false);
        enemyThree.SetActive(false);
        enemyFour.SetActive(false);

        CancelInvoke("ReleaseEnemyTwoStart");
        CancelInvoke("ReleaseEnemyThreeStart");
        CancelInvoke("ReleaseEnemyFourStart");
        CancelInvoke("ReleaseEnemyOne");
        CancelInvoke("ReleaseEnemyTwo");
        CancelInvoke("ReleaseEnemyThree");
        CancelInvoke("ReleaseEnemyFour");
        CancelInvoke("EndInvincibility");
        CancelInvoke("ResumeTime");
        CancelInvoke("EndVulnerableEnemies");
        CancelInvoke("UnscareEnemies");
        CancelInvoke("SpawnPowerUp");

        playerAtr.aniSprites.enable(false);

        playerAtr.KillPlayer();

        Invoke("ResumePlay", 3.5f);
    }

    private void ResumePlay() {
        enemyOne.SetActive(true);
        enemyTwo.SetActive(true);
        enemyThree.SetActive(true);
        enemyFour.SetActive(true);

        playerAtr.aniSprites.loop = true;
        playerAtr.aniSprites.UpdateAnimationRate(0.05f);
        playerAtr.aniSprites.sprites = playerAtr.regularSprites;

        enemyOneAtr.ResetEnemy();
        enemyOneAtr.inHome = false;
        enemyOneAtr.ToggleIgnorePlayer(false, player);

        enemyTwoAtr.ResetEnemy();
        enemyTwoAtr.inHome = true;
        enemyTwoAtr.ToggleIgnorePlayer(false, player);

        enemyThreeAtr.ResetEnemy();
        enemyThreeAtr.inHome = true;
        enemyThreeAtr.ToggleIgnorePlayer(false, player);

        enemyFourAtr.ResetEnemy();
        enemyFourAtr.inHome = true;
        enemyFourAtr.ToggleIgnorePlayer(false, player);

        player.transform.position = new Vector3(0, -3.5f, -5.0f);
        enemyOne.transform.position = new Vector3(0, 2.5f, -5.0f);
        enemyTwo.transform.position = new Vector3(-2, -0.5f, -5.0f);
        enemyThree.transform.position = new Vector3(0, -0.5f, -5.0f);
        enemyFour.transform.position = new Vector3(2, -0.5f, -5.0f);

        SetLives(this.lives - 1);
    }

    public Vector3 GetPlayerPos() {
        return player.transform.position;
    }

    private void GameOver() {
        player.SetActive(false);
        enemyOne.SetActive(false);
        enemyTwo.SetActive(false);
        enemyThree.SetActive(false);
        enemyFour.SetActive(false);
        powerUpItem.SetActive(false);
        CancelInvoke("SpawnPowerUp");
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Score: " + score.ToString();
    }

    private void LoadPlayerSprites() {
        Sprite[] playerSprites = new Sprite[4];
        Sprite[] deathSprites = new Sprite[5];

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

                deathSprites[0] = Resources.Load<Sprite>("Kiara_Death_01");
                deathSprites[1] = Resources.Load<Sprite>("Kiara_Death_02");
                deathSprites[2] = Resources.Load<Sprite>("Kiara_Death_03");
                deathSprites[3] = Resources.Load<Sprite>("Kiara_Death_04");
                deathSprites[4] = Resources.Load<Sprite>("Kiara_Death_05");

                playerAniSprites.sprites = playerSprites;
                playerAtr.regularSprites = playerSprites;
                playerAtr.deathSprites = deathSprites;
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

                deathSprites[0] = Resources.Load<Sprite>("Ame_Death_01");
                deathSprites[1] = Resources.Load<Sprite>("Ame_Death_02");
                deathSprites[2] = Resources.Load<Sprite>("Ame_Death_03");
                deathSprites[3] = Resources.Load<Sprite>("Ame_Death_04");
                deathSprites[4] = Resources.Load<Sprite>("Ame_Death_05");

                playerAniSprites.sprites = playerSprites;
                playerAtr.regularSprites = playerSprites;
                playerAtr.deathSprites = deathSprites;
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

                deathSprites[0] = Resources.Load<Sprite>("Calli_Death_01");
                deathSprites[1] = Resources.Load<Sprite>("Calli_Death_02");
                deathSprites[2] = Resources.Load<Sprite>("Calli_Death_03");
                deathSprites[3] = Resources.Load<Sprite>("Calli_Death_04");
                deathSprites[4] = Resources.Load<Sprite>("Calli_Death_05");

                playerAniSprites.sprites = playerSprites;
                playerAtr.regularSprites = playerSprites;
                playerAtr.deathSprites = deathSprites;
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

                deathSprites[0] = Resources.Load<Sprite>("Gura_Death_01");
                deathSprites[1] = Resources.Load<Sprite>("Gura_Death_02");
                deathSprites[2] = Resources.Load<Sprite>("Gura_Death_03");
                deathSprites[3] = Resources.Load<Sprite>("Gura_Death_04");
                deathSprites[4] = Resources.Load<Sprite>("Gura_Death_05");

                playerAniSprites.sprites = playerSprites;
                playerAtr.regularSprites = playerSprites;
                playerAtr.deathSprites = deathSprites;
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

                deathSprites[0] = Resources.Load<Sprite>("Ina_Death_01");
                deathSprites[1] = Resources.Load<Sprite>("Ina_Death_02");
                deathSprites[2] = Resources.Load<Sprite>("Ina_Death_03");
                deathSprites[3] = Resources.Load<Sprite>("Ina_Death_04");
                deathSprites[4] = Resources.Load<Sprite>("Ina_Death_05");

                playerAniSprites.sprites = playerSprites;
                playerAtr.regularSprites = playerSprites;
                playerAtr.deathSprites = deathSprites;
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

    public void SaveScore() {
        string username = usernameField.text;
        
        if (leaderboard == null) {
            leaderboard = new Leaderboard();
        }

        if (leaderboard.leaderboard[username] == null) {
            leaderboard.addScore(username, score);
        } else {
            leaderboard.updateScore(username, score);
        }

        SaveFile();
        SaveAchievements();
        sceneChange.moveToScene(0);
    }

    public void DontSaveScore() {
        SaveAchievements();
        sceneChange.moveToScene(0);
    }

    private void GetHighScore() {
        int highestScore = 0;

        if (leaderboard != null) {
            foreach (int lbScore in leaderboard.leaderboard.Values) {
                if (lbScore > highestScore) {
                    highestScore = lbScore;
                }
            }
        } else {
            hasHighScore = true;
        }

        highScore = highestScore;

        highScoreText.text = highScore.ToString();
    }

    private void SaveFile() {
        string path = Application.persistentDataPath + "/leaderboard" + PlayerStats.level + ".dat";
        FileStream file;

        if (File.Exists(path)) {
            file = File.OpenWrite(path);
        } else {
            file = File.Create(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, leaderboard);

        file.Close();
    }

    private void LoadFile() {
        string path = Application.persistentDataPath + "/leaderboard" + PlayerStats.level + ".dat";
        FileStream file;

        if (File.Exists(path)) {
            file = File.OpenRead(path);

            if (file.Length > 0) {
                BinaryFormatter bf = new BinaryFormatter();
                Leaderboard lb = (Leaderboard) bf.Deserialize(file);

                leaderboard = lb;

                file.Close();
            }
        } else {
            leaderboard = null;
        }
    }

    private void SaveAchievements() {
        string path = Application.persistentDataPath + "/achievements.dat";
        FileStream file;

        if (File.Exists(path)) {
            file = File.OpenWrite(path);
        } else {
            file = File.Create(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, achievements);

        file.Close();
    }

    private void LoadAchievements() {
        string path = Application.persistentDataPath + "/achievements.dat";
        FileStream file;

        if (File.Exists(path)) {
            file = File.OpenRead(path);

            if (file.Length > 0) {
                BinaryFormatter bf = new BinaryFormatter();
                Achievement ach = (Achievement) bf.Deserialize(file);

                achievements = ach;

                file.Close();
            }
        } else {
            achievements = new Achievement();

            CreateAchievements();
        }
    }

    private void CreateAchievements() {
        achievements.createAchievement("Ina_Ten_Rounds", 0);
        achievements.createAchievement("Ina_OneHundred_Rounds", 0);
        achievements.createAchievement("Kiara_Ten_Rounds", 0);
        achievements.createAchievement("Kiara_OneHundred_Rounds", 0);
        achievements.createAchievement("Ame_Ten_Rounds", 0);
        achievements.createAchievement("Ame_OneHundred_Rounds", 0);
        achievements.createAchievement("Calli_Ten_Rounds", 0);
        achievements.createAchievement("Calli_OneHundred_Rounds", 0);
        achievements.createAchievement("Gura_Ten_Rounds", 0);
        achievements.createAchievement("Gura_OneHundred_Rounds", 0);
        achievements.createAchievement("Use_PowerUp_One", 0);
        achievements.createAchievement("Use_PowerUp_Ten", 0);
        achievements.createAchievement("Use_PowerUp_OneHundred", 0);

        SaveAchievements();
    }
}
