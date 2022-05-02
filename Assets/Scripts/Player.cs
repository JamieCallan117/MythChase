using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    public AnimatedSprites aniSprites;
    private Movement movement;
    private Vector3 startingPos;
    [SerializeField] private GameObject powerUpObj;
    private GameManager gameManager;
    private PowerUp powerUp;
    public Sprite[] regularSprites;
    public Sprite[] deathSprites;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip inaDeath;
    [SerializeField] private AudioClip kiaraDeath;
    [SerializeField] private AudioClip ameDeath;
    [SerializeField] private AudioClip calliDeath;
    [SerializeField] private AudioClip guraDeath;

    void Awake()
    {
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = startingPos;
    }

    public void KillPlayer()
    {
        aniSprites.loop = false;
        aniSprites.UpdateAnimationRate(0.5f);
        aniSprites.sprites = deathSprites;
        aniSprites.currentFrame = 0;

        aniSprites.enable(true);

        Invoke("PlayDeathSound", 1.5f);
    }

    private void PlayDeathSound()
    {
        switch(PlayerStats.character)
        {
            case 2:
                audioSource.clip = kiaraDeath;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = ameDeath;
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = calliDeath;
                audioSource.Play();
                break;
            case 5:
                audioSource.clip = guraDeath;
                audioSource.Play();
                break;
            default:
                audioSource.clip = inaDeath;
                audioSource.Play();
                break;
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == powerUpObj)
        {
            powerUp = gameManager.PickUpPowerUp();
        }
    }
}