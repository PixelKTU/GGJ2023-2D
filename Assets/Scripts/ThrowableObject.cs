using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    protected GameObject player;
    public GameObject prefab { get; private set; }
    [SerializeField] protected float jumpAreaSize;

    protected bool onGround = false;
    protected bool thrown = false;


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
        gameObject.layer = 10;
        gameObject.transform.parent = null;
        Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
        rigid.velocity = force;
        thrown = true;
    }

    public virtual void WhenPickedUp() {
        onGround = false;
        thrown = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && thrown)
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

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && thrown)
        {
            Vector2 colliderUpPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + GetComponent<Collider2D>().bounds.extents.y + 0.1f);
            RaycastHit2D hit = Physics2D.Linecast(new Vector2(colliderUpPos.x - jumpAreaSize / 2, colliderUpPos.y), new Vector2(colliderUpPos.x + jumpAreaSize / 2, colliderUpPos.y), 256);
            if (hit)
            {
                hit.transform.gameObject.GetComponent<PickUpItems>().PickUpExistingItem(gameObject);
            }
            return;
        }
    }

    protected virtual void Update()
    {
        if (thrown)
        {
            Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
            if (rigid.velocity.x * rigid.velocity.x + rigid.velocity.y * rigid.velocity.y == 0)
            {
                onGround = true;
            }
        }
    }
}
