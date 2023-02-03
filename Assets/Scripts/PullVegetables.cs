using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullVegetables : MonoBehaviour
{
    [SerializeField] KeyCode DownButton;
    [SerializeField] float MaxDistanceToVegetable;
    [SerializeField] float PullingTime;
    bool _pulling = false;
    float _pullingTime;


    private void OnDrawGizmos()
    {
        Bounds colliderBounds = GetComponent<Collider2D>().bounds;
        Gizmos.DrawWireSphere(colliderBounds.min + new Vector3(colliderBounds.extents.x, 0, 0), MaxDistanceToVegetable);
    }

    private void Update()
    {
        if (Input.GetKeyDown(DownButton))
        {
            Bounds colliderBounds = GetComponent<Collider2D>().bounds;
            Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.extents.x, 0, 0);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, MaxDistanceToVegetable, 64);
            if(colliders.Length > 0)
            {
                // code when started pulling

                gameObject.GetComponent<CharacterController2D>().canMove = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.transform.position = new Vector3(colliders[0].gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                
                _pulling = true;
                _pullingTime = 0;
            }
        }
        else if(Input.GetKeyUp(DownButton) && _pulling)
        {
            // code when pulling got canceled
            gameObject.GetComponent<CharacterController2D>().canMove = true;
            _pulling = false;
        }
        if (_pulling)
        {
            _pullingTime += Time.deltaTime;
            if(_pullingTime >= PullingTime)
            {
                // code when pulling sucessfull
                gameObject.GetComponent<CharacterController2D>().canMove = true;
                _pulling = false;
            }
        }
    }
}
