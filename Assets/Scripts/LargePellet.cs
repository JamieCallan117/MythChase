using UnityEngine;

public class LargePellet : Pellet
{
    void Start()
    {
        points = 50;
    }

    protected virtual void EatLargePellet()
    {
        gameManager.EatLargePellet(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EatLargePellet();
        }
    }
}
