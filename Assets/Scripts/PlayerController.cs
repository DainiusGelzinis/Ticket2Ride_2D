using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float inertiaForce = 0f;

    [SerializeField] private Health health;
    [SerializeField] private GameObject endGameMenuUI;

    [SerializeField] private AudioClip damageSoundClip;

    public bool isDead = false;
    public SpriteCycler laserCycle;

    private bool isInvincible = false;

    public static bool GameIsDead { get; set; } = false;

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
        if (isDead)
            return;

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
                rb.gravityScale = 1f;
                rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocity.y, -2f, 2f)); // slow sliding on wall
            }
            else
            {
                rb.gravityScale = 4f;
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
                // Apply inertia
                float effectiveXVelocity = (horizontalInput * speed) - inertiaForce;
                rb.linearVelocity = new Vector2(effectiveXVelocity, rb.linearVelocity.y);
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

        bool Damage = isInvincible;
        animator.SetBool("Damage", Damage);
    }

    public void SetInertiaForce(float value)
    {
        inertiaForce = value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") && !isDead && !isInvincible)
        {

            health.TakeDamage(1f);
            SoundFXManager.instance.PlaySoundFXCLip(damageSoundClip, transform, 1f);


            if (health.currentHealth <= 0f)
            {
                Die();
            }
            else
            {

                StartCoroutine(InvincibilityCoroutine());
            }
        } else if  (collision.gameObject.CompareTag("Floor") && !isDead)
        {
            Die();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser") && !isDead && !isInvincible && laserCycle.currentIndex == 2)
        {

            health.TakeDamage(1f);
            SoundFXManager.instance.PlaySoundFXCLip(damageSoundClip, transform, 1f);


            if (health.currentHealth <= 0f)
            {
                Die();
            }
            else
            {

                StartCoroutine(InvincibilityCoroutine());
            }
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;


        yield return new WaitForSeconds(1f);

        isInvincible = false;


    }


    private void Die()
    {
        if (isDead) return;
        isDead = true;
        GameIsDead = true;

        // Freeze the Rigidbody so the player can’t keep moving:
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        Time.timeScale = 0f;

        // SHOW the EndGameMenu:
        if (endGameMenuUI != null)
            endGameMenuUI.SetActive(true);

    }



    private void ReloadScene()
    {
        Time.timeScale = 1f;  // in case time was paused elsewhere
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


     public void RespawnAtCheckpoint()
    {
        // 1) teleport
        Vector3 cp = CheckpointManager.Instance.GetCheckpointPosition();
        CheckpointManager.Instance.ResetSceneObjects();
        transform.position = cp;

        // 2) reset physics
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;

        // 3) heal
        health.RestoreFullHealth();

        // 4) allow movement again
        isDead = false;
        GameIsDead = false;
    }

    public float GetInertiaForce()
    {
        return inertiaForce;
    }
    
    
}