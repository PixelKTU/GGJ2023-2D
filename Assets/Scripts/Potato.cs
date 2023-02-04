using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : ThrowableObject
{
    [SerializeField] int damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject != player)
        {
            collision.gameObject.GetComponent<PlayerHealth>().RemoveHealth(damage);
        }
    }
}
