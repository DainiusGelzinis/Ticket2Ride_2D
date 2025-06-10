using UnityEngine;

public class BoxController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private AutoSlider slide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2 (-slide.iner, rb.linearVelocity.y);
    }
}
