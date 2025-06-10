using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth = 10f;
    public float currentHealth { get; private set; }

    private void Awake()
    {
        // Initialize currentHealth to the starting value
        currentHealth = startingHealth;
    }

    public void TakeDamage(float damage)
    {
        // Subtract damage and clamp at zero
        currentHealth = Mathf.Max(currentHealth - damage, 0f);

        // Example check: if health has reached zero, do something
        if (currentHealth <= 0f)
        {
            // Player has died or health is depleted
            // (You said “don’t pay attention to Die method yet,” so leave this empty for now.)
        }
        else
        {
            // Player is still alive—maybe play a hurt animation, etc.
        }
    }

    public void RestoreFullHealth()
    {
        currentHealth = startingHealth;
    }

}
