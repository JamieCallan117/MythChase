using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=System.Random;

//The enemies in the game.
public class Enemy : MonoBehaviour
{
    private AnimatedSprites aniSprites;
    private Vector3 startingPos;
    private Sprite[] regularSprites;
    private Sprite[] vulnerableSprites;
    private Movement movement;
    private bool vulnerable;
    private bool powerUpVulnerable;
    private bool eaten;
    private bool inHome;
    private bool scared;
    private bool enteringHome;
    private bool leavingHome;
    private bool exitingHome;
    private GameManager gameManager;
    [SerializeField] private CircleCollider2D circleCollider;
    private Sprite eatenSprite;
    [SerializeField] private GameObject homePoint;
    [SerializeField] private GameObject inHomePoint;

    //Set default values for each enemy.
    void Awake()
    {
        aniSprites = GetComponent<AnimatedSprites>();
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = this.transform.position;
        scared = false;
        powerUpVulnerable = false;
        vulnerable = false;
        eaten = false;
        enteringHome = false;
        leavingHome = false;
        exitingHome = false;
    }

    void Update()
    {
        //When the enemy is entering the home after it has been eaten.
        if (enteringHome)
        {
            var step = movement.GetSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, inHomePoint.transform.position, step);

            if (Vector3.Distance(transform.position, inHomePoint.transform.position) < 0.001f)
            {
                circleCollider.enabled = true;
                eaten = false;
                enteringHome = false;
                inHome = true;
                aniSprites.enable(true);

                aniSprites.SetSprites(regularSprites);

                Random rand = new Random();
                int randInt = rand.Next(0, 2);

                if (randInt == 0)
                {
                    movement.Move(Vector2.left);
                }
                else
                {
                    movement.Move(Vector2.right);
                }

                gameManager.ReleaseEnemy(this);
            }
        }

        //Moves the enemy to the center of the home base ready for it to exit.
        if (leavingHome)
        {
            var step = movement.GetSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, inHomePoint.transform.position, step);

            if (Vector3.Distance(transform.position, inHomePoint.transform.position) < 0.001f)
            {
                exitingHome = true;
                leavingHome = false;
            }
        }

        //Makes the enemy exit the home base and then go either left or right.
        if (exitingHome)
        {
            var step = movement.GetSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, homePoint.transform.position, step);

            if (Vector3.Distance(transform.position, homePoint.transform.position) < 0.001f)
            {
                circleCollider.enabled = true;

                Random rand = new Random();
                int randInt = rand.Next(0, 2);

                if (randInt == 0)
                {
                    movement.Move(Vector2.left);
                }
                else
                {
                    movement.Move(Vector2.right);
                }
                
                exitingHome = false;
                inHome = false;
            }
        }
    }

    //Resets most values for the enemy.
    public void ResetEnemy()
    {
        scared = false;
        powerUpVulnerable = false;
        vulnerable = false;
        eaten = false;
        enteringHome = false;
        leavingHome = false;
        exitingHome = false;
        circleCollider.enabled = true;
        aniSprites.enable(true);

        aniSprites.SetSprites(regularSprites);
    }

    //Increases the speed of the enemy at each new round.
    public void IncreaseSpeed() {
        movement.IncreaseSpeed();
    }

    //Directly set the speed of an enemy.
    public void SetSpeed(float newSpeed)
    {
        movement.SetSpeed(newSpeed);
    }

    //Set if the enemy is vulnerable or not.
    public void SetVulnerable(bool vulnerable)
    {
        if (this.vulnerable == true)
        {
            CancelInvoke("UndoVulnerability");
        }

        if (eaten == false)
        {
            this.vulnerable = vulnerable;

            if (vulnerable == true)
            {
                aniSprites.SetSprites(vulnerableSprites);

                Invoke("UndoVulnerability", 10.0f);
            }
            else
            {
                aniSprites.SetSprites(regularSprites);
            }
        }
    }

    //Set an enemy as no longer vulnerable.
    private void UndoVulnerability()
    {
        SetVulnerable(false);
    }

    //Sets the regular sprites to be used by each enemy.
    public void SetRegularSprites(Sprite[] spritesToUse)
    {
        regularSprites = spritesToUse;
    }

    //Sets the vulnerable sprites to be used by each enemy.
    public void SetVulnerableSprites(Sprite[] spritesToUse)
    {
        vulnerableSprites = spritesToUse;
    }

    //Sets the sprite to be used when an enemy is eaten.
    public void SetEatenSprite(Sprite spriteToUse)
    {
        eatenSprite = spriteToUse;
    }

    //Makes the enemy vulnerable from the specific power up. Sprite doesn't change.
    public void SetPowerUpVulnerable(bool vul)
    {
        powerUpVulnerable = vul;
    }

    //Sets if an enemy is scared from the specific power up. Sprite doesn't change.
    public void SetScared(bool scare)
    {
        scared = scare;
    }

    //When an enemy gets eaten by the player.
    public void GetEaten()
    {
        SetVulnerable(false);
        powerUpVulnerable = false;
        scared = false;

        eaten = true;

        aniSprites.enable(false);
        aniSprites.SetSprite(eatenSprite);

        gameManager.EnemyEaten();
    }

    //Starts making an enemy leave the home base.
    public void Release()
    {
        movement.Move(Vector2.zero);
        leavingHome = true;
        circleCollider.enabled = false;
    }

    //Resets the enemy to its starting position.
    public void ResetPosition()
    {
        transform.position = startingPos;
    }

    //Toggles movement on or off for the enemy.
    public void ToggleMovement(bool enabled)
    {
        movement.ToggleMovement(enabled);
    }

    //Moves the enemy in the desired direction.
    public void Move(Vector2 direction)
    {
        movement.Move(direction);
    }

    //Makes the enemy not move.
    public void HaltMovement()
    {
        movement.HaltMovement();
    }

    //Sets if an enemy is inside the home base or not.
    public void InHome(bool atHome)
    {
        inHome = atHome;
    }

    //Toggles whether the enemy should collide with the player or not.
    public void ToggleIgnorePlayer(bool ignore, GameObject player)
    {
        if (ignore)
        {
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
        }
        else
        {
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>(), false);
        }
    }

    //For when the enemy collides with the player or if it's in the home base then bounce around.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (vulnerable == true || powerUpVulnerable == true)
            {
                GetEaten();
                OnCollisionEnter2D(other);
            }
            else if (eaten == true)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            }
            else
            {
                circleCollider.enabled = false;
                gameManager.PlayerHit();
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            if (inHome)
            {
                movement.Move(-movement.GetCurrentDirection());
            }
        }
    }

    //For when the enemy hits the checkpoints to determine movement.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoints"))
        {
            if (scared && !eaten && other.gameObject != homePoint)
            {
                RunAway(other);
            }
            else if (vulnerable && other.gameObject != homePoint)
            {
                RunAway(other);
            }
            else if (eaten && other.gameObject != homePoint)
            {
                ReturnHome(other);
            }
            else if (eaten && other.gameObject == homePoint)
            {
                EnterHome();
            }
            else if (!vulnerable && !eaten && other.gameObject != homePoint)
            {
                ChasePlayer(other);
            }
        }
    }

    //Starts the entering home base stage.
    private void EnterHome()
    {
        movement.Move(Vector2.zero);
        enteringHome = true;
        circleCollider.enabled = false;
    }

    //When an enemy is vulnerable or scared it moves in random directions.
    private void RunAway(Collider2D other)
    {
        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.GetAvailableDirections();

        Random rand = new Random();
        int randInt = rand.Next(0, availableDirections.Count);

        movement.Move(availableDirections[randInt]);
    }

    //When an enemy is eaten it heads towards the home base.
    private void ReturnHome(Collider2D other)
    {
        Vector3 homePos = homePoint.transform.position;

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.GetAvailableDirections();

        Vector3 direction = Vector3.zero;
        float minDistance = 1000.0f;

        foreach (Vector2 possibleDirection in availableDirections)
        {
            if (possibleDirection != -movement.GetCurrentDirection())
            {
                Vector3 testPosition = transform.position + new Vector3(possibleDirection.x, possibleDirection.y, -5);
                float distance = Vector3.Distance(homePos, testPosition);

                if (distance < minDistance)
                {
                    direction = possibleDirection;
                    minDistance = distance;
                }
            }
        }

        movement.Move(direction);
    }

    //In regular play, when the enemy is chasing the player.
    private void ChasePlayer(Collider2D other)
    {
        Vector3 playerPos = gameManager.GetPlayerPos();

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.GetAvailableDirections();

        Vector3 direction = Vector3.zero;
        float minDistance = 1000.0f;

        foreach (Vector2 possibleDirection in availableDirections)
        {
            if (possibleDirection != -movement.GetCurrentDirection())
            {
                Vector3 testPosition = transform.position + new Vector3(possibleDirection.x, possibleDirection.y, -5);
                float distance = Vector3.Distance(playerPos, testPosition);

                if (distance < minDistance)
                {
                    direction = possibleDirection;
                    minDistance = distance;
                }
            }
        }

        movement.Move(direction);
    }
}
