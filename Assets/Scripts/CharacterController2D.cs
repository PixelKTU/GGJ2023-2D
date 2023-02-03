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
    public float horizontalDrag;
    public float jumpStrength = 5;
    public float moveSpeed;
    private float jumpTimeCounter;
    public float jumpTime = 0.5f;
    private bool isJumping;
    private float startingSizeX;

    [HideInInspector] public bool canMove = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        startingSizeX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded patikrina
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius, 128);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = (colliders.Length > 0);
        
        //atsakingas uz vaiksciojima
        if (canMove)
        {

            float playerVelocity = 0;
            if (Input.GetKey(leftKey))
            {
                transform.localScale = new Vector3(-startingSizeX, transform.localScale.y, transform.localScale.z); // flip sprite to the left
                playerVelocity -= moveSpeed;
            }
            if (Input.GetKey(rightKey))
            {
                transform.localScale = new Vector3(startingSizeX, transform.localScale.y, transform.localScale.z); // flip sprite to the right
                playerVelocity += moveSpeed;
            }

            if (playerVelocity == 0)
            {
                if (rb.velocity.x > 0)
                {
                    rb.velocity = new Vector2(Mathf.Max(0, rb.velocity.x - horizontalDrag * Time.deltaTime), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Min(0, rb.velocity.x + horizontalDrag * Time.deltaTime), rb.velocity.y);
                }
            }
            else {
                rb.velocity = new Vector2(playerVelocity, rb.velocity.y);
            }

            //atsakingas uz sokinejima
            if (Input.GetKeyDown(jumpKey) && isGrounded == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                jumpTimeCounter = jumpTime;
                isJumping = true;

            }
            if (Input.GetKey(jumpKey) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            if (Input.GetKeyUp(jumpKey))
            {

                isJumping = false;
            }
        }
    }
}
