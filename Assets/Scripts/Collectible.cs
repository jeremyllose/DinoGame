using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Settings")]
    public int myLevelID = 1; // 1 for Coins, 2 for Totems, 3 for Rings

    // DELETE THE START() FUNCTION
    // We do NOT want these to register themselves anymore.
    // The LevelManager already knows we need 15 (or whatever you set manually).

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance != null)
            {
                // Only collect if it's the right level
                if (LevelManager.Instance.currentLevelIndex == myLevelID)
                {
                    LevelManager.Instance.CollectObjective();
                    
                  
                    Destroy(gameObject);
                }
            }
        }
    }
}