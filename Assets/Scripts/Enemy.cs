using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimatedSprites))]
public class Enemy : MonoBehaviour {
    private AnimatedSprites aniSprites;
    public Sprite[] regularSprites;
    public Sprite[] vulnerableSprites;

    private void Awake() {
        aniSprites = GetComponent<AnimatedSprites>();
    }

    public void SetVulnerable(bool vulnerable) {
        if (vulnerable == true) {
            aniSprites.sprites = vulnerableSprites;
        } else {
            aniSprites.sprites = regularSprites;
        }
    }
}
