using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckPoint : MonoBehaviour {
    public List<Vector2> directions = new List<Vector2>();
    public LayerMask obstacleLayer;

    public void Start() {
        if (ValidDirection(Vector2.up)) {
            directions.Add(Vector2.up);
        }

        if (ValidDirection(Vector2.right)) {
            directions.Add(Vector2.right);
        }

        if (ValidDirection(Vector2.down)) {
            directions.Add(Vector2.down);
        }

        if (ValidDirection(Vector2.left)) {
            directions.Add(Vector2.left);
        }
    }

    private bool ValidDirection(Vector2 direction) {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1f, obstacleLayer);

        return hit.collider == null;
    }
}
