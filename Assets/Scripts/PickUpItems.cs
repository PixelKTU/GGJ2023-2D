using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    public Animator animator;
    public bool currentHolding = false;
    public bool previousHolding = false;

    //[SerializeField] KeyCode DownButton;
    [SerializeField] private float MaxDistanceToVegetable;
    [SerializeField] private float PullingTime;
    [SerializeField] private Vector2 ThrowForce;
    [SerializeField] private GameObject PickedObjectSpot;

    [SerializeField] private AudioClip picking;
    [SerializeField] private AudioClip throwSound;

    private PlayerHealth health;
    private BoxCollider2D PickedObjectSpotCollider;
    private bool _pulling = false;
    private float _pullingTime;
    private GameObject _pickedObject = null;
    private GameObject _pickingGrass = null;
    private bool soundCanBePlayed = true;
    private CharacterController2D controller2D;
    private bool lastFrameGrabbing = false;

    private void OnDrawGizmos()
    {
        Bounds colliderBounds = GetComponent<Collider2D>().bounds;
        Gizmos.DrawWireSphere(colliderBounds.min + new Vector3(colliderBounds.extents.x, 0, 0), MaxDistanceToVegetable);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
        PickedObjectSpotCollider = PickedObjectSpot.GetComponent<BoxCollider2D>();
        controller2D = gameObject.GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        if (!health.dead && (controller2D.grabbing && !lastFrameGrabbing))
        {
            if (_pickedObject == null)
            {
                Bounds colliderBounds = GetComponent<Collider2D>().bounds;
                Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.extents.x, 0, 0);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, MaxDistanceToVegetable, 64);
                if (colliders.Length > 0)
                {
                    // code when started pulling

                    controller2D.canMove = false;
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    gameObject.transform.position = new Vector3(colliders[0].gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                    _pickingGrass = colliders[0].gameObject;
                    _pickingGrass.GetComponent<PickableGrass>().StartPicking();
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
                _pickedObject.GetComponent<Rigidbody2D>().simulated = true;

                PickedObjectSpotCollider.enabled = false;

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

        lastFrameGrabbing = controller2D.grabbing;

        if (_pulling)
        {
            _pullingTime += Time.deltaTime;
            if (_pullingTime >= PullingTime)
            {
                // code when pulling sucessfull
                GameObject prefab = _pickingGrass.GetComponent<PickableGrass>().containedObject;
                _pickedObject = Instantiate(prefab, PickedObjectSpot.transform);
                _pickedObject.layer = 9;
                _pickedObject.GetComponent<Rigidbody2D>().simulated = false;

                PickedObjectSpotCollider.enabled = true;
                PickedObjectSpotCollider.size = _pickedObject.GetComponent<BoxCollider2D>().size;

                _pickedObject.transform.localPosition = Vector3.zero;
                _pickedObject.GetComponent<ThrowableObject>().Created(gameObject, prefab);
                controller2D.canMove = true;
                _pulling = false;
                //Destroy(_pickingGrass);
                _pickingGrass.GetComponent<PickableGrass>().HideLeafs();
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

        if (_pickedObject == null && PickedObjectSpotCollider.enabled)
        {
            PickedObjectSpotCollider.enabled = false;
        }

        if (_pickedObject != null && !PickedObjectSpotCollider.enabled)
        {
            PickedObjectSpotCollider.enabled = true;
            PickedObjectSpotCollider.size = _pickedObject.GetComponent<BoxCollider2D>().size;
        }

        if (_pickedObject != null && soundCanBePlayed == true)
        {
            SoundManager.Instance.PlaySoundOneShot(picking);
            soundCanBePlayed = false;
        }

        if (_pickedObject == null)
        {
            soundCanBePlayed = true;
        }

        previousHolding = currentHolding;

        if (_pickedObject == null)
        {
            currentHolding = false;
        }
        else if (_pickedObject != null)
        {
            currentHolding = true;
        }

        if (currentHolding == false && previousHolding == true)
        {
            SoundManager.Instance.PlaySoundOneShot(throwSound);
        }

    }

    public void PickUpExistingItem(GameObject item)
    {
        if (!health.dead && !_pulling && _pickedObject == null)
        {
            _pickedObject = item;
            item.layer = 9;
            item.GetComponent<ThrowableObject>().WhenPickedUp();
            _pickedObject.GetComponent<Rigidbody2D>().simulated = false;

            PickedObjectSpotCollider.enabled = true;
            PickedObjectSpotCollider.size = _pickedObject.GetComponent<BoxCollider2D>().size;

            _pickedObject.transform.parent = PickedObjectSpot.transform;
            _pickedObject.transform.localPosition = Vector3.zero;
            ThrowableObject throwobj = _pickedObject.GetComponent<ThrowableObject>();
            throwobj.Created(gameObject, throwobj.prefab);
        }
    }

    public void ItemRemovedFromHands()
    {

        _pickedObject.transform.parent = null;
        _pickedObject = null;

        PickedObjectSpotCollider.enabled = false;
    }

    public void DeleteItemFromHands()
    {
        if (_pickedObject != null)
        {
            Destroy(_pickedObject);
            _pickedObject = null;
            PickedObjectSpotCollider.enabled = false;
        }
    }

}
