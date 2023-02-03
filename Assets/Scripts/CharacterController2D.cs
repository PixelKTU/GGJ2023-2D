using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode jumpKey;

    CapsuleCollider2D mainCollider;
    Rigidbody2D rb;

    bool isGrounded;
    public float jumpStrength = 5;
    public float maxSpeed = 10f;
    private float jumpTimeCounter;
    public float jumpTime = 0.5f;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded patikrina
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        //atsakingas uz vaiksciojima
        if (Input.GetKey(leftKey) && Mathf.Abs(rb.velocity.x) < maxSpeed)
        {

            rb.AddForce(new Vector2(-700, 0) * Time.deltaTime);
        }
        if (Input.GetKey(rightKey) && Mathf.Abs(rb.velocity.x) < maxSpeed)
        {

            rb.AddForce(new Vector2(700, 0) * Time.deltaTime);
        }

        //atsakingas uz sokinejima
        if(Input.GetKeyDown(jumpKey)&&isGrounded==true)
        {
            rb.velocity = Vector2.up * jumpStrength;
            jumpTimeCounter = jumpTime;
            isJumping = true;

        }
        if (Input.GetKey(jumpKey) && isJumping == true)
        {
            if(jumpTimeCounter>0)
            {
                rb.velocity = Vector2.up * jumpStrength;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping= false;
            }
        }
        if(Input.GetKeyUp(jumpKey))
        {

            isJumping= false; 
        }
    }
}
