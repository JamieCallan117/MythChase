using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    [SerializeField] private GameObject achievementOneObj;
    [SerializeField] private GameObject achievementTwoObj;
    [SerializeField] private GameObject achievementThreeObj;
    [SerializeField] private GameObject achievementFourObj;
    [SerializeField] private GameObject achievementFiveObj;
    [SerializeField] private GameObject achievementSixObj;
    [SerializeField] private GameObject achievementSevenObj;
    [SerializeField] private GameObject achievementEightObj;
    [SerializeField] private GameObject achievementNineObj;
    [SerializeField] private GameObject achievementTenObj;
    [SerializeField] private GameObject achievementElevenObj;
    [SerializeField] private GameObject achievementTwelveObj;
    [SerializeField] private GameObject achievementThirteenObj;
    private AchievementStar achievementOne;
    private AchievementStar achievementTwo;
    private AchievementStar achievementThree;
    private AchievementStar achievementFour;
    private AchievementStar achievementFive;
    private AchievementStar achievementSix;
    private AchievementStar achievementSeven;
    private AchievementStar achievementEight;
    private AchievementStar achievementNine;
    private AchievementStar achievementTen;
    private AchievementStar achievementEleven;
    private AchievementStar achievementTwelve;
    private AchievementStar achievementThirteen;
    [SerializeField] private GameObject achievementText;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;
    private TextMeshProUGUI achievementDesc;
    private Achievement achievements;

    void Start()
    {
        achievementOne = achievementOneObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementTwo = achievementTwoObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementThree = achievementThreeObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementFour = achievementFourObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementFive = achievementFiveObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementSix = achievementSixObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementSeven = achievementSevenObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementEight = achievementEightObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementNine = achievementNineObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementTen = achievementTenObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementEleven = achievementElevenObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementTwelve = achievementTwelveObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementThirteen = achievementThirteenObj.GetComponent(typeof(AchievementStar)) as AchievementStar;
        achievementDesc = achievementText.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;

        LoadAchievements();

        UpdateAchievements();
    }

    private void UpdateAchievements()
    {
        if (achievements.GetAchievement("Ina_Ten_Rounds") >= 10)
        {
            achievementOne.UnlockAchievement();
        }

        if (achievements.GetAchievement("Ina_OneHundred_Rounds") >= 100)
        {
            achievementTwo.UnlockAchievement();
        }

        if (achievements.GetAchievement("Kiara_Ten_Rounds") >= 10)
        {
            achievementThree.UnlockAchievement();
        }

        if (achievements.GetAchievement("Kiara_OneHundred_Rounds") >= 100)
        {
            achievementFour.UnlockAchievement();
        }

        if (achievements.GetAchievement("Ame_Ten_Rounds") >= 10)
        {
            achievementFive.UnlockAchievement();
        }

        if (achievements.GetAchievement("Ame_OneHundred_Rounds") >= 100)
        {
            achievementSix.UnlockAchievement();
        }

        if (achievements.GetAchievement("Calli_Ten_Rounds") >= 10)
        {
            achievementSeven.UnlockAchievement();
        }

        if (achievements.GetAchievement("Calli_OneHundred_Rounds") >= 100)
        {
            achievementEight.UnlockAchievement();
        }

        if (achievements.GetAchievement("Gura_Ten_Rounds") >= 10)
        {
            achievementNine.UnlockAchievement();
        }

        if (achievements.GetAchievement("Gura_OneHundred_Rounds") >= 100)
        {
            achievementTen.UnlockAchievement();
        }

        if (achievements.GetAchievement("Use_PowerUp_One") >= 1)
        {
            achievementEleven.UnlockAchievement();
        }

        if (achievements.GetAchievement("Use_PowerUp_Ten") >= 10)
        {
            achievementTwelve.UnlockAchievement();
        }

        if (achievements.GetAchievement("Use_PowerUp_OneHundred") >= 100)
        {
            achievementThirteen.UnlockAchievement();
        }
    }

    public void SelectAchievement(int selected)
    {
        switch (selected)
        {
            case 2:
                achievementDesc.text = "The Priestess - Complete 100 rounds as Ninomae Ina'nis - " + achievements.GetAchievement("Ina_OneHundred_Rounds").ToString() + "/100.";
                achievementOneObj.GetComponent<Outline>().enabled = false;
                achievementTwoObj.GetComponent<Outline>().enabled = true;
                achievementThreeObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                break;
            case 3:
                achievementDesc.text = "Kikkeriki! - Complete 10 rounds as Takanashi Kiara - " + achievements.GetAchievement("Kiara_Ten_Rounds").ToString() + "/10.";
                achievementTwoObj.GetComponent<Outline>().enabled = false;
                achievementThreeObj.GetComponent<Outline>().enabled = true;
                achievementFourObj.GetComponent<Outline>().enabled = false;
                achievementEightObj.GetComponent<Outline>().enabled = false;
                break;
            case 4:
                achievementDesc.text = "The Phoenix - Complete 100 rounds as Takanashi Kiara - " + achievements.GetAchievement("Kiara_OneHundred_Rounds").ToString() + "/100.";
                achievementThreeObj.GetComponent<Outline>().enabled = false;
                achievementFourObj.GetComponent<Outline>().enabled = true;
                achievementFiveObj.GetComponent<Outline>().enabled = false;
                achievementNineObj.GetComponent<Outline>().enabled = false;
                break;
            case 5:
                achievementDesc.text = "Hic! - Complete 10 rounds as Amelia Watson - " + achievements.GetAchievement("Ame_Ten_Rounds").ToString() + "/10.";
                achievementFourObj.GetComponent<Outline>().enabled = false;
                achievementFiveObj.GetComponent<Outline>().enabled = true;
                achievementTenObj.GetComponent<Outline>().enabled = false;
                break;
            case 6:
                achievementDesc.text = "The Detective - Complete 100 rounds as Amelia Watson - " + achievements.GetAchievement("Ame_OneHundred_Rounds").ToString() + "/100.";
                achievementOneObj.GetComponent<Outline>().enabled = false;
                achievementSixObj.GetComponent<Outline>().enabled = true;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = false;
                break;
            case 7:
                achievementDesc.text = "Guh! - Complete 10 rounds as Mori Calliope - " + achievements.GetAchievement("Calli_Ten_Rounds").ToString() + "/10.";
                achievementTwoObj.GetComponent<Outline>().enabled = false;
                achievementSixObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = true;
                achievementEightObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = false;
                break;
            case 8:
                achievementDesc.text = "The Reaper - Complete 100 rounds as Mori Calliope - " + achievements.GetAchievement("Calli_OneHundred_Rounds").ToString() + "/100.";
                achievementThreeObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                achievementEightObj.GetComponent<Outline>().enabled = true;
                achievementNineObj.GetComponent<Outline>().enabled = false;
                achievementTwelveObj.GetComponent<Outline>().enabled = false;
                break;
            case 9:
                achievementDesc.text = "a! - Complete 10 rounds as Gawr Gura - " + achievements.GetAchievement("Gura_Ten_Rounds").ToString() + "/10.";
                achievementFourObj.GetComponent<Outline>().enabled = false;
                achievementEightObj.GetComponent<Outline>().enabled = false;
                achievementNineObj.GetComponent<Outline>().enabled = true;
                achievementTenObj.GetComponent<Outline>().enabled = false;
                achievementThirteenObj.GetComponent<Outline>().enabled = false;
                break;
            case 10:
                achievementDesc.text = "The Shark - Complete 100 rounds as Gawr Gura - " + achievements.GetAchievement("Gura_OneHundred_Rounds").ToString() + "/100.";
                achievementFiveObj.GetComponent<Outline>().enabled = false;
                achievementNineObj.GetComponent<Outline>().enabled = false;
                achievementTenObj.GetComponent<Outline>().enabled = true;
                achievementThirteenObj.GetComponent<Outline>().enabled = false;
                break;
            case 11:
                achievementDesc.text = "Project: Hope - Use 1 Power Up! - " + achievements.GetAchievement("Use_PowerUp_One").ToString() + "/1.";
                achievementSixObj.GetComponent<Outline>().enabled = false;
                achievementSevenObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = true;
                achievementTwelveObj.GetComponent<Outline>().enabled = false;
                break;
            case 12:
                achievementDesc.text = "The Council - Use 10 Power Ups! - " + achievements.GetAchievement("Use_PowerUp_Ten").ToString() + "/10.";
                achievementEightObj.GetComponent<Outline>().enabled = false;
                achievementElevenObj.GetComponent<Outline>().enabled = false;
                achievementTwelveObj.GetComponent<Outline>().enabled = true;
                achievementThirteenObj.GetComponent<Outline>().enabled = false;
                break;
            case 13:
                achievementDesc.text = "The Ancient Ones - Use 100 Power Ups! - " + achievements.GetAchievement("Use_PowerUp_OneHundred").ToString() + "/100.";
                achievementNineObj.GetComponent<Outline>().enabled = false;
                achievementTenObj.GetComponent<Outline>().enabled = false;
                achievementTwelveObj.GetComponent<Outline>().enabled = false;
                achievementThirteenObj.GetComponent<Outline>().enabled = true;
                break;
            default:
                achievementDesc.text = "Wah! - Complete 10 rounds as Ninomae Ina'nis - " + achievements.GetAchievement("Ina_Ten_Rounds").ToString() + "/10.";
                achievementOneObj.GetComponent<Outline>().enabled = true;
                achievementTwoObj.GetComponent<Outline>().enabled = false;
                achievementSixObj.GetComponent<Outline>().enabled = false;
                break;
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

}
