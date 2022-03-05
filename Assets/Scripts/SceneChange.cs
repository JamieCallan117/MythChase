using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {
    public void moveToScene(int sceneID) {
        SceneManager.LoadScene(sceneID);
    }

    public void selectLevel(int level) {
        PlayerStats.level = level;

        switch (level) {
            case 1:
                moveToScene(2);
                break;
            default:
                moveToScene(0);
                break;
        }

        if (level >= 1 && level <= 3) {
            moveToScene(2); //Character select scene
        } else {
            moveToScene(0); //Main menu
        }
    }

    public void selectCharacter(int character) {
        PlayerStats.character = character;

        switch (PlayerStats.level) {
            case 1:
                moveToScene(3);
                break;
            case 2:
                moveToScene(4);
                break;
            case 3:
                moveToScene(5);
                break;
            default:
                moveToScene(0);
                break;
        }
    }

    public void quitGame() {
        Application.Quit();
    }
}
