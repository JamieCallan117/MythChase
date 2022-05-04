using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void moveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

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

    public void leaderboards()
    {
        moveToScene(6);
    }

    public void achievements()
    {
        moveToScene(7);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
