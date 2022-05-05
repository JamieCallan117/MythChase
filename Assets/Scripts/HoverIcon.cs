using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

//Handles which arrows appear on menus when different options are selected.
public class HoverIcon : MonoBehaviour
{
    private GameObject playArrow;
    private GameObject leaderboardsArrow;
    private GameObject achievementsArrow;
    private GameObject quitArrow;
    private GameObject levelOneArrow;
    private GameObject levelTwoArrow;
    private GameObject levelThreeArrow;
    private GameObject inaArrow;
    private GameObject kiaraArrow;
    private GameObject ameArrow;
    private GameObject calliArrow;
    private GameObject guraArrow;
    private GameObject textBox;
    private TextMeshProUGUI characterText;

    void Awake()
    {
        playArrow = GameObject.Find("PlayArrow");
        leaderboardsArrow = GameObject.Find("LeaderboardsArrow");
        achievementsArrow = GameObject.Find("AchievementsArrow");
        quitArrow = GameObject.Find("QuitArrow");
        levelOneArrow = GameObject.Find("LevelOneArrow");
        levelTwoArrow = GameObject.Find("LevelTwoArrow");
        levelThreeArrow = GameObject.Find("LevelThreeArrow");
        inaArrow = GameObject.Find("InaArrow");
        kiaraArrow = GameObject.Find("KiaraArrow");
        ameArrow = GameObject.Find("AmeArrow");
        calliArrow = GameObject.Find("CalliArrow");
        guraArrow = GameObject.Find("GuraArrow");
        textBox = GameObject.Find("CharacterText");

        if (textBox != null)
        {
            characterText = textBox.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        }  
    }

    void Start()
    {
        if (leaderboardsArrow != null)
        {
            leaderboardsArrow.SetActive(false);
        }

        if (achievementsArrow != null)
        {
            achievementsArrow.SetActive(false);
        }

        if (quitArrow != null)
        {
            quitArrow.SetActive(false);
        }

        if (levelTwoArrow != null)
        {
            levelTwoArrow.SetActive(false);
        }

        if (levelThreeArrow != null)
        {
            levelThreeArrow.SetActive(false);
        }

        if (kiaraArrow != null)
        {
            kiaraArrow.SetActive(false);
        }

        if (ameArrow != null)
        {
            ameArrow.SetActive(false);
        }

        if (calliArrow != null)
        {
            calliArrow.SetActive(false);
        }

        if (guraArrow != null)
        {
            guraArrow.SetActive(false);
        }
    }

    //Play button arrow appears and the others dissapear.
    public void hoverPlay()
    {
        playArrow.SetActive(true);
        leaderboardsArrow.SetActive(false);
        achievementsArrow.SetActive(false);
        quitArrow.SetActive(false);
    }

    //Leaderboards button arrow appears and the others dissapear.
    public void hoverLeaderboards()
    {
        playArrow.SetActive(false);
        leaderboardsArrow.SetActive(true);
        achievementsArrow.SetActive(false);
        quitArrow.SetActive(false);
    }

    //Achievements button arrow appears and the others dissapear.
    public void hoverAchievements()
    {
        playArrow.SetActive(false);
        leaderboardsArrow.SetActive(false);
        achievementsArrow.SetActive(true);
        quitArrow.SetActive(false);
    }

    //Quit button arrow appears and the others dissapear.
    public void hoverQuit()
    {
        playArrow.SetActive(false);
        leaderboardsArrow.SetActive(false);
        achievementsArrow.SetActive(false);
        quitArrow.SetActive(true);
    }

    //When level one is selected, hide arrows for the others.
    public void hoverLevelOne()
    {
        levelOneArrow.SetActive(true);
        levelTwoArrow.SetActive(false);
        levelThreeArrow.SetActive(false);
    }

    //When level two is selected, hide arrows for the others.
    public void hoverLevelTwo()
    {
        levelOneArrow.SetActive(false);
        levelTwoArrow.SetActive(true);
        levelThreeArrow.SetActive(false);
    }

    //When level three is selected, hide arrows for the others.
    public void hoverLevelThree()
    {
        levelOneArrow.SetActive(false);
        levelTwoArrow.SetActive(false);
        levelThreeArrow.SetActive(true);
    }

    //When the Ina character is selected, hide the arrows for the others and set the power up description.
    public void hoverIna()
    {
        inaArrow.SetActive(true);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(false);
        calliArrow.SetActive(false);
        guraArrow.SetActive(false);

        characterText.text = "Ina'nis - Can summon the power of the void to teleport to a random junction on the map.";
    }

    //When the Kiara character is selected, hide the arrows for the others and set the power up description.
    public void hoverKiara()
    {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(true);
        ameArrow.SetActive(false);
        calliArrow.SetActive(false);
        guraArrow.SetActive(false);

        characterText.text = "Kiara - Uses her phoenix powers to become invulnerable for a short period of time.";
    }

    //When the Ame character is selected, hide the arrows for the others and set the power up description.
    public void hoverAme()
    {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(true);
        calliArrow.SetActive(false);
        guraArrow.SetActive(false);

        characterText.text = "Amelia - Freezes time around her to allow for a quick escape.";
    }

    //When the Calli character is selected, hide the arrows for the others and set the power up description.
    public void hoverCalli()
    {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(false);
        calliArrow.SetActive(true);
        guraArrow.SetActive(false);

        characterText.text = "Calliope - Unleashes her powers to harvest the souls of anyone she comes into contact with.";
    }

    //When the Gura character is selected, hide the arrows for the others and set the power up description.
    public void hoverGura()
    {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(false);
        calliArrow.SetActive(false);
        guraArrow.SetActive(true);

        characterText.text = "Gura - Becomes the feared Apex predator and scares away any who dare to approach her.";
    }
}