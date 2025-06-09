using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SpriteCycler : MonoBehaviour
{
    public Sprite[] sprites;
    public Slider slider;
    private SpriteRenderer spriteRenderer;
    public int currentIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentIndex];
        }
    }

    void Update()
    {

        currentIndex = (int)Mathf.Floor(3 / 2f + 3 * slider.value / 10f);
        spriteRenderer.sprite = sprites[currentIndex];
        spriteRenderer.enabled = false;
        spriteRenderer.enabled = true;

    }
}