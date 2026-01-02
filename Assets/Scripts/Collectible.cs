using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void Start()
    {
        // 1. Tell LevelManager "I exist, add me to the total count."
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RegisterObjective();
        }
        else
        {
            Debug.LogWarning("[Collectible] LevelManager not found! Make sure it's in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 2. Tell LevelManager "I was collected."
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.CollectObjective();
            }

            // 3. Play Sound (Optional, since LevelManager handles coin sound too)
            // if (AudioManager.Instance != null) 
            //    AudioManager.Instance.PlaySFX(AudioManager.Instance.coinPickup);

            // 4. Destroy this object
            Destroy(gameObject);
        }
    }
}