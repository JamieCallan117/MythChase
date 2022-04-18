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

    public LayerMask obstacleLayer;

    private int counter = 0;


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
        // if (movement.ValidMove(direction) == true) {
        //     movement.ForceMove(direction);
        //     //print("Valid move in direction: " + direction.ToString());
        //     return true;
        // } else {
        //     return false;
        // }

        return movement.EnemyMove(direction);
    }

    private void OnTriggerStay2D(Collider2D other) {
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

            bool hasMoved = false;

            Vector2 cd = movement.currentDirection;

            if (cd.x == 0.0 && cd.y == 1.0) {
                print("Direction: Up " + counter);
            } else if (cd.x == 0.0 && cd.y == -1.0) {
                print("Direction: Down " + counter);
            } else if (cd.x == 1.0 && cd.y == 0.0) {
                print("Direction: Right " + counter);
            } else if (cd.x == -1.0 && cd.y == 0.0) {
                print("Direction: Left " + counter);
            }

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, movement.currentDirection, 1.0f, obstacleLayer);

            double distance = Math.Round(hit.distance, 1);

            if (Math.Abs(xDistance) > Math.Abs(yDistance)) {
                if (playerPos.x > this.transform.position.x && movement.currentDirection != Vector2.left) {
                    Move(Vector2.right);
                    print("Wants right " + counter);

                    if (movement.currentDirection == Vector2.right) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    if (distance == 0.5) {
                        hasMoved = false;
                    }
                } else if (playerPos.x < this.transform.position.x && movement.currentDirection != Vector2.right) {
                    Move(Vector2.left);
                    print("Wants left " + counter);

                    if (movement.currentDirection == Vector2.left) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    if (distance == 0.5) {
                        hasMoved = false;
                    }
                }
            } 
            
            if (hasMoved == false) {
                if (playerPos.y > this.transform.position.y && movement.currentDirection != Vector2.down) {
                    Move(Vector2.up);
                    print("Wants up " + counter);

                    if (movement.currentDirection == Vector2.up) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    if (distance == 0.5) {
                        hasMoved = false;
                    }
                } else if (playerPos.y < this.transform.position.y && movement.currentDirection != Vector2.up) {
                    Move(Vector2.down);
                    print("Wants down " + counter);

                    if (movement.currentDirection == Vector2.down) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    if (distance == 0.5) {
                        hasMoved = false;
                    }
                }
            }

            if (hasMoved == false) {
                if (playerPos.x > this.transform.position.x && movement.currentDirection != Vector2.left) {
                    Move(Vector2.right);
                    print("Forced but still needed right " + counter);

                    if (movement.currentDirection == Vector2.right) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    // if (distance == 0.5) {
                    //     hasMoved = false;
                    // }
                } else if (playerPos.x < this.transform.position.x && movement.currentDirection != Vector2.right) {
                    Move(Vector2.left);
                    print("Forced but still needed left " + counter);

                    if (movement.currentDirection == Vector2.left) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    // if (distance == 0.5) {
                    //     hasMoved = false;
                    // }
                }
            }

            if (hasMoved == false) {
                if (Math.Abs(yDistance) > Math.Abs(xDistance)) {
                    if (playerPos.y > this.transform.position.y) {
                        Move(Vector2.down);
                        print("Forced down " + counter);

                        if (movement.currentDirection == Vector2.down) {
                            hasMoved = true;
                        } else {
                            hasMoved = false;
                        }

                        // if (distance == 0.5) {
                        //     hasMoved = false;
                        // }
                    } else if (playerPos.y < this.transform.position.y) {
                        Move(Vector2.up);
                        print("Forced up " + counter);

                        if (movement.currentDirection == Vector2.up) {
                            hasMoved = true;
                        } else {
                            hasMoved = false;
                        }

                        // if (distance == 0.5) {
                        //     hasMoved = false;
                        // }
                    }
                }
            }

            if (hasMoved == false) {
                if (playerPos.x > this.transform.position.x) {
                    Move(Vector2.left);
                    print("Forced left " + counter);

                    if (movement.currentDirection == Vector2.left) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    // if (distance == 0.5) {
                    //     hasMoved = false;
                    // }
                } else if (playerPos.x < this.transform.position.x) {
                    Move(Vector2.right);
                    print("Forced right " + counter);

                    if (movement.currentDirection == Vector2.right) {
                        hasMoved = true;
                    } else {
                        hasMoved = false;
                    }

                    // if (distance == 0.5) {
                    //     hasMoved = false;
                    // }
                }
            }

            if (hasMoved == false) {
                print("No valid move " + counter);
            } else {
                cd = movement.currentDirection;

                if (cd.x == 0.0 && cd.y == 1.0) {
                    print("Successfully moved: Up " + counter);
                } else if (cd.x == 0.0 && cd.y == -1.0) {
                    print("Successfully moved: Down " + counter);
                } else if (cd.x == 1.0 && cd.y == 0.0) {
                    print("Successfully moved: Right " + counter);
                } else if (cd.x == -1.0 && cd.y == 0.0) {
                    print("Successfully moved: Left " + counter);
                } 
            }

            counter++;
        }
    }
}
