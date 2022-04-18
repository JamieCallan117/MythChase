using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {
    public bool movementEnabled = false;
    public float speed = 8f;
    public LayerMask obstacleLayer;
    public Rigidbody2D rigidbody2d;
    public Vector2 currentDirection;
    public Vector2 nextDirection;
    private float yUpper = 0.0f;
    private float yLower = 0.0f;
    private float xLeft = 0.0f;
    private float xRight = 0.0f;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();

        if (PlayerStats.level == 1) {
            yUpper = 13.4f;
            yLower = -16.4f;
            xLeft = -0.5f;
            xRight = 0.5f;
        }
    }

    private void Update() {
        //Attempts to move in the 'queued' direction
        if (nextDirection != Vector2.zero) {
            Move(nextDirection);
        }
    }

    private void FixedUpdate() {
        //Moves the object in the desired direction
        Vector2 position = rigidbody2d.position;
        Vector2 translation = currentDirection * speed * Time.fixedDeltaTime;

        if (movementEnabled == true) {
            rigidbody2d.MovePosition(position + translation);
        }

        if (PlayerStats.level == 1) {
            if (transform.position.y >= yUpper) {
                transform.position = new Vector3(xLeft, (yLower + 0.1f), -5.0f);
            } else if (transform.position.y <= yLower) {
                transform.position = new Vector3(xRight, (yUpper - 0.1f), -5.0f);
            }
        }
    
    }

    public void Move(Vector2 direction) {
        //Check to see if the next tile isn't a wall, and if not set the current direction to
        //the desired direction.
        //If it is a wall, then queue up the direction as the next direction.
        if (ValidMove(direction)) {
            currentDirection = direction;
            nextDirection = Vector2.zero;
        } else {
            nextDirection = direction;
        }
    }

    public void ForceMove(Vector2 direction) {
        currentDirection = direction;
    }

    public bool ValidMove(Vector2 direction) {
        //Check to see if there is a wall in the next tile over or not, returns null if there isn't.
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1f, obstacleLayer);
        return hit.collider == null;
    }

}
