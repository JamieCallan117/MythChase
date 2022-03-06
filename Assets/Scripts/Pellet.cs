using UnityEngine;

public class Pellet : MonoBehaviour {
    public int points = 10;

    protected virtual void EatPellet() {
        FindObjectOfType<GameManager>().EatPellet(this);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            EatPellet();
        }
    }
}
