using UnityEngine;

public class WaterTotem : MonoBehaviour
{
    [Header("Inspector Assignments")]
    public GameObject waterObject;  
    public Renderer totemRenderer;  
    
    // We don't need myLevelID or Start() anymore because LevelManager handles the count manually.
    
    private bool _isActivated = false;

    private void Start()
    {
        // Just hide the water visual
        if (waterObject != null) waterObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated) return;

        if (other.CompareTag("Player"))
        {
            // Only activate if we are on Level 2
            if (LevelManager.Instance != null && LevelManager.Instance.currentLevelIndex == 2)
            {
                ActivateTotem();
            }
        }
    }

    public void ActivateTotem()
    {
        _isActivated = true;
        Debug.Log("Water Totem Activated!");

     
        if (waterObject != null) waterObject.SetActive(true);
        if (totemRenderer != null) totemRenderer.material.color = Color.cyan;
        if (SurvivalStats.Instance != null) SurvivalStats.Instance.RestoreThirst(100f);

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.CollectObjective(); // Counts 1/3, 2/3...
            LevelManager.Instance.SetCheckpoint(transform.position);
        }
    }
}