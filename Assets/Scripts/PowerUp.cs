using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Power ups. Sets the type of it and then calls the method to use it.
public class PowerUp : MonoBehaviour
{
    private int type;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    //Sets what type the power up is based on the character selected.
    public void SetType(int typeToBe)
    {
        type = typeToBe;
    }

    //Use the power based on what type.
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
