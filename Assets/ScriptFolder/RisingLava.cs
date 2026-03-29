using UnityEngine;

public class RisingLava : MonoBehaviour
{
    [Header("Settings")]
    public float riseSpeed = 0.5f; 
    public float damage = 9999f;   
    public int activeLevelID = 4; // Only move on Level 4

    void Update()
    {
        // --- THE FIX ---
        // If LevelManager exists, and we are NOT on Level 4, stop here.
        if (LevelManager.Instance != null)
        {
            if (LevelManager.Instance.currentLevelIndex != activeLevelID)
            {
                return; // Do nothing. Stay at the bottom.
            }
        }

        // Move the lava UP
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Double check level before killing
        if (LevelManager.Instance != null && LevelManager.Instance.currentLevelIndex != activeLevelID) return;

        if (other.CompareTag("Player"))
        {
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(damage, "Burned by Lava");
            }
        }
    }
}