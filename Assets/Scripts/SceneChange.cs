using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {
    public void moveToScene(int sceneID) {
        SceneManager.LoadScene(sceneID);
    }

    public void quitGame() {
        Application.Quit();
    }
}
