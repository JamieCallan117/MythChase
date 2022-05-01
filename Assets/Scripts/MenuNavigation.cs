using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour {
    public int returnScene;
    public SceneChange sceneChange;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)) {
            sceneChange.moveToScene(returnScene);
        }
    }
}
