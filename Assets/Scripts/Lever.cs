using UnityEngine;

public class Lever : MonoBehaviour
{
    public Animator doorAnimator;
    private bool isActivated = false;

    private Animator leverAnimator;

    void Start()
    {
        leverAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated == false && other.CompareTag("Player"))
        {
            ActivateLever();
        }
    }

    void ActivateLever()
    {
        isActivated = true;
        leverAnimator.SetBool("isActive", isActivated);
        doorAnimator.SetTrigger("Open");
    }
}
