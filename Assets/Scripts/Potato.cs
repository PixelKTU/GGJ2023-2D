using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : ThrowableObject
{
    [SerializeField] int damage;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if(!onGround && collision.gameObject.tag == "Player" && collision.gameObject != player)
        {
            collision.gameObject.GetComponent<PlayerHealth>().RemoveHealth(damage);
        }
    }
}
