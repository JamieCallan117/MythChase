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
        
        //Level One is scene 2

        switch (level) {
            case 1:
                moveToScene(2);
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
