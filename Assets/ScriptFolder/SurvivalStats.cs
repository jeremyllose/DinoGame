using UnityEngine;
using UnityEngine.UI;
using TMPro; // Needed for TextMeshPro

public class SurvivalStats : MonoBehaviour
{
    public static SurvivalStats Instance { get; private set; }

    [Header("Thirst Settings")]
    public float maxThirst = 100f;
    public float currentThirst;
    public float drainRate = 2.0f;

    [Header("Status")]
    public float heatMultiplier = 1.0f;
    public bool isDead = false;

    [Header("UI Visuals")]
    public GameObject thirstUIContainer; // The "Group" parent
    public Image thirstBarImage;         // The Blue Bar
    public TextMeshProUGUI thirstText;   // NEW: The text that says "100%"
    public GameObject warningUI;         // The "CRITICAL" text

    [Header("Colors")]
    public Color safeColor = new Color(0f, 0.6f, 1f);
    public Color dangerColor = Color.red;
    public float dangerThreshold = 30f;

    private StarterAssets.FirstPersonController _controller;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        currentThirst = maxThirst;
        isDead = false;
        if (thirstUIContainer != null) thirstUIContainer.SetActive(true);
        if (warningUI != null) warningUI.SetActive(false);
    }

    private void OnDisable()
    {
        if (thirstUIContainer != null) thirstUIContainer.SetActive(false);
        if (warningUI != null) warningUI.SetActive(false);
    }

    private void Start()
    {
        // Finding the controller on the Parent (PlayerCapsule)
        _controller = GetComponentInParent<StarterAssets.FirstPersonController>();
    }
    private void Update()
    {
        if (isDead) return;

        // Drain Thirst using the Heat Multiplier
        if (currentThirst > 0)
        {
            // FORMULA: Base Drain x Heat Zone Multiplier
            currentThirst -= drainRate * heatMultiplier * Time.deltaTime;
        }
        else
        {
            Die("Dehydration");
        }

        // 2. Update UI Visuals
        if (thirstUIContainer != null)
        {
            // A. Shrink the Bar
            if (thirstBarImage != null)
                thirstBarImage.fillAmount = currentThirst / maxThirst;

            // B. Update the Number Text (NEW)
            // "F0" means no decimal points (shows 99 instead of 99.123)
            if (thirstText != null)
                thirstText.text = $"{currentThirst.ToString("F0")}%";

            // C. Color & Warning Logic
            if (currentThirst <= dangerThreshold)
            {
                // Flash Red
                float flash = Mathf.PingPong(Time.time * 5f, 1f);
                if (thirstBarImage != null)
                    thirstBarImage.color = Color.Lerp(dangerColor, Color.black, flash * 0.5f);

                // Turn text red too
                if (thirstText != null) thirstText.color = Color.red;

                if (warningUI != null) warningUI.SetActive(true);
            }
            else
            {
                // Normal Blue
                if (thirstBarImage != null) thirstBarImage.color = safeColor;
                if (thirstText != null) thirstText.color = Color.white;

                if (warningUI != null) warningUI.SetActive(false);
            }
        }
    }

    public void RestoreThirst(float amount)
    {
        currentThirst += amount;
        if (currentThirst > maxThirst) currentThirst = maxThirst;
    }

    public void Die(string cause)
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"DIED FROM: {cause}");

        // --- THE FIX ---
        // Instead of trying to handle death here, we tell PlayerHealth
        // "We just took 10,000 damage from Dehydration."
        // This forces PlayerHealth to run its own Die() function, 
        // which opens the Lose Panel for us.

        if (PlayerHealth.Instance != null)
        {
            // Deal fatal damage so PlayerHealth handles the UI
            PlayerHealth.Instance.TakeDamage(9999f, cause);
        }
        else
        {
            // Fallback if PlayerHealth is missing (Safety check)
            if (_controller != null)
            {
                _controller.Die();
                _controller.enabled = false;
            }
        }
    }
}