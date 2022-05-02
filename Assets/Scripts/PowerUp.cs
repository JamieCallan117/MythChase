using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int type;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Activate()
    {
        switch(type)
        {
            case 1:
                gameManager.UsePowerUp(type);
                break;
            case 2:
                gameManager.UsePowerUp(type);
                break;
            case 3:
                gameManager.UsePowerUp(type);
                break;
            case 4:
                gameManager.UsePowerUp(type);
                break;
            case 5:
                gameManager.UsePowerUp(type);
                break;
            default:
                gameManager.ExitWithoutSave();
                break;
        }
    }
}
