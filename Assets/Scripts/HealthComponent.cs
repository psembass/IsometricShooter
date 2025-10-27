using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [field: SerializeField]
    public float MaxHealth { get; private set; } = 100f;
    public float CurrentHealth { get; private set; }

    public event Action OnDeath;
    public event Action<float> OnChanged;

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;
        }
        OnChanged?.Invoke(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
        }
    }

    public void Restore()
    {
        CurrentHealth = MaxHealth;
        OnChanged?.Invoke(CurrentHealth);
    }
}
