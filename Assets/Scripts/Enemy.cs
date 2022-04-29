using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
    private GameManager gameManager;
    public CircleCollider2D circleCollider;

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
    }

    private void Update() {
        if (enteringHome) {
            var step = movement.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, inHomePoint.transform.position, step);

            if (Vector3.Distance(transform.position, inHomePoint.transform.position) < 0.001f) {
                circleCollider.enabled = true;
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
    }

    public void GetEaten() {
        SetVulnerable(false);

        eaten = true;
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
            } else if (eaten && other.gameObject == inHomePoint) {
                eaten = false;
                enteringHome = false;
                inHome = true;
            }
        }
    }

    private void EnterHome() {
        movement.Move(Vector2.zero);
        enteringHome = true;
        circleCollider.enabled = false;
    }

    private void RunAway(Collider2D other) {
        Vector3 playerPos = gameManager.GetPlayerPos();

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Vector3 direction = Vector3.zero;
        float maxDistance = 0.0f;

        foreach (Vector2 possibleDirection in availableDirections) {
            Vector3 testPosition = transform.position + new Vector3(possibleDirection.x, possibleDirection.y, -5);
            float distance = Vector3.Distance(playerPos, testPosition);

            if (distance > maxDistance) {
                direction = possibleDirection;
                maxDistance = distance;
            }
        }

        movement.Move(direction);
    }

    private void ReturnHome(Collider2D other) {
        Vector3 homePos = homePoint.transform.position;

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Vector3 direction = Vector3.zero;
        float minDistance = 100.0f;

        foreach (Vector2 possibleDirection in availableDirections) {
            Vector3 testPosition = transform.position + new Vector3(possibleDirection.x, possibleDirection.y, -5);
            float distance = Vector3.Distance(homePos, testPosition);

            if (distance < minDistance) {
                direction = possibleDirection;
                minDistance = distance;
            }
        }

        movement.Move(direction);
    }

    private void ChasePlayer(Collider2D other) {
        Vector3 playerPos = gameManager.GetPlayerPos();

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        Vector3 direction = Vector3.zero;
        float minDistance = 100.0f;

        foreach (Vector2 possibleDirection in availableDirections) {
            Vector3 testPosition = transform.position + new Vector3(possibleDirection.x, possibleDirection.y, -5);
            float distance = Vector3.Distance(playerPos, testPosition);

            if (distance < minDistance) {
                direction = possibleDirection;
                minDistance = distance;
            }
        }

        movement.Move(direction);
    }
}
