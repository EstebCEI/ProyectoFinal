using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;

    [Header("UI")]
    public GameObject PantallaMuerte;

    [Header("Regeneración")]
    public float regenDelay = 8f;
    public float regenRate = 10f;

    private float lastDamageTime;

    [Header("Referencias")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;

    private bool isDead = false;

    void Start()
    {
        health = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }

        lastDamageTime = Time.time;

        if (animator != null)
            animator.SetBool("isDead", false);
    }

    void Update()
    {
        if (isDead) return;

        UpdateUI();
        HandleRegen();
        DebugDamage();
    }

    void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = health;
    }

    void HandleRegen()
    {
        if (health <= 0 || isDead) return;

        if (Time.time >= lastDamageTime + regenDelay)
        {
            health += regenRate * Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        lastDamageTime = Time.time;

        if (health <= 0f)
        {
            health = 0f;
            Die();
            return;
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        int stance = playerMovement.GetStance();

        if (animator != null)
        {
            animator.SetBool("isDead", true);
            animator.applyRootMotion = true;

            switch (stance)
            {
                case 0:
                    animator.CrossFade("DeathStanding", 0.05f);
                    break;

                case 1:
                    animator.CrossFade("DeathCrouch", 0.05f);
                    break;

                case 2:
                    animator.CrossFade("DeathProne", 0.05f);
                    break;
            }
        }

        if (playerMovement != null)
            playerMovement.SetDead(true);

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSecondsRealtime(3.5f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (PantallaMuerte != null)
            PantallaMuerte.SetActive(true);

        Time.timeScale = 0f;
    }

    void DebugDamage()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.kKey.wasPressedThisFrame)
        {
            TakeDamage(20f);
        }
    }
}