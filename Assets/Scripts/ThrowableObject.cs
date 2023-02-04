using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    protected GameObject player;

    public virtual void Created(GameObject player)
    {
        this.player = player;
    }

    public virtual void Throw(Vector2 force)
    {
        gameObject.transform.parent = null;
        Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.velocity = force;
    }
}
