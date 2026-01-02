using UnityEngine;

public class FlyingCollectible : MonoBehaviour
{
    private void Start()
    {
        // 1. Register with Level Manager
        // This adds +1 to the "Total Needed" count for the current level (Level 3)
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RegisterObjective();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 2. Check if the Player flew through the ring
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ring Collected!");

            if (LevelManager.Instance != null)
            {
                // 3. Count it!
                LevelManager.Instance.CollectObjective();
            }

            // 4. Destroy the Ring so you can't collect it twice
            Destroy(gameObject);
        }
    }
}