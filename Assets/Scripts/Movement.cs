using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

        yUpper = 13.4f;
        yLower = -16.4f;

        if (PlayerStats.level == 1)
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
        if (nextDirection != Vector2.zero)
        {
            Move(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        Vector2 translation = currentDirection * speed * Time.fixedDeltaTime;

        if (movementEnabled == true)
        {
            rigidbody2d.MovePosition(position + translation);
        }

        if (PlayerStats.level == 1)
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

    public void ToggleMovement(bool enabled)
    {
        movementEnabled = enabled;
    }

    public void HaltMovement()
    {
        currentDirection = Vector2.zero;
        nextDirection = Vector2.zero;
    }

    public void TeleportObject(Vector3 newPos) {
        transform.position = newPos;
    }

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

    public void IncreaseSpeed() {
        speed += 0.1f;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public Vector2 GetCurrentDirection()
    {
        return currentDirection;
    }

    private bool ValidMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1f, obstacleLayer);

        return hit.collider == null;
    }

}
