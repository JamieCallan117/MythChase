using UnityEngine;

//Moves both player and enemy objects around.
public class Movement : MonoBehaviour
{
    private bool movementEnabled = false;
    private float speed = 8.0f;
    [SerializeField] private LayerMask obstacleLayer;
    private Rigidbody2D rigidbody2d;
    private Vector2 currentDirection;
    private Vector2 nextDirection;
    private float yUpper = 0.0f;
    private float yLower = 0.0f;
    private float xLeft = 0.0f;
    private float xRight = 0.0f;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        //Used for the "tunnels" that send you from one side of the map to the other.
        yUpper = 13.4f;
        yLower = -16.4f;

        if (DataStorage.level == 1)
        {
            xLeft = -0.5f;
            xRight = 0.5f;
        }
        else
        {
            xLeft = -13.4f;
            xRight = 13.4f;
        }
    }

    void Update()
    {
        //Used for moving in the "queued" direction.
        if (nextDirection != Vector2.zero)
        {
            Move(nextDirection);
        }
    }

    //Actually moves the player/enemy and also checks if it has entered a tunnel and needs to be sent to the opposite side.
    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        Vector2 translation = currentDirection * speed * Time.fixedDeltaTime;

        if (movementEnabled == true)
        {
            rigidbody2d.MovePosition(position + translation);
        }

        //Level 1 has its tunnels on a different x value.
        if (DataStorage.level == 1)
        {
            if (transform.position.y >= yUpper)
            {
                transform.position = new Vector3(xLeft, (yLower + 0.1f), -5.0f);
            }
            else if (transform.position.y <= yLower)
            {
                transform.position = new Vector3(xRight, (yUpper - 0.1f), -5.0f);
            }
        }
        else
        {
            //Level two and threes tunnels are directly opposite.
            if (transform.position.y >= yUpper)
            {
                transform.position = new Vector3(transform.position.x, (yLower + 0.1f), -5.0f);
            }
            else if (transform.position.y <= yLower)
            {
                transform.position = new Vector3(transform.position.x, (yUpper - 0.1f), -5.0f);
            }
            else if (transform.position.x >= xRight)
            {
                transform.position = new Vector3((xLeft + 0.1f), transform.position.y, -5.0f);
            }
            else if (transform.position.x <= xLeft)
            {
                transform.position = new Vector3((xRight - 0.1f), transform.position.y, -5.0f);
            }
        }
    
    }

    //Toggles movement on/off.
    public void ToggleMovement(bool enabled)
    {
        movementEnabled = enabled;
    }

    //Halts all movement for the object.
    public void HaltMovement()
    {
        currentDirection = Vector2.zero;
        nextDirection = Vector2.zero;
    }

    //Teleports an object to a specific location.
    public void TeleportObject(Vector3 newPos) {
        transform.position = newPos;
    }

    //Sets the direction to move either now or in the "queue"
    public void Move(Vector2 direction)
    {
        if (ValidMove(direction))
        {
            currentDirection = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    //Increases movement speed.
    public void IncreaseSpeed() {
        speed += 0.1f;
    }

    //Sets movement speed.
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    //Gets movement speed.
    public float GetSpeed()
    {
        return speed;
    }

    //Gets current direction.
    public Vector2 GetCurrentDirection()
    {
        return currentDirection;
    }

    //Checks if the player/enemy can move in the selected direction.
    private bool ValidMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1f, obstacleLayer);

        return hit.collider == null;
    }

}
