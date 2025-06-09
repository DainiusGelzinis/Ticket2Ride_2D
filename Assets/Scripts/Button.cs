using UnityEngine;

public class Button : MonoBehaviour
{
    public Animator doorAnimator;
    private bool isPressed = false;
    private int objOnButton = 0;

    private Animator buttonAnimator;

    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            objOnButton++;
            if (isPressed == false)
            {
                pressButton();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            objOnButton--;
            if (objOnButton <= 0)
            {
                objOnButton = 0;
                releaseButton();
            }
        }
    }

    void pressButton()
    {
        isPressed = true;
        buttonAnimator.SetBool("isPressed", isPressed);
        doorAnimator.SetTrigger("Open");
    }

    void releaseButton()
    {
        isPressed = false;
        buttonAnimator.SetBool("isPressed", isPressed);
        doorAnimator.SetTrigger("Close");
    }
}
