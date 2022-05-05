using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the player.
public class Player : MonoBehaviour
{
    private AnimatedSprites aniSprites;
    private Movement movement;
    private Vector3 startingPos;
    [SerializeField] private GameObject powerUpObj;
    private GameManager gameManager;
    private PowerUp powerUp;
    private Sprite[] regularSprites;
    private Sprite[] deathSprites;
    [SerializeField] private PlayAudio playAudio;

    void Awake()
    {
        aniSprites = GetComponent<AnimatedSprites>();
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = transform.position;
    }

    private void Update()
    {
        //Handles input for direction and using power up.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.Move(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.Move(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.Move(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.Move(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.Space) && gameManager.HasPowerUp())
        {
            powerUp.Activate();
        }
    }

    //Increases players speed.
    public void IncreaseSpeed()
    {
        movement.IncreaseSpeed();
    }

    //Sets player speed.
    public void SetSpeed(float newSpeed)
    {
        movement.SetSpeed(newSpeed);
    }

    //Resets position of player to starting position.
    public void ResetPosition()
    {
        transform.position = startingPos;
    }

    //Kills the player.
    public void KillPlayer()
    {
        aniSprites.KillPlayer(deathSprites);

        Invoke("PlayDeathSound", 1.5f);
    }

    //Revives the player.
    public void RevivePlayer()
    {
        aniSprites.RevivePlayer(regularSprites);
    }

    //Plays the death sound for the selected character.
    private void PlayDeathSound()
    {
        switch(DataStorage.character)
        {
            case 2:
                playAudio.PlayKiaraDeath();
                break;
            case 3:
                playAudio.PlayAmeDeath();
                break;
            case 4:
                playAudio.PlayCalliDeath();
                break;
            case 5:
                playAudio.PlayGuraDeath();
                break;
            default:
                playAudio.PlayInaDeath();
                break;
        }
    }

    //Toggles movement of/off.
    public void ToggleMovement(bool enabled)
    {
        movement.ToggleMovement(enabled);
    }

    //Stops current movement.
    public void HaltMovement()
    {
        movement.HaltMovement();
    }

    //Teleports player.
    public void TeleportObject(Vector3 newPos)
    {
        movement.TeleportObject(newPos);
    }

    //Sets currently used sprites.
    public void SetCurrentSprites(Sprite[] spritesToUse)
    {
        aniSprites.SetSprites(spritesToUse);
    }

    //Sets the regular sprites.
    public void SetRegularSprites(Sprite[] spritesToUse)
    {
        regularSprites = spritesToUse;
    }

    //Sets the sprites to be used on death.
    public void SetDeathSprites(Sprite[] spritesToUse)
    {
        deathSprites = spritesToUse;
    }

    //When colliding with the power up object.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == powerUpObj)
        {
            powerUp = gameManager.PickUpPowerUp();
        }
    }
}