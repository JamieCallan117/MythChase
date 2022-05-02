using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.UI;

public class Achievements : MonoBehaviour {
    public GameObject achievementOneObj;
    public GameObject achievementTwoObj;
    public GameObject achievementThreeObj;
    public GameObject achievementFourObj;
    public GameObject achievementFiveObj;
    public GameObject achievementSixObj;
    public GameObject achievementSevenObj;
    public GameObject achievementEightObj;
    public GameObject achievementNineObj;
    public GameObject achievementTenObj;
    public GameObject achievementElevenObj;
    public GameObject achievementTwelveObj;
    public GameObject achievementThirteenObj;
    public GameObject achievementText;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    private TextMeshProUGUI achievementDesc;
    private Achievement achievements;

    private void Start() {
        achievementDesc = achievementText.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;

        LoadAchievements();

        UpdateAchievements();
    }

    private void UpdateAchievements() {
        if ((int) achievements.achievements["Ina_Ten_Rounds"] >= 10) {
            achievementOneObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Ina_OneHundred_Rounds"] >= 100) {
            achievementTwoObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Kiara_Ten_Rounds"] >= 10) {
            achievementThreeObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Kiara_OneHundred_Rounds"] >= 100) {
            achievementFourObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Ame_Ten_Rounds"] >= 10) {
            achievementFiveObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Ame_OneHundred_Rounds"] >= 100) {
            achievementSixObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Calli_Ten_Rounds"] >= 10) {
            achievementSevenObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Calli_OneHundred_Rounds"] >= 100) {
            achievementEightObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Gura_Ten_Rounds"] >= 10) {
            achievementNineObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Gura_OneHundred_Rounds"] >= 100) {
            achievementTenObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Use_PowerUp_One"] >= 1) {
            achievementElevenObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Use_PowerUp_Ten"] >= 10) {
            achievementTwelveObj.GetComponent<Image>().sprite = unlockedSprite;
        }

        if ((int) achievements.achievements["Use_PowerUp_OneHundred"] >= 100) {
            achievementThirteenObj.GetComponent<Image>().sprite = unlockedSprite;
        }
    }

    public void SelectAchievement(int selected) {
        switch (selected) {
            case 2:
                achievementDesc.text = "The Priestess - Complete 100 rounds as Ninomae Ina'nis - " + achievements.achievements["Ina_OneHundred_Rounds"].ToString() + "/100.";
                achievementOneObj.GetComponent<Outline>().enabled = false;
                achievementTwoObj.GetComponent<Outline>().enabled = true;
                achievementThreeObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                break;
            case 3:
                achievementDesc.text = "Kikkeriki! - Complete 10 rounds as Takanashi Kiara - " + achievements.achievements["Kiara_Ten_Rounds"].ToString() + "/10.";
                achievementTwoObj.GetComponent<Outline>().enabled = false;
                achievementThreeObj.GetComponent<Outline>().enabled = true;
                achievementFourObj.GetComponent<Outline>().enabled = false;
                achievementEightObj.GetComponent<Outline>().enabled = false;
                break;
            case 4:
                achievementDesc.text = "The Phoenix - Complete 100 rounds as Takanashi Kiara - " + achievements.achievements["Kiara_OneHundred_Rounds"].ToString() + "/100.";
                achievementThreeObj.GetComponent<Outline>().enabled = false;
                achievementFourObj.GetComponent<Outline>().enabled = true;
                achievementFiveObj.GetComponent<Outline>().enabled = false;
                achievementNineObj.GetComponent<Outline>().enabled = false;
                break;
            case 5:
                achievementDesc.text = "Hic! - Complete 10 rounds as Amelia Watson - " + achievements.achievements["Ame_Ten_Rounds"].ToString() + "/10.";
                achievementFourObj.GetComponent<Outline>().enabled = false;
                achievementFiveObj.GetComponent<Outline>().enabled = true;
                achievementTenObj.GetComponent<Outline>().enabled = false;
                break;
            case 6:
                achievementDesc.text = "The Detective - Complete 100 rounds as Amelia Watson - " + achievements.achievements["Ame_OneHundred_Rounds"].ToString() + "/100.";
                achievementOneObj.GetComponent<Outline>().enabled = false;
                achievementSixObj.GetComponent<Outline>().enabled = true;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = false;
                break;
            case 7:
                achievementDesc.text = "Guh! - Complete 10 rounds as Mori Calliope - " + achievements.achievements["Calli_Ten_Rounds"].ToString() + "/10.";
                achievementTwoObj.GetComponent<Outline>().enabled = false;
                achievementSixObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = true;
                achievementEightObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = false;
                break;
            case 8:
                achievementDesc.text = "The Reaper - Complete 100 rounds as Mori Calliope - " + achievements.achievements["Calli_OneHundred_Rounds"].ToString() + "/100.";
                achievementThreeObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                achievementEightObj.GetComponent<Outline>().enabled = true;
                achievementNineObj.GetComponent<Outline>().enabled = false;
                achievementTwelveObj.GetComponent<Outline>().enabled = false;
                break;
            case 9:
                achievementDesc.text = "a! - Complete 10 rounds as Gawr Gura - " + achievements.achievements["Gura_Ten_Rounds"].ToString() + "/10.";
                achievementFourObj.GetComponent<Outline>().enabled = false;
                achievementEightObj.GetComponent<Outline>().enabled = false;
                achievementNineObj.GetComponent<Outline>().enabled = true;
                achievementTenObj.GetComponent<Outline>().enabled = false;
                achievementThirteenObj.GetComponent<Outline>().enabled = false;
                break;
            case 10:
                achievementDesc.text = "The Shark - Complete 100 rounds as Gawr Gura - " + achievements.achievements["Gura_OneHundred_Rounds"].ToString() + "/100.";
                achievementFiveObj.GetComponent<Outline>().enabled = false;
                achievementNineObj.GetComponent<Outline>().enabled = false;
                achievementTenObj.GetComponent<Outline>().enabled = true;
                achievementThirteenObj.GetComponent<Outline>().enabled = false;
                break;
            case 11:
                achievementDesc.text = "Project: Hope - Use 1 Power Up! - " + achievements.achievements["Use_PowerUp_One"].ToString() + "/1.";
                achievementSixObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = true;
                achievementTwelveObj.GetComponent<Outline>().enabled = false;
                break;
            case 12:
                achievementDesc.text = "The Council - Use 10 Power Ups! - " + achievements.achievements["Use_PowerUp_Ten"].ToString() + "/10.";
                achievementEightObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = false;
                achievementTwelveObj.GetComponent<Outline>().enabled = true;
                achievementThirteenObj.GetComponent<Outline>().enabled = false;
                break;
            case 13:
                achievementDesc.text = "The Ancient Ones - Use 100 Power Ups! - " + achievements.achievements["Use_PowerUp_OneHundred"].ToString() + "/100.";
                achievementNineObj.GetComponent<Outline>().enabled = false;
                achievementTenObj.GetComponent<Outline>().enabled = false;
                achievementTwelveObj.GetComponent<Outline>().enabled = false;
                achievementThirteenObj.GetComponent<Outline>().enabled = true;
                break;
            default:
                achievementDesc.text = "Wah! - Complete 10 rounds as Ninomae Ina'nis - " + achievements.achievements["Ina_Ten_Rounds"].ToString() + "/10.";
                achievementOneObj.GetComponent<Outline>().enabled = true;
                achievementTwoObj.GetComponent<Outline>().enabled = false;
                achievementSixObj.GetComponent<Outline>().enabled = false;
                break;
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
