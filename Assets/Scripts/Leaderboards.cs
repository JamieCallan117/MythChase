using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class Leaderboards : MonoBehaviour
{
    private Leaderboard leaderboard;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject leaderboardsBox;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI leaderboardsText;
    private string scoreOneName = "";
    private string scoreTwoName = "";
    private string scoreThreeName = "";
    private string scoreFourName = "";
    private string scoreFiveName = "";
    private string scoreSixName = "";
    private string scoreSevenName = "";
    private string scoreEightName = "";
    private string scoreNineName = "";
    private string scoreTenName = "";
    private int scoreOne = 0;
    private int scoreTwo = 0;
    private int scoreThree = 0;
    private int scoreFour = 0;
    private int scoreFive = 0;
    private int scoreSix = 0;
    private int scoreSeven = 0;
    private int scoreEight = 0;
    private int scoreNine = 0;
    private int scoreTen = 0;
    private int level = 1;

    void Awake()
    {
        levelText = textBox.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        leaderboardsText = leaderboardsBox.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }

    void Start()
    {
        level = 1;

        GenerateLeaderboard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (level < 3)
            {
                level++;

                GenerateLeaderboard();
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (level > 1)
            {
                level--;

                GenerateLeaderboard();
            }
        }
    }

    private void GenerateLeaderboard()
    {
        ResetLeaderboard();

        switch (level) {
            case 2:
                levelText.text = "Level Two";
                break;
            case 3:
                levelText.text = "Level Three";
                break;
            default:
                levelText.text = "Level One";
                break;
        }

        LoadFile();

        if (leaderboard == null)
        {
            leaderboardsText.text = "No data for level " + level;
        }
        else
        {
            foreach (DictionaryEntry entry in leaderboard.GetLeaderboards())
            {
                int score = (int) entry.Value;
                string name = (string) entry.Key;

                if (score > scoreOne)
                {
                    scoreOne = score;
                    scoreOneName = name;
                }
                else if (score > scoreTwo)
                {
                    scoreTwo = score;
                    scoreTwoName = name;
                }
                else if (score > scoreThree)
                {
                    scoreThree = score;
                    scoreThreeName = name;
                }
                else if (score > scoreFour)
                {
                    scoreFour = score;
                    scoreFourName = name;
                }
                else if (score > scoreFive)
                {
                    scoreFive = score;
                    scoreFiveName = name;
                }
                else if (score > scoreSix)
                {
                    scoreSix = score;
                    scoreSixName = name;
                }
                else if (score > scoreSeven)
                {
                    scoreSeven = score;
                    scoreSevenName = name;
                }
                else if (score > scoreEight)
                {
                    scoreEight = score;
                    scoreEightName = name;
                }
                else if (score > scoreNine)
                {
                    scoreNine = score;
                    scoreNineName = name;
                }
                else if (score > scoreTen)
                {
                    scoreTen = score;
                    scoreTenName = name;
                }
            }

            PopulateLeaderboard();
        }
    }

    private void ResetLeaderboard()
    {
        scoreOneName = "";
        scoreTwoName = "";
        scoreThreeName = "";
        scoreFourName = "";
        scoreFiveName = "";
        scoreSixName = "";
        scoreSevenName = "";
        scoreEightName = "";
        scoreNineName = "";
        scoreTenName = "";

        scoreOne = 0;
        scoreTwo = 0;
        scoreThree = 0;
        scoreFour = 0;
        scoreFive = 0;
        scoreSix = 0;
        scoreSeven = 0;
        scoreEight = 0;
        scoreNine = 0;
        scoreTen = 0;
    }

    private void PopulateLeaderboard()
    {
        string leaderboardStr = "";

        if (scoreOne != 0)
        {
            leaderboardStr += "1 - " + scoreOneName + " - " + scoreOne + "\n";
        }

        if (scoreTwo != 0)
        {
            leaderboardStr += "2 - " + scoreTwoName + " - " + scoreTwo + "\n";
        }

        if (scoreThree != 0)
        {
            leaderboardStr += "3 - " + scoreThreeName + " - " + scoreThree + "\n";
        }

        if (scoreFour != 0)
        {
            leaderboardStr += "4 - " + scoreFourName + " - " + scoreFour + "\n";
        }

        if (scoreFive != 0)
        {
            leaderboardStr += "5 - " + scoreFiveName + " - " + scoreFive + "\n";
        }

        if (scoreSix != 0)
        {
            leaderboardStr += "6 - " + scoreSixName + " - " + scoreSix + "\n";
        }

        if (scoreSeven != 0)
        {
            leaderboardStr += "7 - " + scoreSevenName + " - " + scoreSeven + "\n";
        }

        if (scoreEight != 0)
        {
            leaderboardStr += "8 - " + scoreEightName + " - " + scoreEight + "\n";
        }

        if (scoreNine != 0)
        {
            leaderboardStr += "9 - " + scoreNineName + " - " + scoreNine + "\n";
        }

        if (scoreTen != 0)
        {
            leaderboardStr += "10 - " + scoreTenName + " - " + scoreTen;
        }

        leaderboardsText.text = leaderboardStr;
    }

    private void LoadFile()
    {
        string path = Application.persistentDataPath + "/leaderboard" + level + ".dat";
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
}
