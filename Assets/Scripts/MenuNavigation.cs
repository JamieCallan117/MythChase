using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] private int returnScene;
    [SerializeField] private SceneChange sceneChange;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            sceneChange.moveToScene(returnScene);
        }
    }
}
