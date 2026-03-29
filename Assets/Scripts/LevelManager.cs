using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using TMPro;
using System.Collections;

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

    [Header("Level Objects")]
    public GameObject level2Totems; // [NEW] Drag "Level2_TotemGroup" here
    public GameObject level3Rings;  // Drag "Level3_RingGroup" here

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

        // --- HIDE OBJECTS AT START ---
        if (level2Totems != null) level2Totems.SetActive(false);
        if (level3Rings != null) level3Rings.SetActive(false);

        // --- DELETE THESE LINES (THE ERROR IS HERE) ---
        // if (AudioManager.Instance != null)
        //    AudioManager.Instance.CheckMusic(SceneManager.GetActiveScene().buildIndex);
        // ----------------------------------------------

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

        TeleportToLevel(currentLevelIndex);
        
        // This function handles the music now!
        StartLevel(currentLevelIndex);
    }

    public void StartLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        currentObjectivesFound = 0;

        // --- MUSIC SWITCH (The New Line) ---
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelMusic(currentLevelIndex);
        }

        // --- EXISTING CODE BELOW ---
        switch (levelIndex)
        {
            case 1: totalObjectivesNeeded = 10; break;
            case 2: totalObjectivesNeeded = 3; break;
            case 3: totalObjectivesNeeded = 12; break;
            case 4: totalObjectivesNeeded = 1; break;
            default: totalObjectivesNeeded = 1; break;
        }

        if (characterSwitcher != null) characterSwitcher.SwitchCharacter(currentLevelIndex);
        if (skyboxManager != null) skyboxManager.ApplySkybox(currentLevelIndex);

        // --- LEVEL SPECIFIC OBJECTS ---
        if (levelIndex == 2 && level2Totems != null) level2Totems.SetActive(true);
        else if (level2Totems != null) level2Totems.SetActive(false);

        if (levelIndex == 3 && level3Rings != null) level3Rings.SetActive(true);
        else if (level3Rings != null) level3Rings.SetActive(false);

        UpdateMissionUI();

        if (levelTitleScript != null)
            levelTitleScript.ShowLevelTitle(currentLevelIndex);
    }

    // --- STANDARD FUNCTIONS ---
    public void OnClick_NextLevel()
    {
        Time.timeScale = 1f;
        if (proceedUI != null) proceedUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        int nextLevel = currentLevelIndex + 1;
        int spawnIndex = nextLevel - 1;

        if (spawnIndex < levelSpawnPoints.Length)
            StartCoroutine(TeleportRoutine(spawnIndex, nextLevel));
        else
        {
            Debug.Log("Game Finished! Going back to Main Menu...");
            LoadMainMenu();
        }
    }

    private IEnumerator TeleportRoutine(int spawnIndex, int nextLevel)
    {
        if (playerController != null)
        {
            playerController.enabled = false;
            yield return null;
            playerController.transform.position = levelSpawnPoints[spawnIndex].position;
            playerController.transform.rotation = levelSpawnPoints[spawnIndex].rotation;
            currentCheckpointPos = levelSpawnPoints[spawnIndex].position;
            yield return null;
            playerController.enabled = true;
        }
        StartLevel(nextLevel);
    }

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
        if (playerController != null) playerController.enabled = false;


        if (currentLevelIndex != 4)
        {
            if (proceedUI != null)
            {
                proceedUI.SetActive(true);
                if (finishText != null) finishText.text = "Congratulations!\nYou have finished Level " + currentLevelIndex;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void RegisterObjective() { totalObjectivesNeeded++; UpdateMissionUI(); }

    public void CollectObjective()
    {
        currentObjectivesFound++;
    
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
       
        if (playerController != null) { playerController.enabled = false; playerController.transform.position = currentCheckpointPos; playerController.enabled = true; }
        if (SurvivalStats.Instance != null) { SurvivalStats.Instance.currentThirst = SurvivalStats.Instance.maxThirst; SurvivalStats.Instance.isDead = false; }
        if (PlayerHealth.Instance != null) { PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth; PlayerHealth.Instance.isDead = false; if (PlayerHealth.Instance.losePanel != null) PlayerHealth.Instance.losePanel.SetActive(false); }
        Time.timeScale = 1f; Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
    }
}