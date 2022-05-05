using UnityEngine;

//Used to go back through menus to previous scenes.
public class MenuNavigation : MonoBehaviour
{
    [SerializeField] private int returnScene;
    [SerializeField] private SceneChange sceneChange;

    void Update()
    {
        //Esc and backspace send you to the previous scene.
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            sceneChange.moveToScene(returnScene);
        }
    }
}
