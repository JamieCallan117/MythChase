using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 8.0f;
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 directionToMove = Vector2.zero;
    private Vector2 nextDirection = Vector2.zero;
    public LayerMask wallLayer;
    public bool validMove;

    private void Update() {
        CheckInput();

        //If we have a direction lined up.
        if (nextDirection != Vector2.zero) {
            //If that lined up direction is valid.
            if (ValidMove(nextDirection)) {
                Move(nextDirection);
                nextDirection = Vector2.zero;
            //If the new direction is valid.
            } else if (ValidMove(directionToMove)) {
                Move(directionToMove);
                nextDirection = Vector2.zero;
            //If the current direction is valid.
            } else if (ValidMove(currentDirection)) {
                Move(currentDirection);

                if (directionToMove != Vector2.zero) {
                    nextDirection = directionToMove;
                }
            //If unable to move.
            } else {
                Move(Vector2.zero);
            }
        //Valid input.
        } else if (ValidMove(directionToMove) && directionToMove != Vector2.zero) {
            Move(directionToMove);
        //If current direction is valid.
        } else if (ValidMove(currentDirection)) {
            Move(currentDirection);

            if (directionToMove != Vector2.zero) {
                nextDirection = directionToMove;
                directionToMove = Vector2.zero;
            }
        //If unable to move in current direction.
        } else {
            Move(Vector2.zero);
        }
    }

    private void CheckInput() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            directionToMove = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            directionToMove = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            directionToMove = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            directionToMove = Vector2.right;
        }
    }

    private bool ValidMove(Vector2 d) {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, d, 1.5f, wallLayer);
        hit.collider == null;
        return hit.collider == null;
    }

    private void Move(Vector2 d) {
        transform.position += (Vector3)(d * speed) * Time.deltaTime;
        currentDirection = d;
    }
}
