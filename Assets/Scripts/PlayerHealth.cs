using UnityEngine;
using System;
using StarterAssets; // Needed to talk to the Controller

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 10f;
    public float currentHealth;

    [Header("UI Settings")]
    public GameObject losePanel;
    // public float deathDelay = 1.5f; // No longer needed since we don't destroy

    public static PlayerHealth Instance;
    public event Action onHealthChangedCallback;

    [HideInInspector] public bool isDead = false;

    private LosePanelUI losePanelUI;
    private FirstPersonController _controller; // Reference to movement script

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

        // Find the controller so we can stop movement/play anim
        _controller = GetComponent<FirstPersonController>();

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

        // 1. Show UI
        if (losePanel)
        {
            losePanel.SetActive(true);
            if (losePanelUI != null)
                losePanelUI.SetCauseOfDeath(lastDamageCause);
            
            // Unlock cursor so player can click "Restart"
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 2. Trigger Animation & Stop Movement
        if (_controller != null)
        {
            _controller.Die();           // Triggers "Death" in Animator
            _controller.enabled = false; // Stops input/movement
        }

        // REMOVED: Destroy(gameObject, deathDelay);
    }

    void NotifyHealthChange()
    {
        onHealthChangedCallback?.Invoke();
    }
}