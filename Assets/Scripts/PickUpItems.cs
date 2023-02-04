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
    GameObject _pickingGrass = null;
    public AudioSource picking;
    bool soundCanBePlayed = true;

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
                    _pulling = true;
                    _pullingTime = 0;
                    if (animator != null)
                    {
                        animator.SetBool("isPulling", true);
                    }
                }
            }
            else
            {
                ThrowableObject throwobj = _pickedObject.GetComponent<ThrowableObject>();
                CopyComponent<Rigidbody2D>(throwobj.prefab.GetComponent<Rigidbody2D>(), _pickedObject);

                if (gameObject.transform.localScale.x > 0)
                {
                    throwobj.Throw(ThrowForce);
                }
                else
                {
                    throwobj.Throw(new Vector2(-ThrowForce.x, ThrowForce.y));
                }
                _pickedObject = null;
            }
        }
        if (_pulling)
        {
            _pullingTime += Time.deltaTime;
            if (_pullingTime >= PullingTime)
            {
                // code when pulling sucessfull
                GameObject prefab = _pickingGrass.GetComponent<PickableGrass>().containedObject;
                _pickedObject = Instantiate(prefab, PickedObjectSpot.transform);
                _pickedObject.layer = 9;
                Destroy(_pickedObject.GetComponent<Rigidbody2D>());
                _pickedObject.transform.localPosition = Vector3.zero;
                _pickedObject.GetComponent<ThrowableObject>().Created(gameObject,prefab);
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
        
        if(_pickedObject!=null&&soundCanBePlayed==true)
        {
            picking.Play();
            soundCanBePlayed = false;
        }
        if(_pickedObject==null)
        {
            soundCanBePlayed = true;
        }


    }

    public void PickUpExistingItem(GameObject item)
    {
        if (!_pulling && _pickedObject == null)
        {
            _pickedObject = item;
            item.layer = 9;
            item.GetComponent<ThrowableObject>().WhenPickedUp();
            Destroy(_pickedObject.GetComponent<Rigidbody2D>());
            _pickedObject.transform.parent = PickedObjectSpot.transform;
            _pickedObject.transform.localPosition = Vector3.zero;
            ThrowableObject throwobj = _pickedObject.GetComponent<ThrowableObject>();
            throwobj.Created(gameObject,throwobj.prefab);
        }
    }

    public void ItemRemovedFromHands()
    {
        _pickedObject = null;
    }

}
