using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;

    [Header("Regeneración")]
    public float regenDelay = 8f;      
    public float regenRate = 10f;       

    private float lastDamageTime;

    void Start()
    {
        health = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }

        lastDamageTime = Time.time;
    }

    void Update()
    {
        UpdateUI();
        HandleRegen();
        DebugDamage();
    }

    // UI
    void UpdateUI()
    {
        if (healthSlider != null && healthSlider.value != health)
        {
            healthSlider.value = health;
        }
    }

    // REGENERACIÓN
    void HandleRegen()
    {
        if (health <= 0) return;

        // Si han pasado X segundos sin daño
        if (Time.time >= lastDamageTime + regenDelay)
        {
            health += regenRate * Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth);
        }
    }

    // DAÑO
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Reinicia temporizador de regeneración
        lastDamageTime = Time.time;

        if (health == 0)
            Die();
    }

    // MUERTE
    void Die()
    {
        Debug.Log("Player has died.");
    }

    // TEST (tecla K)
    void DebugDamage()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.kKey.wasPressedThisFrame)
        {
            TakeDamage(20f);
        }
    }
}