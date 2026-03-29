using UnityEngine;
using StarterAssets; // Needed to freeze the player

public class SecretWinTrigger : MonoBehaviour
{
    [Header("UI Assignments")]
    [Tooltip("Drag the 'secretVictoryPanel' from your Canvas here")]
    public GameObject secretVictoryPanel; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Secret Ending Triggered!");

            // 1. Safety Check: Did you forget to drag the panel?
            if (secretVictoryPanel == null)
            {
                Debug.LogError("ERROR: You forgot to drag the SecretVictoryPanel into the script slot!");
                return;
            }
            
            // 2. Show the Panel
            secretVictoryPanel.SetActive(true);

            // 3. Freeze Time
            Time.timeScale = 0f;
            
            // 4. Disable Player Input
            FirstPersonController controller = other.GetComponent<FirstPersonController>();
            if (controller != null) controller.enabled = false;

            // 5. Unlock Cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


        }
    }
}