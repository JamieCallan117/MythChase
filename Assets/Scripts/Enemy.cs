using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=System.Random;


[RequireComponent(typeof(AnimatedSprites))]
public class Enemy : MonoBehaviour {
    private AnimatedSprites aniSprites;
    private Vector3 startingPos;
    public Sprite[] regularSprites;
    public Sprite[] vulnerableSprites;
    private Movement movement;
    public bool vulnerable;
    public bool eaten;
    public bool inHome;
    private bool enteringHome;
    private bool leavingHome;
    private bool exitingHome;
    private GameManager gameManager;
    public CircleCollider2D circleCollider;
    public Sprite eatenSprite;

    public GameObject homePoint;
    public GameObject inHomePoint;

    private void Awake() {
        aniSprites = GetComponent<AnimatedSprites>();
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = this.transform.position;
        vulnerable = false;
        eaten = false;
        enteringHome = false;
        leavingHome = false;
        exitingHome = false;
    }

    private void Update() {
        if (enteringHome) {
            var step = movement.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, inHomePoint.transform.position, step);

            if (Vector3.Distance(transform.position, inHomePoint.transform.position) < 0.001f) {
                circleCollider.enabled = true;
                eaten = false;
                enteringHome = false;
                inHome = true;
                aniSprites.isEnabled = true;

                Random rand = new Random();
                int randInt = rand.Next(0, 2);

                if (randInt == 0) {
                    movement.Move(Vector2.left);
                } else {
                    movement.Move(Vector2.right);
                }
            }
        }

        if (leavingHome) {
            var step = movement.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, inHomePoint.transform.position, step);

            if (Vector3.Distance(transform.position, inHomePoint.transform.position) < 0.001f) {
                exitingHome = true;
                leavingHome = false;
            }
        }

        if (exitingHome) {
            var step = movement.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, homePoint.transform.position, step);

            if (Vector3.Distance(transform.position, homePoint.transform.position) < 0.001f) {
                circleCollider.enabled = true;

                Random rand = new Random();
                int randInt = rand.Next(0, 2);

                if (randInt == 0) {
                    movement.Move(Vector2.left);
                } else {
                    movement.Move(Vector2.right);
                }
                
                exitingHome = false;
                inHome = false;
            }
        }
    }

    public void SetVulnerable(bool vulnerable) {
        this.vulnerable = vulnerable;

        if (this.vulnerable == true) {
            aniSprites.sprites = vulnerableSprites;
        } else {
            aniSprites.sprites = regularSprites;
        }

        StartCoroutine(UndoVulnerability());
    }

    private IEnumerator UndoVulnerability() {
        yield return new WaitForSecondsRealtime(10);

        SetVulnerable(false);
    }

    public void GetEaten() {
        SetVulnerable(false);

        eaten = true;

        aniSprites.enable(false);
        aniSprites.spriteRenderer.sprite = eatenSprite;
    }

    public void Release() {
        movement.Move(Vector2.zero);
        leavingHome = true;
        circleCollider.enabled = false;
    }

    public void ResetPosition() {
        this.transform.position = startingPos;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (vulnerable == true) {
                GetEaten();
                OnCollisionEnter2D(other);
            } else if (eaten == true) {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            } else {
                gameManager.PlayerHit();
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Walls")) {
            if (inHome) {
                movement.Move(-movement.currentDirection);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoints")) {
            if (vulnerable && other.gameObject != homePoint) {
                RunAway(other);
            } else if (eaten && other.gameObject != homePoint) {
                ReturnHome(other);
            } else if (eaten && other.gameObject == homePoint) {
                EnterHome();
            } else if (!vulnerable && !eaten && other.gameObject != homePoint) {
                ChasePlayer(other);
            }
        }
    }

    private void EnterHome() {
        movement.Move(Vector2.zero);
        enteringHome = true;
        circleCollider.enabled = false;
    }

    private void RunAway(Collider2D other) {
        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Random rand = new Random();
        int randInt = rand.Next(0, availableDirections.Count);

        movement.Move(availableDirections[randInt]);
    }

    private void ReturnHome(Collider2D other) {
        Vector3 homePos = homePoint.transform.position;

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Vector3 direction = Vector3.zero;
        float minDistance = 1000.0f;

        foreach (Vector2 possibleDirection in availableDirections) {
            if (possibleDirection != -movement.currentDirection) {
                Vector3 testPosition = transform.position + new Vector3(possibleDirection.x, possibleDirection.y, -5);
                float distance = Vector3.Distance(homePos, testPosition);

                if (distance < minDistance) {
                    direction = possibleDirection;
                    minDistance = distance;
                }
            }
        }

        movement.Move(direction);
    }

    private void ChasePlayer(Collider2D other) {
        Vector3 playerPos = gameManager.GetPlayerPos();

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Vector3 direction = Vector3.zero;
        float minDistance = 1000.0f;

        foreach (Vector2 possibleDirection in availableDirections) {
            if (possibleDirection != -movement.currentDirection) {
                Vector3 testPosition = transform.position + new Vector3(possibleDirection.x, possibleDirection.y, -5);
                float distance = Vector3.Distance(playerPos, testPosition);

                if (distance < minDistance) {
                    direction = possibleDirection;
                    minDistance = distance;
                }
            }
        }

        movement.Move(direction);
    }
}
