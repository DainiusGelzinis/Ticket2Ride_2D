using UnityEngine;
using System.Collections;

public class SpriteCycler : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentIndex];
            StartCoroutine(CycleSprites());
        }
    }

    IEnumerator CycleSprites()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait for 5 seconds

            currentIndex = (currentIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentIndex];
            spriteRenderer.enabled = false;
            spriteRenderer.enabled = true;
        }
    }
}