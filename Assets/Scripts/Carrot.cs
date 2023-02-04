using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : ThrowableObject
{
    [SerializeField] int damage;
    [SerializeField] float carrotXVelocity;
    [SerializeField] Vector2 carrotStartingForce;
    [SerializeField] float timeUntilFlying;
    private float time;
    private bool flying;

    Rigidbody2D rigid;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (!onGround && collision.gameObject.tag == "Player" && collision.gameObject != player)
        {
            // when carrot hits player
            collision.gameObject.GetComponent<PlayerHealth>().RemoveHealth(damage);
            Destroy(gameObject);
        }else if (collision.gameObject.layer == 7 || collision.gameObject.layer == 10 || (flying && collision.gameObject.tag == "Player" && collision.gameObject == player))
        {
            // when carrot hits ground or other object
            Destroy(gameObject);
        }
    }

    public override void Throw(Vector2 force)
    {
        gameObject.layer = 10;
        gameObject.transform.parent = null;
        
        thrown = true;
        rigid = GetComponent<Rigidbody2D>();
        if (force.x < 0)
        {
            carrotXVelocity *= -1;
            carrotStartingForce = new Vector2(-carrotStartingForce.x, carrotStartingForce.y);
        }
        rigid.velocity = carrotStartingForce;
        time = timeUntilFlying;
        flying = false;
        
    }

    protected override void Update()
    {
        if (thrown) {
            
            if (time <= 0)
            {
                if (!flying)
                {
                    flying = true;
                    rigid.bodyType = RigidbodyType2D.Kinematic;
                }
                rigid.velocity = new Vector2(carrotXVelocity, 0);
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }
}
