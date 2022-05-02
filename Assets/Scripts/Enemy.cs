using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=System.Random;

[RequireComponent(typeof(AnimatedSprites))]
public class Enemy : MonoBehaviour
{
    private AnimatedSprites aniSprites;
    private Vector3 startingPos;
    public Sprite[] regularSprites;
    public Sprite[] vulnerableSprites;
    private Movement movement;
    private bool vulnerable;
    public bool powerUpVulnerable;
    private bool eaten;
    public bool inHome;
    public bool scared;
    private bool enteringHome;
    private bool leavingHome;
    private bool exitingHome;
    private GameManager gameManager;
    private CircleCollider2D circleCollider;
    public Sprite eatenSprite;
    [SerializeField] private GameObject homePoint;
    [SerializeField] private GameObject inHomePoint;

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
        if (enteringHome)
        {
            var step = movement.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, inHomePoint.transform.position, step);

            if (Vector3.Distance(transform.position, inHomePoint.transform.position) < 0.001f)
            {
                circleCollider.enabled = true;
                eaten = false;
                enteringHome = false;
                inHome = true;
                aniSprites.enable(true);

                aniSprites.sprites = regularSprites;

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

        if (leavingHome)
        {
            var step = movement.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, inHomePoint.transform.position, step);

            if (Vector3.Distance(transform.position, inHomePoint.transform.position) < 0.001f)
            {
                exitingHome = true;
                leavingHome = false;
            }
        }

        if (exitingHome)
        {
            var step = movement.speed * Time.deltaTime;
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
        aniSprites.sprites = regularSprites;
    }

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
                aniSprites.sprites = vulnerableSprites;

                Invoke("UndoVulnerability", 10.0f);
            }
            else
            {
                aniSprites.sprites = regularSprites;
            }
        }
    }

    private void UndoVulnerability()
    {
        SetVulnerable(false);
    }

    public void GetEaten()
    {
        SetVulnerable(false);
        powerUpVulnerable = false;
        scared = false;

        eaten = true;

        aniSprites.enable(false);
        aniSprites.spriteRenderer.sprite = eatenSprite;

        gameManager.EnemyEaten();
    }

    public void Release()
    {
        movement.Move(Vector2.zero);
        leavingHome = true;
        circleCollider.enabled = false;
    }

    public void ResetPosition()
    {
        this.transform.position = startingPos;
    }

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
                movement.Move(-movement.currentDirection);
            }
        }
    }

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

    private void EnterHome()
    {
        movement.Move(Vector2.zero);
        enteringHome = true;
        circleCollider.enabled = false;
    }

    private void RunAway(Collider2D other)
    {
        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Random rand = new Random();
        int randInt = rand.Next(0, availableDirections.Count);

        movement.Move(availableDirections[randInt]);
    }

    private void ReturnHome(Collider2D other)
    {
        Vector3 homePos = homePoint.transform.position;

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Vector3 direction = Vector3.zero;
        float minDistance = 1000.0f;

        foreach (Vector2 possibleDirection in availableDirections)
        {
            if (possibleDirection != -movement.currentDirection)
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

    private void ChasePlayer(Collider2D other)
    {
        Vector3 playerPos = gameManager.GetPlayerPos();

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Vector3 direction = Vector3.zero;
        float minDistance = 1000.0f;

        foreach (Vector2 possibleDirection in availableDirections)
        {
            if (possibleDirection != -movement.currentDirection)
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
