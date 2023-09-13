using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : ThrowableObject
{
    [SerializeField] float timeUntilExplosion;
    [SerializeField] float explosionRadius;
    [SerializeField] float maxExplosionForce;
    [SerializeField] float stunDuration;
    [SerializeField] int explosionDamage;
    private float _time = 0;
    private bool _ticking = false;


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, explosionRadius);
    }
    

    public override void Throw(Vector2 force)
    {
        base.Throw(force);
        _ticking = true;
    }

    protected override void Update()
    {
        base.Update();
        if (_ticking)
        {
            _time += Time.deltaTime;
            if(_time >= timeUntilExplosion)
            {
               
                Explode();
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (thrown && _ticking && collision.gameObject.tag == "Player" && collision.gameObject != player)
        {
            Explode();
        }
    }

    private void Explode()
    {
        _ticking = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, 256);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (transform.parent != null)
                {
                    transform.parent.parent.GetComponent<PickUpItems>().ItemRemovedFromHands();
                }
                PlayerHealth hp = collider.GetComponent<PlayerHealth>();
                hp.RemoveHealth(explosionDamage);
                if (!hp.dead)
                {
                    Vector2 dir = (collider.gameObject.transform.position - transform.position).normalized;
                    collider.GetComponent<Rigidbody2D>().velocity = dir * maxExplosionForce;
                    collider.GetComponent<CharacterController2D>().Stun(stunDuration);
                    
                }
            }
        }
        GetComponent<Animator>().SetBool("Explode", true);
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    public void DestroyGameobject()
    {
        Destroy(gameObject);
    }
}
