using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = transform.position;
    }

    private void Update()
    {
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

    public void IncreaseSpeed()
    {
        movement.IncreaseSpeed();
    }

    public void SetSpeed(float newSpeed)
    {
        movement.SetSpeed(newSpeed);
    }

    public void ResetPosition()
    {
        transform.position = startingPos;
    }

    public void KillPlayer()
    {
        aniSprites.KillPlayer(deathSprites);

        Invoke("PlayDeathSound", 1.5f);
    }

    public void RevivePlayer()
    {
        aniSprites.RevivePlayer(regularSprites);
    }

    private void PlayDeathSound()
    {
        switch(PlayerStats.character)
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

    public void ToggleMovement(bool enabled)
    {
        movement.ToggleMovement(enabled);
    }

    public void HaltMovement()
    {
        movement.HaltMovement();
    }

    public void TeleportObject(Vector3 newPos)
    {
        movement.TeleportObject(newPos);
    }

    public void SetRegularSprites(Sprite[] spritesToUse)
    {
        regularSprites = spritesToUse;
    }

    public void SetDeathSprites(Sprite[] spritesToUse)
    {
        deathSprites = spritesToUse;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == powerUpObj)
        {
            powerUp = gameManager.PickUpPowerUp();
        }
    }
}