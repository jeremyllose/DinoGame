using UnityEngine;
using StarterAssets; 

public class PauseMenu : MonoBehaviour
{
    [Header("UI Assignments")]
    public GameObject pauseMenuPanel; 

    [Header("Player Reference")]
    public FirstPersonController playerController; 

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        // --- NEW GUARD CLAUSE ---
        // 1. Check if LevelManager exists and if the Proceed (Win) UI is active
        if (LevelManager.Instance != null && 
            LevelManager.Instance.proceedUI != null && 
            LevelManager.Instance.proceedUI.activeSelf)
        {
            return; // STOP HERE. Do not allow pausing.
        }

        // 2. Check if PlayerHealth exists and if the Lose UI is active
        if (PlayerHealth.Instance != null && 
            PlayerHealth.Instance.losePanel != null && 
            PlayerHealth.Instance.losePanel.activeSelf)
        {
            return; // STOP HERE.
        }
        // ------------------------

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null) playerController.enabled = false;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null) playerController.enabled = true;
    }

    // --- BUTTON FUNCTIONS ---
    public void OnClick_Checkpoint()
    {
        ResumeGame(); 
        if (LevelManager.Instance != null) LevelManager.Instance.RespawnAtCheckpoint();
    }

    public void OnClick_RestartLevel()
    {
        if (LevelManager.Instance != null) LevelManager.Instance.RestartLevel();
    }

    public void OnClick_Quit()
    {
        if (LevelManager.Instance != null) LevelManager.Instance.QuitGame();
    }
    
    // Added this back in case you need it for the button
    public void OnClick_MainMenu()
    {
        if (LevelManager.Instance != null) LevelManager.Instance.LoadMainMenu();
    }
}