using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ThrowableObject
{
    [SerializeField] float timeUntilExplosion;
    [SerializeField] int explosionDamage;
    [SerializeField] float explosionRadius;
    private float _time;
    private bool _ticking = false;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, explosionRadius);
    }

    public override void Throw(Vector2 force)
    {
        base.Throw(force);
        _time = 0;
        _ticking = true;
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
                    collider.GetComponent<PlayerHealth>().RemoveHealth(explosionDamage);
                }
                Destroy(gameObject);
            }
        }
    }
}
