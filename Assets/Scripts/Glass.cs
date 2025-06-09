using UnityEngine;

public class Glass : MonoBehaviour
{
    public float disappearDelay = 1.0f;
    public float reappearDelay = 3.0f;   

    private Collider2D platformCollider;  
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    //private GameObject glass;

    private bool WalkedOn = false;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (WalkedOn == false && collision.collider.CompareTag("Player"))
        {
            animator.SetBool("Disappear", true);
            Invoke(nameof(Disappear), disappearDelay);
        }
    }

    void Disappear()
    {
        WalkedOn = true;
        animator.SetBool("Disappear", false);
        platformCollider.enabled = false;
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        Invoke(nameof(Reappear), reappearDelay);
    }

    void Reappear()
    {
        WalkedOn = false;
        
        platformCollider.enabled = true;
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }
}
