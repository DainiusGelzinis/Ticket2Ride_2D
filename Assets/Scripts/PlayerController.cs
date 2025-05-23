using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private float wallJumpCooldown = 0.2f;
    private float wallJumpTimer;

    private float horizontalInput;
    private int facingDir = 1;

    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player sprite based on move 
        if (horizontalInput > 0.01f)
            facingDir = 1;
        else if (horizontalInput < -0.01f)
            facingDir = -1;

        transform.localScale = new Vector3(1 * facingDir, 1, 1);

        UpdateAnimator();
        
        if (wallJumpTimer > 0)
            wallJumpTimer -= Time.deltaTime;

        if (wallJumpTimer <= 0)
        {
            // Wall logic
            if (OnWall() && !IsGrounded())
            {
                rb.gravityScale = 0;
                rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocity.y, -2f, 2f)); // slow sliding on wall
            }
            else
            {
                rb.gravityScale = 2.5f;
            }

            // Jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (IsGrounded())
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
                else if (OnWall() && !IsGrounded())
                {
                    // Wall jump push off direction
                    float pushDirection = -Mathf.Sign(transform.localScale.x);

                    // Apply push off and upward force
                    Vector2 wallJumpVelocity = new Vector2(pushDirection * speed, jumpForce);

                    rb.linearVelocity = wallJumpVelocity;

                    // Flip player to face away from wall
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                    wallJumpTimer = wallJumpCooldown;
                    rb.gravityScale = 2.5f; // restore gravity immediately
                }
            }

            // Normal horizontal movement 
            if (wallJumpTimer <= 0)
            {
                rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * 0.9f, 0f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    private bool OnWall()
    {
        Vector2 direction = new Vector2(transform.localScale.x, 0);
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * 0.9f, 0f, direction, 0.1f, wallLayer);
        return hit.collider != null;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("InputY", rb.linearVelocity.y);

        bool isWalking = Mathf.Abs(horizontalInput) > 0.01f && IsGrounded();
        animator.SetBool("isWalking", isWalking);

        bool isJumping = !IsGrounded();
        //animator.SetBool("isJumping", isJumping);
    }
}
