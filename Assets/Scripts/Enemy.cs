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
    private GameManager gameManager;

    private void Awake() {
        aniSprites = GetComponent<AnimatedSprites>();
        movement = GetComponent<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        startingPos = this.transform.position;
        vulnerable = false;
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
        this.vulnerable = false;
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (vulnerable == true) {
                print("Return to base");
            } else {
                gameManager.PlayerHit();
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoints")) {
            Vector3 playerPos = gameManager.GetPlayerPos();

            float xDistance = CalculateXDIstance(playerPos);
            float yDistance = CalculateYDistance(playerPos);

            CheckPoint checkPointHit = other.GetComponent<CheckPoint>();

            List<Vector2> availableDirections = checkPointHit.directions;

            bool hasMoved = false;

            if (Math.Abs(xDistance) > Math.Abs(yDistance)) {
                if (playerPos.x > this.transform.position.x && movement.currentDirection != Vector2.left && availableDirections.Contains(Vector2.right)) {
                    movement.Move(Vector2.right);

                    hasMoved = true;
                } else if (movement.currentDirection != Vector2.right && availableDirections.Contains(Vector2.left)) {
                    movement.Move(Vector2.left);

                    hasMoved = true;
                }
            }

            if (hasMoved == false) {
                if (playerPos.y > this.transform.position.y && movement.currentDirection != Vector2.down && availableDirections.Contains(Vector2.up)) {
                    movement.Move(Vector2.up);

                    hasMoved = true;
                } else if (playerPos.y < this.transform.position.y && movement.currentDirection != Vector2.up && availableDirections.Contains(Vector2.down)) {
                    movement.Move(Vector2.down);

                    hasMoved = true;
                }
            }

            if (hasMoved == false && Math.Abs(xDistance) <= Math.Abs(yDistance)) {
                if (playerPos.x > this.transform.position.x && movement.currentDirection != Vector2.left && availableDirections.Contains(Vector2.right)) {
                    movement.Move(Vector2.right);

                    hasMoved = true;
                } else if (movement.currentDirection != Vector2.right && availableDirections.Contains(Vector2.left)) {
                    movement.Move(Vector2.left);

                    hasMoved = true;
                }
            }

            if (hasMoved == false && Math.Abs(yDistance) > Math.Abs(xDistance)) {
                if (playerPos.y > this.transform.position.y && availableDirections.Contains(Vector2.down)) {
                    movement.Move(Vector2.down);
                    
                    hasMoved = true;
                } else if (playerPos.y < this.transform.position.y && availableDirections.Contains(Vector2.up)) {
                    movement.Move(Vector2.up);
                    
                    hasMoved = true;
                }
            }

            if (hasMoved == false) {
                if (playerPos.x > this.transform.position.x && availableDirections.Contains(Vector2.left)) {
                    movement.Move(Vector2.left);
                    
                    hasMoved = true;
                } else if (playerPos.x < this.transform.position.x && availableDirections.Contains(Vector2.right)) {
                    movement.Move(Vector2.right);
                    
                    hasMoved = true;
                }
            }
        }
    }
}
