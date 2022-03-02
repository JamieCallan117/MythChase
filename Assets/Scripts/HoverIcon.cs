using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverIcon : MonoBehaviour {
    private GameObject playArrow;
    private GameObject quitArrow;
    private GameObject levelOneArrow;
    private GameObject levelTwoArrow;
    private GameObject levelThreeArrow;

    public void Awake() {
        playArrow = GameObject.Find("PlayArrow");
        quitArrow = GameObject.Find("QuitArrow");
        levelOneArrow = GameObject.Find("LevelOneArrow");
        levelTwoArrow = GameObject.Find("LevelTwoArrow");
        levelThreeArrow = GameObject.Find("LevelThreeArrow");
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
}
