using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

//Populate the leaderboards scene and swap between each levels leaderboards.
public class Leaderboards : MonoBehaviour
{
    private Leaderboard leaderboard;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject leaderboardsBox;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI leaderboardsText;
    private string leaderboardStr = "";
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
        //Swap between levels.
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

    //Generate the leaderboards for the selected level and display.
    private void GenerateLeaderboard()
    {
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
            //Convert the hashtable value and keys to arrays and sort by value.
            leaderboardsText.text = "Loading...";
            leaderboardStr = "";

            Hashtable leaderboardHT = leaderboard.GetLeaderboards();

            string[] arrKey = new string[leaderboardHT.Count];
            int[] arrVal = new int[leaderboardHT.Count];

            leaderboardHT.Keys.CopyTo(arrKey, 0);
            leaderboardHT.Values.CopyTo(arrVal, 0);

            Array.Sort(arrVal, arrKey);

            Array.Reverse(arrVal);
            Array.Reverse(arrKey);

            for (int i = 0; i < arrKey.Length; i++)
            {
                leaderboardStr += (i + 1) + " - " + arrKey[i].ToString() + " - " + arrVal[i].ToString() + "\n";
            }

            leaderboardsText.text = leaderboardStr;
        }
    }

    //Load the leaderboard file.
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
