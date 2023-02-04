using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    protected GameObject player;
    public GameObject prefab { get; private set; }
    [SerializeField] protected float jumpAreaSize;


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 colliderUpPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + GetComponent<Collider2D>().bounds.extents.y + 0.1f);
        Gizmos.DrawLine(new Vector2(colliderUpPos.x - jumpAreaSize / 2, colliderUpPos.y), new Vector2(colliderUpPos.x + jumpAreaSize / 2, colliderUpPos.y));
    }

    public virtual void Created(GameObject player, GameObject prefab)
    {
        this.player = player;
        this.prefab = prefab;
    }

    public virtual void Throw(Vector2 force)
    {
        gameObject.transform.parent = null;
        Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.velocity = force;
    }

    public virtual void WhenPickedUp() { }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector2 colliderUpPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + GetComponent<Collider2D>().bounds.extents.y + 0.1f);
            RaycastHit2D hit = Physics2D.Linecast(new Vector2(colliderUpPos.x - jumpAreaSize / 2, colliderUpPos.y), new Vector2(colliderUpPos.x + jumpAreaSize / 2, colliderUpPos.y), 256);
            if(hit)
            {
                hit.transform.gameObject.GetComponent<PickUpItems>().PickUpExistingItem(gameObject);
            }
            return;
        }
    }
}
