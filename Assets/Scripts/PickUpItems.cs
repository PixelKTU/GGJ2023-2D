using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] KeyCode DownButton;
    [SerializeField] float MaxDistanceToVegetable;
    [SerializeField] float PullingTime;
    [SerializeField] Vector2 ThrowForce;
    [SerializeField] GameObject PickedObjectSpot;
    bool _pulling = false;
    float _pullingTime;
    GameObject _pickedObject = null;
    GameObject _pickingPrefab = null;
    GameObject _pickingGrass = null;

    public Animator animator;


    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

    private void OnDrawGizmos()
    {
        Bounds colliderBounds = GetComponent<Collider2D>().bounds;
        Gizmos.DrawWireSphere(colliderBounds.min + new Vector3(colliderBounds.extents.x, 0, 0), MaxDistanceToVegetable);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(DownButton))
        {
            if (_pickedObject == null)
            {
                Bounds colliderBounds = GetComponent<Collider2D>().bounds;
                Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.extents.x, 0, 0);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, MaxDistanceToVegetable, 64);
                if (colliders.Length > 0)
                {
                    // code when started pulling

                    gameObject.GetComponent<CharacterController2D>().canMove = false;
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    gameObject.transform.position = new Vector3(colliders[0].gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                    _pickingGrass = colliders[0].gameObject;
                    _pickingPrefab = colliders[0].GetComponent<PickableGrass>().containedObject;
                    _pulling = true;
                    _pullingTime = 0;
                    if (animator != null)
                    {
                        animator.SetBool("isPulling",true);
                    }
                }
            }
            else
            {

                CopyComponent<Rigidbody2D>(_pickingPrefab.GetComponent<Rigidbody2D>(), _pickedObject);

                if (gameObject.transform.localScale.x > 0)
                {
                    _pickedObject.GetComponent<ThrowableObject>().Throw(ThrowForce);
                }
                else
                {
                    _pickedObject.GetComponent<ThrowableObject>().Throw(new Vector2(-ThrowForce.x, ThrowForce.y));
                }
                _pickedObject = null;
                _pickingPrefab = null;
            }
        }
        if (_pulling)
        {
            _pullingTime += Time.deltaTime;
            if(_pullingTime >= PullingTime)
            {
                // code when pulling sucessfull
                _pickedObject = Instantiate(_pickingPrefab, PickedObjectSpot.transform);
                Destroy(_pickedObject.GetComponent<Rigidbody2D>());
                _pickedObject.transform.localPosition = Vector3.zero;
                _pickedObject.GetComponent<ThrowableObject>().Created(gameObject);
                gameObject.GetComponent<CharacterController2D>().canMove = true;
                _pulling = false;
                Destroy(_pickingGrass);
                if (animator != null)
                {
                    animator.SetBool("isPulling", false);
                }
            }
        }
        if (animator != null)
        {
            if (_pickedObject != null)
            {
                animator.SetBool("holdingItem", true);
            }
            else
            {
                animator.SetBool("holdingItem", false);
            }
        }
    }
}
