using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetroot : ThrowableObject
{
    [SerializeField] int damage;
    [SerializeField] float stunSize;
    [SerializeField] float knowbackForce;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if(!onGround && collision.gameObject.tag == "Player" && collision.gameObject != player)
        {
            PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
            hp.RemoveHealth(damage);
            if (!hp.dead)
            {
                collision.gameObject.GetComponent<CharacterController2D>().Stun(stunSize);
                Vector2 direction = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = direction * knowbackForce;
                onGround = true;
                
            }
        }
    }
}
