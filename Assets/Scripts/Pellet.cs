using UnityEngine;

public class Pellet : MonoBehaviour {
    public int points = 10;
    private GameManager gameManager;

    // This will call when the scene is loaded, so add all "this" objects to an array of some kind in GameManager so we can check
    // if they're all active to determine whether the round is complete or not and also to reactivate them in a new round.
    public void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddPellet(this);
    }

    protected virtual void EatPellet() {
        gameManager.EatPellet(this);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            EatPellet();
        }
    }
}
