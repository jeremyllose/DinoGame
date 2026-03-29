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

            // 1. Force Open the Panel
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("You forgot to drag the Victory Panel into the FinishLineTrigger script!");
            }

            // 2. Tell LevelManager to freeze time & play sound
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.CompleteLevel();
            }

            // 3. --- THE FIX: FORCE UNLOCK CURSOR ---
            // We must do this manually because LevelManager skips UI logic for Level 4
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}