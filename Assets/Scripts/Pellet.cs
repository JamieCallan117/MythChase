using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EatPellet();
        }
    }
}
