using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets; // Needed for FirstPersonController
using TMPro; 
using System.Collections; // Needed for Coroutines (The Teleport Fix)

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Dependencies")]
    public FirstPersonController playerController;
    public CharacterSwitcher characterSwitcher;
    public SkyboxManager skyboxManager; 

    [Header("UI Assignments")]
    public GameObject proceedUI; 
    public TextMeshProUGUI finishText; 
    public LevelTitleFade levelTitleScript; 

    [Header("Level Zones")]
    public Transform[] levelSpawnPoints; 

    [Header("Game State")]
    public int currentLevelIndex = 1; 
    
    [Header("Level Progress")]
    public int totalObjectivesNeeded = 0;
    public int currentObjectivesFound = 0;

    [Header("Checkpoint System")]
    public Vector3 currentCheckpointPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (proceedUI != null) proceedUI.SetActive(false);

        // Sync Music
        if (AudioManager.Instance != null)
            AudioManager.Instance.CheckMusic(SceneManager.GetActiveScene().buildIndex);

        // Check Saves
        if (PlayerPrefs.HasKey("RestartedLevel"))
        {
            currentLevelIndex = PlayerPrefs.GetInt("RestartedLevel");
            PlayerPrefs.DeleteKey("RestartedLevel");
        }
        else if (PlayerPrefs.HasKey("StartLevel"))
        {
            currentLevelIndex = PlayerPrefs.GetInt("StartLevel");
            PlayerPrefs.DeleteKey("StartLevel");
        }

        // Initial Teleport (Safe to do directly in Start)
        TeleportToLevel(currentLevelIndex);

        StartLevel(currentLevelIndex);
    }

    public void StartLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        currentObjectivesFound = 0;
        totalObjectivesNeeded = 0; 

        if (characterSwitcher != null) characterSwitcher.SwitchCharacter(currentLevelIndex);
        if (skyboxManager != null) skyboxManager.ApplySkybox(currentLevelIndex);
        
        UpdateMissionUI();

        if (levelTitleScript != null)
            levelTitleScript.ShowLevelTitle(currentLevelIndex);
    }

    // --- TELEPORT LOGIC (THE FIX) ---

    public void OnClick_NextLevel()
    {
        // 1. Unfreeze time
        Time.timeScale = 1f;

        if (proceedUI != null) proceedUI.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        int nextLevel = currentLevelIndex + 1;
        int spawnIndex = nextLevel - 1; 

        if (spawnIndex < levelSpawnPoints.Length)
        {
            // Start the Coroutine to teleport safely
            StartCoroutine(TeleportRoutine(spawnIndex, nextLevel));
        }
        else
        {
            Debug.Log("Game Finished! Going back to Main Menu...");
            LoadMainMenu();
        }
    }

    // This Coroutine waits for the frame to end before moving logic
    private IEnumerator TeleportRoutine(int spawnIndex, int nextLevel)
    {
        if (playerController != null)
        {
            // Disable controller
            playerController.enabled = false;
            
            // WAIT 1 FRAME (Crucial!)
            yield return null; 

            // Move Player
            playerController.transform.position = levelSpawnPoints[spawnIndex].position;
            playerController.transform.rotation = levelSpawnPoints[spawnIndex].rotation;

            // Update Checkpoint
            currentCheckpointPos = levelSpawnPoints[spawnIndex].position;

            // WAIT 1 FRAME (Let physics catch up)
            yield return null;

            // Re-enable controller
            playerController.enabled = true;
        }

        StartLevel(nextLevel);
    }
    
    // Helper for Start()
    private void TeleportToLevel(int levelIndex)
    {
        int spawnIndex = levelIndex - 1;
        if (playerController != null && levelSpawnPoints != null && spawnIndex < levelSpawnPoints.Length)
        {
            playerController.enabled = false; 
            playerController.transform.position = levelSpawnPoints[spawnIndex].position;
            playerController.transform.rotation = levelSpawnPoints[spawnIndex].rotation; 
            playerController.enabled = true;
            currentCheckpointPos = levelSpawnPoints[spawnIndex].position;
        }
    }

    // --- STANDARD FUNCTIONS ---

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        PlayerPrefs.SetInt("RestartedLevel", currentLevelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0); 
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void CompleteLevel()
    {
        Time.timeScale = 0f;
        
        // Disable input so they can't look around
        if (playerController != null) playerController.enabled = false;

        if (AudioManager.Instance != null) 
            AudioManager.Instance.PlaySFX(AudioManager.Instance.winSound);

        if (proceedUI != null)
        {
            proceedUI.SetActive(true);
            if (finishText != null) finishText.text = "Congratulations!\nYou have finished Level " + currentLevelIndex;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RegisterObjective() { totalObjectivesNeeded++; UpdateMissionUI(); }
    public void CollectObjective() 
    { 
        currentObjectivesFound++; 
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(AudioManager.Instance.coinPickup);
        UpdateMissionUI(); 
        if (currentObjectivesFound >= totalObjectivesNeeded && totalObjectivesNeeded > 0) CompleteLevel(); 
    }

    private void UpdateMissionUI()
    {
        if (UIManager.Instance != null)
        {
            string baseText = GetMissionText(currentLevelIndex);
            if (totalObjectivesNeeded > 0) baseText += $" ({currentObjectivesFound}/{totalObjectivesNeeded})";
            UIManager.Instance.UpdateMissionText(baseText);
        }
    }

    public string GetMissionText(int levelIndex)
    {
        switch (levelIndex)
        {
            case 1: return "Level 1: Collect items.";
            case 2: return "Level 2: Activate Water Totems."; 
            case 3: return "Level 3: Fly through hoops."; 
            case 4: return "Level 4: Survive the volcano."; 
            default: return "Survival Mode.";
        }
    }
    public void SetCheckpoint(Vector3 newPos) { currentCheckpointPos = newPos; }
    public void RespawnAtCheckpoint() 
    { 
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(AudioManager.Instance.loseSound);
        if (playerController != null) { playerController.enabled = false; playerController.transform.position = currentCheckpointPos; playerController.enabled = true; }
        if (SurvivalStats.Instance != null) { SurvivalStats.Instance.currentThirst = SurvivalStats.Instance.maxThirst; SurvivalStats.Instance.isDead = false; }
        if (PlayerHealth.Instance != null) { PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth; PlayerHealth.Instance.isDead = false; if (PlayerHealth.Instance.losePanel != null) PlayerHealth.Instance.losePanel.SetActive(false); }
        Time.timeScale = 1f; Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
    }
}