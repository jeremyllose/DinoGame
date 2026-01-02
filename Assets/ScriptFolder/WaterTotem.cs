using UnityEngine;

public class WaterTotem : MonoBehaviour
{
    [Header("Inspector Assignments")]
    public GameObject waterObject;  // Drag the hidden water (Blue plane) here
    public Renderer totemRenderer;  // Optional: Drag the Totem Mesh here to change its color

    private bool _isActivated = false;

    private void Start()
    {
        // 1. Force the water to be hidden when the game starts
        if (waterObject != null)
        {
            waterObject.SetActive(false);
        }

        // 2. Tell LevelManager we exist
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RegisterObjective();
        }
    }

    // TRIGGER: This fires when the Player walks into the invisible collider
    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated) return;

        // Check if the object colliding is the Player
        if (other.CompareTag("Player"))
        {
            ActivateTotem();
        }
    }

    public void ActivateTotem()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.totemSplash);
        _isActivated = true;
        Debug.Log("Water Totem Activated!");

        // A. UNHIDE WATER (The Indicator)
        if (waterObject != null)
        {
            waterObject.SetActive(true);
        }

        // B. VISUAL TINT 
        if (totemRenderer != null)
        {
            totemRenderer.material.color = Color.cyan;
        }

        // C. REPLENISH THIRST
        if (SurvivalStats.Instance != null)
        {
            SurvivalStats.Instance.RestoreThirst(100f);
        }

        // D. LEVEL PROGRESS
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.CollectObjective();

            // --- NEW: SAVE CHECKPOINT ---
            // Save the Totem's position as the new safe spot
            LevelManager.Instance.SetCheckpoint(transform.position);
        }
    }
}