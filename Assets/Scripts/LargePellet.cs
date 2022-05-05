using UnityEngine;

//The large/power pellets to set enemies as vulnerable once eaten.
public class LargePellet : Pellet
{
    void Start()
    {
        points = 50;
    }

    //When the pellet is eaten.
    protected virtual void EatLargePellet()
    {
        gameManager.EatLargePellet(this);
    }

    //If a player collides with the pellet.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EatLargePellet();
        }
    }
}
