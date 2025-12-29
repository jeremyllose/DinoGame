using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 10f;
    public float currentHealth;

    [Header("UI Settings")]
    public GameObject losePanel;
    public float deathDelay = 1.5f;

    public static PlayerHealth Instance;
    public event Action onHealthChangedCallback;

    [HideInInspector] public bool isDead = false;

    private LosePanelUI losePanelUI;

    // 🆕 Tracks the last cause of damage
    private string lastDamageCause = "unknown";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChange();

        if (losePanel)
        {
            losePanel.SetActive(false);
            losePanelUI = losePanel.GetComponent<LosePanelUI>();
        }
    }

    public void TakeDamage(float amount, string cause = "unknown")
    {
        if (isDead || amount <= 0) return;

        // 🆕 Save the cause for when the player dies
        lastDamageCause = cause;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        NotifyHealthChange();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"💀 Player died! Cause: {lastDamageCause}");

        if (losePanel)
        {
            losePanel.SetActive(true);
            if (losePanelUI != null)
                losePanelUI.SetCauseOfDeath(lastDamageCause);
        }

        Destroy(gameObject, deathDelay);
    }

    void NotifyHealthChange()
    {
        onHealthChangedCallback?.Invoke();
    }
}
