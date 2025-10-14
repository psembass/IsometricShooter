using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onDeath();
        }
    }

    private void onDeath()
    {
        // todo death actions, animation, return to pool
        gameObject.SetActive(false);
    }
}
