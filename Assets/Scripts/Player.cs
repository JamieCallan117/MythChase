using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour {
    private Movement movement;
    private Vector3 startingPos;
    public GameObject powerUpObj;
    private GameManager gameManager;
    private PowerUp powerUp;

    private void Awake() {
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = this.transform.position;
    }

    public void ResetPosition() {
        this.transform.position = startingPos;
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

        if (Input.GetKeyDown(KeyCode.Space)) {
            powerUp.test();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == powerUpObj) {
            powerUp = gameManager.PickUpPowerUp();
        }
    }
}