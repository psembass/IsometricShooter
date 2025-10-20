using UnityEngine;
using UnityEngine.UIElements;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField]
    private UIDocument document;
    [SerializeField]
    private HealthComponent healthComponent;
    private ProgressBar healthBar;
    private Label label;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement rootVisualElement = document.rootVisualElement;
        healthBar = rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = healthComponent.MaxHealth;
        label = healthBar.Q<Label>(className: "unity-progress-bar__title");
        healthComponent.OnChanged += UpdateHealth;
        UpdateHealth(healthComponent.CurrentHealth);
    }

    void UpdateHealth(float currentHealth)
    {
        healthBar.value = currentHealth;
        label.text = healthComponent.CurrentHealth.ToString();
    }
}
