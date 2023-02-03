using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public bool Player2;
    CapsuleCollider2D mainCollider;
    Rigidbody2D rb;
    bool isGrounded;
    public float jumpStrength = 20000;
    public float maxSpeed = 10f;
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
        //atsakingas uz player 1 valdyma
        if (Player2 == false)
        {

            if (Input.GetKey(KeyCode.A) && Mathf.Abs(rb.velocity.x) < maxSpeed)
            {

                rb.AddForce(new Vector2(-700, 0) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D) && Mathf.Abs(rb.velocity.x) < maxSpeed)
            {

                rb.AddForce(new Vector2(700, 0) * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.W)&&isGrounded==true)
            {
                rb.AddForce(new Vector2(0, jumpStrength) * Time.deltaTime);
            }
        
        }
        //atsakingas uz player 2 valdyma
        if (Player2 == true)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && Mathf.Abs(rb.velocity.x) < maxSpeed)
            {

                rb.AddForce(new Vector2(-700, 0) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow) && Mathf.Abs(rb.velocity.x) < maxSpeed)
            {

                rb.AddForce(new Vector2(700, 0) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow) && isGrounded == true)
            {
                rb.AddForce(new Vector2(0, jumpStrength) * Time.deltaTime);
            }
        }
    }
}
