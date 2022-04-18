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

    private bool Move(Vector2 direction) {
        if (movement.ValidMove(direction) == true) {
            movement.ForceMove(direction);
            return true;
        } else {
            return false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (vulnerable == true) {
                print("Return to base");
            } else {
                gameManager.PlayerHit(this);
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoints")) {
            Vector3 playerPos = gameManager.GetPlayerPos();

            float xDistance = CalculateXDIstance(playerPos);
            float yDistance = CalculateYDistance(playerPos);

            bool hasMoved = false;

            if (Math.Abs(xDistance) > Math.Abs(yDistance)) {
                if (playerPos.x > this.transform.position.x) {
                    hasMoved = Move(Vector2.right);
                } else {
                    hasMoved = Move(Vector2.left);
                }
            } 
            
            if (hasMoved == false) {
                if (playerPos.y > this.transform.position.y) {
                    hasMoved = Move(Vector2.up);
                } else {
                    hasMoved = Move(Vector2.down);
                }
            }

            if (hasMoved == false) {
                if (Math.Abs(yDistance) > Math.Abs(xDistance)) {
                    if (playerPos.y > this.transform.position.y) {
                        hasMoved = Move(Vector2.down);
                    } else {
                        hasMoved = Move(Vector2.up);
                    }
                }
            }

            if (hasMoved == false) {
                    if (playerPos.x > this.transform.position.x) {
                        hasMoved = Move(Vector2.left);
                    } else {
                        hasMoved = Move(Vector2.right);
                    }
                }

            if (hasMoved == false) {
                print("Either my code is shit or something's gone wrong big time");
            }
        }
    }
}
