using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {
    public float speed = 8f;
    public LayerMask obstacleLayer;
    public Rigidbody2D rigidbody2d;
    public Vector2 currentDirection;
    public Vector2 nextDirection;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
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

        rigidbody2d.MovePosition(position + translation);
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

    public bool ValidMove(Vector2 direction) {
        //Check to see if there is a wall in the next tile over or not, returns null if there isn't.
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1f, obstacleLayer);
        return hit.collider == null;
    }

}
