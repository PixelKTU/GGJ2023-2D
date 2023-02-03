using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public bool Player2;
    CapsuleCollider2D mainCollider;
    Rigidbody2D rb;
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
        
        }
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
        }
    }
}
