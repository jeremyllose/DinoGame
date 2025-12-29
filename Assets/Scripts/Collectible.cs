using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static int collectiblesCollected = 0;
    public static int totalCollectibles = 0;

    private void Start()
    {
        totalCollectibles++;
        Debug.Log($"[Collectible] Spawned: {gameObject.name}. Total collectibles now = {totalCollectibles}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Collectible] Trigger entered by {other.name}");

        if (other.CompareTag("Player"))
        {
            collectiblesCollected++;
            Debug.Log($"[Collectible] Collected by Player! {collectiblesCollected}/{totalCollectibles}");

            // Update UI
            if (UIManager.Instance != null)
                UIManager.Instance.UpdateCollectibleUI(collectiblesCollected, totalCollectibles);
            else
                Debug.LogWarning("[Collectible] UIManager not found!");

            // Destroy collectible object
            Destroy(gameObject);

            // Check win condition (Level 1 only)
            if (collectiblesCollected >= totalCollectibles)
            {
                Debug.Log("[Collectible] All collectibles obtained! Notifying LevelManager...");
                if (LevelManager.Instance != null)
                    LevelManager.Instance.CompleteLevel();
                else
                    Debug.LogWarning("[Collectible] LevelManager not found!");
            }
        }
    }

    public static void ResetCounters()
    {
        collectiblesCollected = 0;
        totalCollectibles = 0;
        Debug.Log("[Collectible] Counters reset.");
    }
}
