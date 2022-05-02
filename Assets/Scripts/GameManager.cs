using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Random=System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyOne;
    [SerializeField] private GameObject enemyTwo;
    [SerializeField] private GameObject enemyThree;
    [SerializeField] private GameObject enemyFour;
    [SerializeField] private GameObject lifeOne;
    [SerializeField] private GameObject lifeTwo;
    [SerializeField] private GameObject lifeThree;
    [SerializeField] private GameObject powerUp;
    [SerializeField] private GameObject scoreValue;
    [SerializeField] private GameObject highScoreValue;
    [SerializeField] private GameObject readyText;
    [SerializeField] private GameObject powerUpItem;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject usernameObj;
    [SerializeField] private GameObject finalScore;
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
    [SerializeField] private PlayAudio playAudio;
    private int score;
    private int highScore;
    private int lives;
    private int powerUpID;
    private int scoreLives;
    private bool powerUpOwned;
    private bool hasHighScore;
    private List<Pellet> pellets = new List<Pellet>();
    private List<CheckPoint> telePoints = new List<CheckPoint>();
    private Leaderboard leaderboard;
    private Achievement achievements;
    [SerializeField] private SceneChange sceneChange;

    void Awake()
    {
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

        powerUpOwned = false;
        hasHighScore = false;

        scoreLives = 0;

        LoadFile();
        LoadAchievements();

        GetHighScore();
    }

    void Start()
    {  
        SetScore(0);
        SetLives(3);
        LoadPlayerSprites(); 
        powerUp.SetActive(false);
        powerUpItem.SetActive(false);

        playerAtr.SetSpeed(8.0f);
        enemyOneAtr.SetSpeed(7.0f);
        enemyTwoAtr.SetSpeed(7.0f);
        enemyThreeAtr.SetSpeed(7.0f);
        enemyFourAtr.SetSpeed(7.0f);

        StartCoroutine(newRoundWait());

        playAudio.PlayIntroMusic();
    }

    void Update()
    {
        scoreText.text = score.ToString();

        if (hasHighScore == false && score > highScore)
        {
            hasHighScore = true;
        }

        if (hasHighScore)
        {
            highScoreText.text = score.ToString();
        }

        bool foundEnabledPellet = false;

        for (int i = 0; i < pellets.Count; i++)
        {
            if (pellets[i].gameObject.activeInHierarchy == true && foundEnabledPellet == false)
            {
                foundEnabledPellet = true;
                break;
            }
        }

        if (foundEnabledPellet == false)
        {
            NewRound();
        }

        if ((score - scoreLives) >= 1000 && lives < 3 && lives != 0)
        {
            SetLives(lives + 1);

            scoreLives = score;
        }
    }

    private IEnumerator newRoundWait()
    {
        readyText.SetActive(true);
        yield return new WaitForSecondsRealtime(5);

        startGame();
    }

    private IEnumerator roundEndWait()
    {
        for (int i = 0; i < pellets.Count; i++)
        {
            pellets[i].gameObject.SetActive(true);
        }

        playerAtr.IncreaseSpeed();
        enemyOneAtr.IncreaseSpeed();
        enemyTwoAtr.IncreaseSpeed();
        enemyThreeAtr.IncreaseSpeed();
        enemyFourAtr.IncreaseSpeed();

        int roundsCompleted;

        switch(PlayerStats.character)
        {
            case 2:
                roundsCompleted = achievements.GetAchievement("Kiara_Ten_Rounds");
                achievements.updateAchievement("Kiara_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Kiara_OneHundred_Rounds", roundsCompleted + 1);
                break;
            case 3:
                roundsCompleted = achievements.GetAchievement("Ame_Ten_Rounds");
                achievements.updateAchievement("Ame_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Ame_OneHundred_Rounds", roundsCompleted + 1);
                break;
            case 4:
                roundsCompleted = achievements.GetAchievement("Calli_Ten_Rounds");
                achievements.updateAchievement("Calli_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Calli_OneHundred_Rounds", roundsCompleted + 1);
                break;
            case 5:
                roundsCompleted = achievements.GetAchievement("Gura_Ten_Rounds");
                achievements.updateAchievement("Gura_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Gura_OneHundred_Rounds",  + roundsCompleted + 1);
                break;
            default:
                roundsCompleted = achievements.GetAchievement("Ina_Ten_Rounds");
                achievements.updateAchievement("Ina_Ten_Rounds", roundsCompleted + 1);
                achievements.updateAchievement("Ina_OneHundred_Rounds", roundsCompleted + 1);

                break;
        }

        yield return new WaitForSecondsRealtime(2);

        StartCoroutine(newRoundWait());
    }

    private void startGame()
    {
        playerAtr.ToggleMovement(true);
        enemyOneAtr.ToggleMovement(true);
        enemyTwoAtr.ToggleMovement(true);
        enemyThreeAtr.ToggleMovement(true);
        enemyFourAtr.ToggleMovement(true);

        enemyOneAtr.InHome(false);
        enemyTwoAtr.InHome(true);
        enemyThreeAtr.InHome(true);
        enemyFourAtr.InHome(true);

        Random rand = new Random();
        int randInt = rand.Next(0, 2);

        if (randInt == 0)
        {
            enemyOneAtr.Move(Vector2.left);
            enemyThreeAtr.Move(Vector2.left);
        }
        else
        {
            enemyOneAtr.Move(Vector2.right);
            enemyThreeAtr.Move(Vector2.right);
        }

        enemyTwoAtr.Move(Vector2.right);
        enemyFourAtr.Move(Vector2.left);

        readyText.SetActive(false);

        Invoke("ReleaseEnemyTwoStart", 5.0f);
        Invoke("ReleaseEnemyThreeStart", 10.0f);
        Invoke("ReleaseEnemyFourStart", 15.0f);

        InvokeRepeating("SpawnPowerUp", 0.0f, 10.0f);
    }

    public void AddPellet(Pellet pellet)
    {
        pellets.Add(pellet);
    }

    private void SetScore(int newScore)
    {
        score = newScore;
    }

    private void SetLives(int newLives)
    {
        lives = newLives;

        switch (lives)
        {
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

                GameOver();
                break;
        }
    }

    private void SpawnPowerUp()
    {
        Random rand = new Random();
        int randInt = rand.Next(0, 5);

        if (randInt == 0 && powerUpOwned == false)
        {
            powerUpItem.SetActive(true);
        }
    }

    private void ReleaseEnemyTwoStart()
    {
        enemyTwoAtr.Release();
    }

    private void ReleaseEnemyThreeStart()
    {
        enemyThreeAtr.Release();
    }

    private void ReleaseEnemyFourStart()
    {
        enemyFourAtr.Release();
    }

    public void ReleaseEnemy(Enemy enemyToRelease)
    {
        if (enemyToRelease == enemyOneAtr)
        {
            Invoke("ReleaseEnemyOne", 5.0f);
        } else if (enemyToRelease == enemyTwoAtr)
        {
            Invoke("ReleaseEnemyTwo", 5.0f);
        } else if (enemyToRelease == enemyThreeAtr)
        {
            Invoke("ReleaseEnemyThree", 5.0f);
        } else if (enemyToRelease == enemyFourAtr)
        {
            Invoke("ReleaseEnemyFour", 5.0f);
        }
    }

    private void ReleaseEnemyOne()
    {
        enemyOneAtr.Release();
        enemyOneAtr.ToggleIgnorePlayer(false, player);
    }

    private void ReleaseEnemyTwo()
    {
        enemyTwoAtr.Release();
        enemyTwoAtr.ToggleIgnorePlayer(false, player);
    }

    private void ReleaseEnemyThree()
    {
        enemyThreeAtr.Release();
        enemyThreeAtr.ToggleIgnorePlayer(false, player);
    }

    private void ReleaseEnemyFour()
    {
        enemyFourAtr.Release();
        enemyFourAtr.ToggleIgnorePlayer(false, player);
    }

    public void EnemyEaten()
    {
        score += 250;
    }

    public void EatPellet(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.GetPoints());
    }

    public void EatLargePellet(LargePellet pellet)
    {
        EatPellet(pellet);

        VulnerableEnemies();
    }

    private void VulnerableEnemies()
    {
        enemyOneAtr.SetVulnerable(true);
        enemyTwoAtr.SetVulnerable(true);
        enemyThreeAtr.SetVulnerable(true);
        enemyFourAtr.SetVulnerable(true);
    }

    public PowerUp PickUpPowerUp()
    {
        powerUp.SetActive(true);
        powerUpItem.SetActive(false);

        powerUpOwned = true;

        return powerUpAtr;
    }

    public bool HasPowerUp()
    {
        return powerUpOwned;
    }

    public void UsePowerUp(int type)
    {
        powerUp.SetActive(false);

        powerUpOwned = false;

        int powerUpsUsed = achievements.GetAchievement("Use_PowerUp_One");

        achievements.updateAchievement("Use_PowerUp_One", powerUpsUsed + 1);
        achievements.updateAchievement("Use_PowerUp_Ten", powerUpsUsed + 1);
        achievements.updateAchievement("Use_PowerUp_OneHundred", powerUpsUsed + 1);

        switch(type)
        {
            case 1:
                Random rand = new Random();
                int randInt = rand.Next(0, telePoints.Count);
                
                playerAtr.HaltMovement();

                playerAtr.TeleportObject(telePoints[randInt].transform.position);

                break;
            case 2:
                enemyOneAtr.ToggleIgnorePlayer(true, player);
                enemyTwoAtr.ToggleIgnorePlayer(true, player);
                enemyThreeAtr.ToggleIgnorePlayer(true, player);
                enemyFourAtr.ToggleIgnorePlayer(true, player);

                Invoke("EndInvincibility", 10.0f);
                break;
            case 3:
                enemyOneAtr.ToggleMovement(false);
                enemyTwoAtr.ToggleMovement(false);
                enemyThreeAtr.ToggleMovement(false);
                enemyFourAtr.ToggleMovement(false);

                Invoke("ResumeTime", 10.0f);
                break;
            case 4:
                enemyOneAtr.SetPowerUpVulnerable(true);
                enemyTwoAtr.SetPowerUpVulnerable(true);
                enemyThreeAtr.SetPowerUpVulnerable(true);
                enemyFourAtr.SetPowerUpVulnerable(true);

                Invoke("EndVulnerableEnemies", 10.0f);
                break;
            case 5:
                enemyOneAtr.SetScared(true);
                enemyTwoAtr.SetScared(true);
                enemyThreeAtr.SetScared(true);
                enemyFourAtr.SetScared(true);

                Invoke("UnscareEnemies", 10.0f);
                break;
            default:
                sceneChange.moveToScene(0);
                break;
        }
    }

    public void AddTeleportPoint(CheckPoint newPoint)
    {
        telePoints.Add(newPoint);
    }

    

    private void EndInvincibility()
    {
        enemyOneAtr.ToggleIgnorePlayer(false, player);
        enemyTwoAtr.ToggleIgnorePlayer(false, player);
        enemyThreeAtr.ToggleIgnorePlayer(false, player);
        enemyFourAtr.ToggleIgnorePlayer(false, player);
    }

    private void ResumeTime()
    {
        enemyOneAtr.ToggleMovement(true);
        enemyTwoAtr.ToggleMovement(true);
        enemyThreeAtr.ToggleMovement(true);
        enemyFourAtr.ToggleMovement(true);
    }

    private void EndVulnerableEnemies()
    {
        enemyOneAtr.SetPowerUpVulnerable(false);
        enemyTwoAtr.SetPowerUpVulnerable(false);
        enemyThreeAtr.SetPowerUpVulnerable(false);
        enemyFourAtr.SetPowerUpVulnerable(false);
    }

    private void UnscareEnemies()
    {
        enemyOneAtr.SetScared(false);
        enemyTwoAtr.SetScared(false);
        enemyThreeAtr.SetScared(false);
        enemyFourAtr.SetScared(false);
    }

    public void PlayerHit()
    {
        playerAtr.ToggleMovement(false);
        enemyOneAtr.ToggleMovement(false);
        enemyTwoAtr.ToggleMovement(false);
        enemyThreeAtr.ToggleMovement(false);
        enemyFourAtr.ToggleMovement(false);

        playerAtr.HaltMovement();
        enemyOneAtr.HaltMovement();
        enemyTwoAtr.HaltMovement();
        enemyThreeAtr.HaltMovement();
        enemyFourAtr.HaltMovement();

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

        playerAtr.KillPlayer();

        Invoke("ResumePlay", 3.5f);
    }

    private void ResumePlay()
    {
        enemyOne.SetActive(true);
        enemyTwo.SetActive(true);
        enemyThree.SetActive(true);
        enemyFour.SetActive(true);

        playerAtr.RevivePlayer();

        enemyOneAtr.ResetEnemy();
        enemyOneAtr.InHome(false);
        enemyOneAtr.ToggleIgnorePlayer(false, player);

        enemyTwoAtr.ResetEnemy();
        enemyTwoAtr.InHome(true);
        enemyTwoAtr.ToggleIgnorePlayer(false, player);

        enemyThreeAtr.ResetEnemy();
        enemyThreeAtr.InHome(true);
        enemyThreeAtr.ToggleIgnorePlayer(false, player);

        enemyFourAtr.ResetEnemy();
        enemyFourAtr.InHome(true);
        enemyFourAtr.ToggleIgnorePlayer(false, player);

        playerAtr.ResetPosition();
        enemyOneAtr.ResetPosition();
        enemyTwoAtr.ResetPosition();
        enemyThreeAtr.ResetPosition();
        enemyFourAtr.ResetPosition();

        SetLives(lives - 1);

        StartCoroutine(newRoundWait());
    }

    private void NewRound() {
        playerAtr.ToggleMovement(false);
        enemyOneAtr.ToggleMovement(false);
        enemyTwoAtr.ToggleMovement(false);
        enemyThreeAtr.ToggleMovement(false);
        enemyFourAtr.ToggleMovement(false);

        playerAtr.HaltMovement();
        enemyOneAtr.HaltMovement();
        enemyTwoAtr.HaltMovement();
        enemyThreeAtr.HaltMovement();
        enemyFourAtr.HaltMovement();

        readyText.SetActive(true);

        enemyOneAtr.ResetEnemy();
        enemyOneAtr.InHome(false);
        enemyOneAtr.ToggleIgnorePlayer(false, player);

        enemyTwoAtr.ResetEnemy();
        enemyTwoAtr.InHome(true);
        enemyTwoAtr.ToggleIgnorePlayer(false, player);

        enemyThreeAtr.ResetEnemy();
        enemyThreeAtr.InHome(true);
        enemyThreeAtr.ToggleIgnorePlayer(false, player);

        enemyFourAtr.ResetEnemy();
        enemyFourAtr.InHome(true);
        enemyFourAtr.ToggleIgnorePlayer(false, player);

        playerAtr.ResetPosition();
        enemyOneAtr.ResetPosition();
        enemyTwoAtr.ResetPosition();
        enemyThreeAtr.ResetPosition();
        enemyFourAtr.ResetPosition();

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

    private void GameOver()
    {
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

    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }

    public void SaveScore()
    {
        string username = usernameField.text;
        
        if (leaderboard == null)
        {
            leaderboard = new Leaderboard();
        }

        if (leaderboard.GetScore(name) == 0)
        {
            leaderboard.addScore(username, score);
        }
        else
        {
            leaderboard.updateScore(username, score);
        }

        SaveFile();
        SaveAchievements();
        sceneChange.moveToScene(0);
    }

    public void ExitWithoutSave()
    {
        SaveAchievements();
        sceneChange.moveToScene(0);
    }

    private void GetHighScore()
    {
        int highestScore = 0;

        if (leaderboard != null)
        {
            foreach (int lbScore in leaderboard.GetLeaderboards().Values)
            {
                if (lbScore > highestScore)
                {
                    highestScore = lbScore;
                }
            }
        }
        else
        {
            hasHighScore = true;
        }

        highScore = highestScore;

        highScoreText.text = highScore.ToString();
    }

    private void SaveFile()
    {
        string path = Application.persistentDataPath + "/leaderboard" + PlayerStats.level + ".dat";
        FileStream file;

        if (File.Exists(path))
        {
            file = File.OpenWrite(path);
        }
        else
        {
            file = File.Create(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, leaderboard);

        file.Close();
    }

    private void LoadFile()
    {
        string path = Application.persistentDataPath + "/leaderboard" + PlayerStats.level + ".dat";
        FileStream file;

        if (File.Exists(path))
        {
            file = File.OpenRead(path);

            if (file.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                leaderboard = (Leaderboard) bf.Deserialize(file);

                file.Close();
            }
        }
        else
        {
            leaderboard = null;
        }
    }

    private void SaveAchievements()
    {
        string path = Application.persistentDataPath + "/achievements.dat";
        FileStream file;

        if (File.Exists(path))
        {
            file = File.OpenWrite(path);
        }
        else
        {
            file = File.Create(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, achievements);

        file.Close();
    }

    private void LoadAchievements()
    {
        string path = Application.persistentDataPath + "/achievements.dat";
        FileStream file;

        if (File.Exists(path))
        {
            file = File.OpenRead(path);

            if (file.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                achievements = (Achievement) bf.Deserialize(file);

                file.Close();
            }
        }
        else
        {
            achievements = new Achievement();

            CreateAchievements();
        }
    }

    private void CreateAchievements()
    {
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

    private void LoadPlayerSprites()
    {
        Sprite[] playerSprites = new Sprite[4];
        Sprite[] deathSprites = new Sprite[5];

        switch(PlayerStats.character)
        {
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

                playerAniSprites.SetSprites(playerSprites);
                playerAtr.SetRegularSprites(playerSprites);
                playerAtr.SetDeathSprites(deathSprites);
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Kotori");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 3, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.SetType(2);
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

                playerAniSprites.SetSprites(playerSprites);
                playerAtr.SetRegularSprites(playerSprites);
                playerAtr.SetDeathSprites(deathSprites);
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Bubba");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.SetType(3);
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

                playerAniSprites.SetSprites(playerSprites);
                playerAtr.SetRegularSprites(playerSprites);
                playerAtr.SetDeathSprites(deathSprites);
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_DeathSensei");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 3, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.SetType(4);
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

                playerAniSprites.SetSprites(playerSprites);
                playerAtr.SetRegularSprites(playerSprites);
                playerAtr.SetDeathSprites(deathSprites);
                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Bloop");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 1, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 2, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 3, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 4, enemyFourAtr);

                powerUpAtr.SetType(5);
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

                playerAniSprites.SetSprites(playerSprites);
                playerAtr.SetRegularSprites(playerSprites);
                playerAtr.SetDeathSprites(deathSprites);

                powerUpSprite.sprite = Resources.Load<Sprite>("Powerup_Takodachi");

                LoadEnemySprites(enemyOneSprite, enemyOneAniSprites, 2, enemyOneAtr);
                LoadEnemySprites(enemyTwoSprite, enemyTwoAniSprites, 3, enemyTwoAtr);
                LoadEnemySprites(enemyThreeSprite, enemyThreeAniSprites, 4, enemyThreeAtr);
                LoadEnemySprites(enemyFourSprite, enemyFourAniSprites, 5, enemyFourAtr);

                powerUpAtr.SetType(1);
                break;
        }
    }

    private void LoadEnemySprites(SpriteRenderer sprite, AnimatedSprites aniSprites, int character, Enemy enemy)
    {
        Sprite[] enemySprites = new Sprite[4];
        Sprite[] vulnerableSprites = new Sprite[4];

        switch(character)
        {
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

                aniSprites.SetSprites(enemySprites);
                enemy.SetRegularSprites(enemySprites);
                enemy.SetVulnerableSprites(vulnerableSprites);
                enemy.SetEatenSprite(Resources.Load<Sprite>("Ina_Eaten"));
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

                aniSprites.SetSprites(enemySprites);
                enemy.SetRegularSprites(enemySprites);
                enemy.SetVulnerableSprites(vulnerableSprites);
                enemy.SetEatenSprite(Resources.Load<Sprite>("Kiara_Eaten"));
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

                aniSprites.SetSprites(enemySprites);
                enemy.SetRegularSprites(enemySprites);
                enemy.SetVulnerableSprites(vulnerableSprites);
                enemy.SetEatenSprite(Resources.Load<Sprite>("Ame_Eaten"));
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

                aniSprites.SetSprites(enemySprites);
                enemy.SetRegularSprites(enemySprites);
                enemy.SetVulnerableSprites(vulnerableSprites);
                enemy.SetEatenSprite(Resources.Load<Sprite>("Calli_Eaten"));
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

                aniSprites.SetSprites(enemySprites);
                enemy.SetRegularSprites(enemySprites);
                enemy.SetVulnerableSprites(vulnerableSprites);
                enemy.SetEatenSprite(Resources.Load<Sprite>("Gura_Eaten"));
                break;
        }
    }
}
