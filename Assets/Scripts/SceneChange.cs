using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //Load the requested scene.
    public void moveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    //Set the chosen level and move to character select scene.
    public void selectLevel(int level)
    {
        DataStorage.level = level;

        switch (level)
        {
            case 1:
            case 2:
            case 3:
                moveToScene(2);
                break;
            default:
                moveToScene(0);
                break;
        }

        if (level >= 1 && level <= 3)
        {
            moveToScene(2);
        }
        else
        {
            moveToScene(0);
        }
    }

    //Choose character and move to level selected.
    public void selectCharacter(int character)
    {
        DataStorage.character = character;

        switch (DataStorage.level)
        {
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

    //Go to leaderboards scene.
    public void leaderboards()
    {
        moveToScene(6);
    }

    //Go to achievements scene.
    public void achievements()
    {
        moveToScene(7);
    }

    //Quit the application.
    public void quitGame()
    {
        Application.Quit();
    }
}
