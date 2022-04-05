using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimatedSprites))]
public class Enemy : MonoBehaviour {
    private AnimatedSprites aniSprites;
    private Vector3 startingPos;
    public Sprite[] regularSprites;
    public Sprite[] vulnerableSprites;
    public bool vulnerable;
    private GameManager gameManager;


    private void Awake() {
        aniSprites = GetComponent<AnimatedSprites>();
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (vulnerable == true) {
                print("Return to base");
            } else {
                gameManager.PlayerHit(this);
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoints")) {
            print("Make a choice");
        }
    }
}
