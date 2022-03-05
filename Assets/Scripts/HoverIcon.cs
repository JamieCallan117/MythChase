using UnityEngine;
using UnityEngine.EventSystems;

public class HoverIcon : MonoBehaviour {
    private GameObject playArrow;
    private GameObject quitArrow;
    private GameObject levelOneArrow;
    private GameObject levelTwoArrow;
    private GameObject levelThreeArrow;
    private GameObject inaArrow;
    private GameObject kiaraArrow;
    private GameObject ameArrow;
    private GameObject calliArrow;
    private GameObject guraArrow;

    public void Awake() {
        playArrow = GameObject.Find("PlayArrow");
        quitArrow = GameObject.Find("QuitArrow");
        levelOneArrow = GameObject.Find("LevelOneArrow");
        levelTwoArrow = GameObject.Find("LevelTwoArrow");
        levelThreeArrow = GameObject.Find("LevelThreeArrow");
        inaArrow = GameObject.Find("InaArrow");
        kiaraArrow = GameObject.Find("KiaraArrow");
        ameArrow = GameObject.Find("AmeArrow");
        calliArrow = GameObject.Find("CalliArrow");
        guraArrow = GameObject.Find("GuraArrow");
    }

    public void Start() {
        if (quitArrow != null) {
            quitArrow.SetActive(false);
        }

        if (levelTwoArrow != null) {
            levelTwoArrow.SetActive(false);
        }

        if (levelThreeArrow != null) {
            levelThreeArrow.SetActive(false);
        }

        if (kiaraArrow != null) {
            kiaraArrow.SetActive(false);
        }

        if (ameArrow != null) {
            ameArrow.SetActive(false);
        }

        if (calliArrow != null) {
            calliArrow.SetActive(false);
        }

        if (guraArrow != null) {
            guraArrow.SetActive(false);
        }
    }

    public void hoverPlay() {
        playArrow.SetActive(true);
        quitArrow.SetActive(false);
    }

    public void hoverQuit() {
        playArrow.SetActive(false);
        quitArrow.SetActive(true);
    }

    public void hoverLevelOne() {
        levelOneArrow.SetActive(true);
        levelTwoArrow.SetActive(false);
        levelThreeArrow.SetActive(false);
    }

    public void hoverLevelTwo() {
        levelOneArrow.SetActive(false);
        levelTwoArrow.SetActive(true);
        levelThreeArrow.SetActive(false);
    }

    public void hoverLevelThree() {
        levelOneArrow.SetActive(false);
        levelTwoArrow.SetActive(false);
        levelThreeArrow.SetActive(true);
    }

    public void hoverIna() {
        inaArrow.SetActive(true);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(false);
        calliArrow.SetActive(false);
        guraArrow.SetActive(false);
    }

    public void hoverKiara() {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(true);
        ameArrow.SetActive(false);
        calliArrow.SetActive(false);
        guraArrow.SetActive(false);
    }

    public void hoverAme() {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(true);
        calliArrow.SetActive(false);
        guraArrow.SetActive(false);
    }

    public void hoverCalli() {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(false);
        calliArrow.SetActive(true);
        guraArrow.SetActive(false);
    }

    public void hoverGura() {
        inaArrow.SetActive(false);
        kiaraArrow.SetActive(false);
        ameArrow.SetActive(false);
        calliArrow.SetActive(false);
        guraArrow.SetActive(true);
    }
}
