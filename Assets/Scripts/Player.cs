using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour {
    public AnimatedSprites aniSprites;
    private Movement movement;
    private Vector3 startingPos;
    public GameObject powerUpObj;
    private GameManager gameManager;
    private PowerUp powerUp;
    public Sprite[] regularSprites;
    public Sprite[] deathSprites;
    public AudioSource audioSource;
    public AudioClip inaDeath;
    public AudioClip kiaraDeath;
    public AudioClip ameDeath;
    public AudioClip calliDeath;
    public AudioClip guraDeath;

    private void Awake() {
        aniSprites = GetComponent<AnimatedSprites>();
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = this.transform.position;
    }

    public void ResetPosition() {
        this.transform.position = startingPos;
    }

    public void KillPlayer() {
        aniSprites.loop = false;
        aniSprites.UpdateAnimationRate(0.5f);
        aniSprites.sprites = deathSprites;
        aniSprites.currentFrame = 0;

        aniSprites.enable(true);

        Invoke("PlayDeathSound", 1.5f);
    }

    private void PlayDeathSound() {
        switch(PlayerStats.character) {
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

    private void Update() {
        // Set the new direction based on the current input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movement.Move(Vector2.up);
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movement.Move(Vector2.down);
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement.Move(Vector2.left);
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement.Move(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.Space) && gameManager.HasPowerUp()) {
            powerUp.Activate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == powerUpObj) {
            powerUp = gameManager.PickUpPowerUp();
        }
    }
}