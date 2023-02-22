using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : ThrowableObject
{

    [SerializeField] int damage;
    [SerializeField] float stunSize;
    [SerializeField] float knowbackForce;
    [SerializeField] float onionLivingTime;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (!onGround && collision.gameObject.CompareTag("Player") && collision.gameObject != player)
        {
            collision.gameObject.GetComponent<PlayerHealth>().RemoveHealth(damage);
            collision.gameObject.GetComponent<CharacterController2D>().Stun(stunSize);
            Vector2 direction = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = direction * knowbackForce;
            onGround = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (onionLivingTime > 0)
        {
            onionLivingTime -= Time.deltaTime;
            if (onionLivingTime <= 0 && !thrown)
            {
                // Runs when player loses shield
            }
        }
    }
    public override void Created(GameObject player, GameObject prefab)
    {
        base.Created(player, prefab);
        // Runs when player gains shield
    }
}
