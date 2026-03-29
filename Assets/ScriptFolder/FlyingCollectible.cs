using UnityEngine;

public class FlyingCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance != null)
            {
                // Only collect if we are on Level 3
                if (LevelManager.Instance.currentLevelIndex == 3)
                {
                    LevelManager.Instance.CollectObjective(); // Counts 1/12, 2/12...
                    
    

                    Destroy(gameObject);
                }
            }
        }
    }
}