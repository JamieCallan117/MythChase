using UnityEngine;

//The normal pellets found on each level.
public class Pellet : MonoBehaviour
{
    protected int points = 10;
    protected GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddPellet(this);
    }

    //When a pellet is eaten.
    protected virtual void EatPellet()
    {
        gameManager.EatPellet(this);
    }

    //Get the number of points the pellet is worth.
    public int GetPoints()
    {
        return points;
    }

    //When a player collides with the pellet.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EatPellet();
        }
    }
}
