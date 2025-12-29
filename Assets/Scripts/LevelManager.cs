using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Setup")]
    public Transform[] levelZones;   // Player spawn/teleport points
    public int currentLevel = 0;     // Current level index

    [Header("UI")]
    public GameObject proceedUI;     // "Proceed to next level" panel/button

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize mission text via UIManager
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateMissionText(GetMissionText(currentLevel + 1));

        if (proceedUI != null)
            proceedUI.SetActive(false);
    }

    public void CompleteLevel()
    {
        Debug.Log("Level Completed!");

        // Show "Proceed" UI instead of auto-teleport
        if (proceedUI != null)
            proceedUI.SetActive(true);
    }

    public void ProceedToNextLevel()
    {
        if (currentLevel < levelZones.Length - 1)
        {
            currentLevel++;
            
            // Update mission text via UIManager
            if (UIManager.Instance != null)
                UIManager.Instance.UpdateMissionText(GetMissionText(currentLevel + 1));

            // Teleport player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = levelZones[currentLevel].position;
            else
                Debug.LogWarning("Player not found! Make sure player has 'Player' tag.");
        }
        else
        {
            Debug.Log("All levels completed!");
            // Show final win screen or credits here
        }

        if (proceedUI != null)
            proceedUI.SetActive(false);
    }

    public string GetMissionText(int level)
    {
        switch (level)
        {
            case 1: return "Mission: Collect all collectibles to finish this level.";
            case 2: return "Mission: Find the hidden key and unlock the door.";
            case 3: return "Mission: Defeat all enemies in the area.";
            default: return "Mission: Explore and discover the secrets.";
        }
    }
}
