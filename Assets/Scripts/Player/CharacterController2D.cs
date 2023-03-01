using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    // [SerializeField] KeyCode leftKey;
    // [SerializeField] KeyCode rightKey;
    //  [SerializeField] KeyCode jumpKey;

    private string horizontalAxis;
    private string verticalAxis;

    BoxCollider2D mainCollider;
    Rigidbody2D rb;
    public Animator animator;

    bool isGrounded;
    public float horizontalDrag;
    public float jumpStrength = 8;
    public float moveSpeed;
    //private float jumpTimeCounter;
    private float startingSizeX;
    public float gravityScale = 5;
    public AudioClip jump;

    [SerializeField] float jumpCooldownTimeInSeconds = 0.1f;

    [Header("Movement assist settings")]
    [SerializeField] float CoyoteTimeInSeconds = 0.1f;
    [SerializeField] float JumpBufferInSeconds = 0.1f;

    [Header("Better jump")]
    [SerializeField] bool enableBetterJump = true;

    [SerializeField] float additionalLowJumpGravMultiplier = 0;
    [SerializeField] float additionalFallGravMultiplier = 0;
    [SerializeField] float maxVerticalSpeed;


    private float stunned = 0;
    private float flipped = 0; 
    [HideInInspector] public bool canMove = true;

    private float jumpBufferTime = 0;
    private float coyoteTime = 0;
    private float jumpCooldown = 0;

    private bool isJumping = false;

    private PlayerSkinData playerSkinData;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        startingSizeX = transform.localScale.x;
        rb.gravityScale = gravityScale;
    }

    public void Stun(float duration)
    {
        stunned = duration;
    }

    public void FlipControls(float duration)
    {
        flipped = duration;
    }

    public PlayerSkinData GetPlayerSkinData()
    {
        return playerSkinData;
    }

    public void OnCreatePlayer(string horizontalAxis, string verticalAxis, PlayerSkinData playerSkinData)
    {
        this.horizontalAxis = horizontalAxis;
        this.verticalAxis = verticalAxis;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = playerSkinData.animations;
        this.playerSkinData = playerSkinData;
    }

    private void OnDrawGizmosSelected()
    {
        mainCollider = GetComponent<BoxCollider2D>();

        Bounds colliderBounds = mainCollider.bounds;
        Vector3 colliderSize = new Vector3(mainCollider.size.x * 0.9f * Mathf.Abs(transform.localScale.x), 0.1f, 1);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderSize.y * 0.45f, 0);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos, colliderSize);

        Gizmos.color = Color.blue;
        float grav = Physics2D.gravity.y;

        float t = -jumpStrength / grav;
        float yPos = transform.position.y + jumpStrength * t + (grav * t * t) / 2;

        Gizmos.DrawRay(new Vector2(transform.position.x, yPos), Vector2.right);
    }

    [HideInInspector]
    public bool grabbing = false;

    float lastVerticalVal = 0;
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameEnded) return;

        // coyote time and jump buffer time update
        if (coyoteTime > 0) coyoteTime -= Time.deltaTime;
        if (jumpBufferTime > 0) jumpBufferTime -= Time.deltaTime;

        // jump cooldown time update 
        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;

        //isGrounded patikrina
        Bounds colliderBounds = mainCollider.bounds;
        Vector3 colliderSize = new Vector3(mainCollider.size.x * 0.9f * Mathf.Abs(transform.localScale.x), 0.1f, 1);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderSize.y * 0.45f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheckPos, colliderSize, 0, 1152);

        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0 && rb.velocity.y < 0.05f)
        {
            coyoteTime = CoyoteTimeInSeconds;
            isGrounded = true;


        }

        if (stunned > 0)
        {
            stunned = Mathf.Max(0, stunned - Time.deltaTime);
        }

        float verticalVal = Input.GetAxisRaw(verticalAxis);
        float horizontalVal = Input.GetAxisRaw(horizontalAxis);

        if (flipped > 0)
        {
            flipped = Mathf.Max(0, flipped - Time.deltaTime);
            horizontalVal *= -1;
        }

        grabbing = verticalVal < 0;

        //atsakingas uz vaiksciojima
        if (canMove && stunned == 0)
        {

            float playerVelocity = 0;

            if (horizontalVal < 0)
            {
                transform.localScale = new Vector3(-startingSizeX, transform.localScale.y, transform.localScale.z); // flip sprite to the left
                playerVelocity -= moveSpeed;
            }
            if (horizontalVal > 0)
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
            else
            {
                rb.velocity = new Vector2(playerVelocity, rb.velocity.y);
            }

            if (jumpCooldown <= 0)
            {
                if (verticalVal > 0 && lastVerticalVal == 0)
                {
                    jumpBufferTime = JumpBufferInSeconds;
                }

                //atsakingas uz sokinejima
                if (((verticalVal > 0 && lastVerticalVal == 0) || jumpBufferTime > 0) && (isGrounded == true || coyoteTime > 0))
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                    //jumpTimeCounter = jumpTime;

                    SoundManager.Instance.PlaySoundOneShot(jump);
                    isJumping = true;

                    jumpCooldown = jumpCooldownTimeInSeconds;
                    jumpBufferTime = 0;
                }
            }
            if (verticalVal <= 0)
            {
                //Debug.Log("pavyko");
                isJumping = false;
            }
        }

        if (enableBetterJump)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (additionalFallGravMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !isJumping)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (additionalLowJumpGravMultiplier - 1) * Time.deltaTime;
            }
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * Mathf.Min(Mathf.Abs(rb.velocity.y), maxVerticalSpeed));
        }
        lastVerticalVal = verticalVal;

        //atsakingas uz animacija
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            animator.SetBool("isGrounded", isGrounded);
        }

    }
}
