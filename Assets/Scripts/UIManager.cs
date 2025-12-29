using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI collectibleText;   // TMP text for collectibles
    public TextMeshProUGUI missionText;       // TMP text for mission description

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Initialize mission text at start
        if (missionText != null && LevelManager.Instance != null)
            missionText.text = LevelManager.Instance.GetMissionText(LevelManager.Instance.currentLevel + 1);

        // Initialize collectible UI at start
        UpdateCollectibleUI(Collectible.collectiblesCollected, Collectible.totalCollectibles);
    }

    public void UpdateCollectibleUI(int collected, int total)
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"Collectibles: {collected}/{total}";
            Debug.Log($"[UIManager] Updated UI -> {collected}/{total}");
        }
        else
        {
            Debug.LogWarning("[UIManager] Collectible Text not assigned!");
        }
    }

    public void UpdateMissionText(string mission)
    {
        if (missionText != null)
        {
            missionText.text = mission;
            Debug.Log($"[UIManager] Mission updated -> {mission}");
        }
        else
        {
            Debug.LogWarning("[UIManager] Mission Text not assigned!");
        }
    }
}
