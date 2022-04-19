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
    private GameManager gameManager;

    public GameObject homePoint;

    private void Awake() {
        aniSprites = GetComponent<AnimatedSprites>();
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = this.transform.position;
        vulnerable = false;
        eaten = false;
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

        //Need to disable collision with player after eaten
    }

    public void ResetPosition() {
        this.transform.position = startingPos;
    }

    private float CalculateXDIstance(Vector3 playerPos) {
        return playerPos.x - this.transform.position.x;
    }

    private float CalculateYDistance(Vector3 playerPos) {
        return playerPos.y - this.transform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (vulnerable == true) {
                GetEaten();
            } else {
                gameManager.PlayerHit();
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

    }

    private void RunAway(Collider2D other) {
        Vector3 playerPos = gameManager.GetPlayerPos();

        float xDistance = CalculateXDIstance(playerPos);
        float yDistance = CalculateYDistance(playerPos);

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        bool hasMoved = false;

        if (Math.Abs(xDistance) > Math.Abs(yDistance)) {
            if (playerPos.x > this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);

                hasMoved = true;
            } else if (playerPos.x < this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);

                hasMoved = true;
            }
        }

        if (hasMoved == false) {
            if (playerPos.y > this.transform.position.y && availableDirections.Contains(Vector2.down) && movement.currentDirection != Vector2.up) {
                movement.Move(Vector2.down);

                hasMoved = true;
            } else if (playerPos.y < this.transform.position.y && availableDirections.Contains(Vector2.up) && movement.currentDirection != Vector2.down) {
                movement.Move(Vector2.up);

                hasMoved = true;
            }
        }

        if (hasMoved == false && Math.Abs(xDistance) <= Math.Abs(yDistance)) {
            if (playerPos.x > this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);

                hasMoved = true;
            } else if (playerPos.x < this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);

                hasMoved = true;
            }
        }

        if (hasMoved == false && Math.Abs(yDistance) > Math.Abs(xDistance)) {
            if (playerPos.y > this.transform.position.y && availableDirections.Contains(Vector2.up) && movement.currentDirection != Vector2.down) {
                movement.Move(Vector2.up);
                
                hasMoved = true;
            } else if (playerPos.y < this.transform.position.y && availableDirections.Contains(Vector2.down) && movement.currentDirection != Vector2.up) {
                movement.Move(Vector2.down);
                
                hasMoved = true;
            }
        }

        if (hasMoved == false) {
            if (playerPos.x > this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);
                
                hasMoved = true;
            } else if (playerPos.x < this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);
                
                hasMoved = true;
            }
        }
    }

    private void ReturnHome(Collider2D other) {
        Vector3 homePos = homePoint.transform.position;

        float xDistance = CalculateXDIstance(homePos);
        float yDistance = CalculateYDistance(homePos);

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        bool hasMoved = false;

        if (Math.Abs(xDistance) > Math.Abs(yDistance)) {
            if (homePos.x > this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);

                hasMoved = true;
            } else if (homePos.x < this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);

                hasMoved = true;
            }
        }

        if (hasMoved == false) {
            if (homePos.y > this.transform.position.y && availableDirections.Contains(Vector2.up) && movement.currentDirection != Vector2.down) {
                movement.Move(Vector2.up);

                hasMoved = true;
            } else if (homePos.y < this.transform.position.y && availableDirections.Contains(Vector2.down) && movement.currentDirection != Vector2.up) {
                movement.Move(Vector2.down);

                hasMoved = true;
            }
        }

        if (hasMoved == false && Math.Abs(xDistance) <= Math.Abs(yDistance)) {
            if (homePos.x > this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);

                hasMoved = true;
            } else if (homePos.x < this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);

                hasMoved = true;
            }
        }

        if (hasMoved == false && Math.Abs(yDistance) > Math.Abs(xDistance)) {
            if (homePos.y > this.transform.position.y && availableDirections.Contains(Vector2.down) && movement.currentDirection != Vector2.up) {
                movement.Move(Vector2.down);
                
                hasMoved = true;
            } else if (homePos.y < this.transform.position.y && availableDirections.Contains(Vector2.up) && movement.currentDirection != Vector2.down) {
                movement.Move(Vector2.up);
                
                hasMoved = true;
            }
        }

        if (hasMoved == false) {
            if (homePos.x > this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);
                
                hasMoved = true;
            } else if (homePos.x < this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);
                
                hasMoved = true;
            }
        }
    }

    private void ChasePlayer(Collider2D other) {
        Vector3 playerPos = gameManager.GetPlayerPos();

        float xDistance = CalculateXDIstance(playerPos);
        float yDistance = CalculateYDistance(playerPos);

        CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

        List<Vector2> availableDirections = checkPointHit.directions;

        bool hasMoved = false;

        if (Math.Abs(xDistance) > Math.Abs(yDistance)) {
            if (playerPos.x > this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);

                hasMoved = true;
            } else if (playerPos.x < this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);

                hasMoved = true;
            }
        }

        if (hasMoved == false) {
            if (playerPos.y > this.transform.position.y && availableDirections.Contains(Vector2.up) && movement.currentDirection != Vector2.down) {
                movement.Move(Vector2.up);

                hasMoved = true;
            } else if (playerPos.y < this.transform.position.y && availableDirections.Contains(Vector2.down) && movement.currentDirection != Vector2.up) {
                movement.Move(Vector2.down);

                hasMoved = true;
            }
        }

        if (hasMoved == false && Math.Abs(xDistance) <= Math.Abs(yDistance)) {
            if (playerPos.x > this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);

                hasMoved = true;
            } else if (playerPos.x < this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);

                hasMoved = true;
            }
        }

        if (hasMoved == false && Math.Abs(yDistance) > Math.Abs(xDistance)) {
            if (playerPos.y > this.transform.position.y && availableDirections.Contains(Vector2.down) && movement.currentDirection != Vector2.up) {
                movement.Move(Vector2.down);
                
                hasMoved = true;
            } else if (playerPos.y < this.transform.position.y && availableDirections.Contains(Vector2.up) && movement.currentDirection != Vector2.down) {
                movement.Move(Vector2.up);
                
                hasMoved = true;
            }
        }

        if (hasMoved == false) {
            if (playerPos.x > this.transform.position.x && availableDirections.Contains(Vector2.left) && movement.currentDirection != Vector2.right) {
                movement.Move(Vector2.left);
                
                hasMoved = true;
            } else if (playerPos.x < this.transform.position.x && availableDirections.Contains(Vector2.right) && movement.currentDirection != Vector2.left) {
                movement.Move(Vector2.right);
                
                hasMoved = true;
            }
        }
    }
}
