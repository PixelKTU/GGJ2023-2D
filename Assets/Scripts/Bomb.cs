using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ThrowableObject
{
    [SerializeField] float timeUntilExplosion;
    [SerializeField] float explosionRadius;
    [SerializeField] float maxExplosionForce;
    private float _time;
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
        _time = 0;
        _ticking = true;
    }

    public override void WhenPickedUp()
    {
        _ticking = false;
    }

    private void Update()
    {
        if (_ticking)
        {
            _time += Time.deltaTime;
            if(_time >= timeUntilExplosion)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, 256);
                foreach(Collider2D collider in colliders)
                {
                    Vector2 velocity = collider.gameObject.transform.position - transform.position;
                    collider.GetComponent<Rigidbody2D>().velocity = velocity.normalized * maxExplosionForce;
                }
                Destroy(gameObject);
            }
        }
    }
}
