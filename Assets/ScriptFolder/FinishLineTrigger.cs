using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    [Header("UI Assignment")]
    [Tooltip("Drag your Victory/Proceed Panel here")]
    public GameObject victoryPanel; 

    private bool levelFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelFinished) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("End of Level Reached!");
            levelFinished = true;

            // 1. Force Open the Panel you dragged in
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("You forgot to drag the Victory Panel into the FinishLineTrigger script!");
            }

            // 2. Tell LevelManager to handle the rest (Freezing time, audio, etc.)
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.CompleteLevel();
            }
        }
    }
}