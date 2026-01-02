using UnityEngine;

public class HeatZone : MonoBehaviour
{
    public float heatIntensity = 3.0f; // Drains thirst 3x faster

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SurvivalStats.Instance != null)
        {
            SurvivalStats.Instance.heatMultiplier = heatIntensity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && SurvivalStats.Instance != null)
        {
            SurvivalStats.Instance.heatMultiplier = 1.0f; // Reset to normal
        }
    }
}