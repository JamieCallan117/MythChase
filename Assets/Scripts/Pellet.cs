using UnityEngine;

public class Pellet : MonoBehaviour
{
    protected int points = 10;
    protected GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddPellet(this);
    }

    protected virtual void EatPellet()
    {
        gameManager.EatPellet(this);
    }

    public int GetPoints()
    {
        return points;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EatPellet();
        }
    }
}
