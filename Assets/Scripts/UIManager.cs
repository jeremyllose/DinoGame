using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI collectibleText;   
    public TextMeshProUGUI missionText;       

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // ERROR FIXED: We removed the code that tried to read Collectible.totalCollectibles.
        // The LevelManager now handles updating the UI automatically when the level begins.
    }

    // This is still here if you need it, but LevelManager mostly uses UpdateMissionText now
    public void UpdateCollectibleUI(int collected, int total)
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"Collectibles: {collected}/{total}";
        }
    }

    public void UpdateMissionText(string mission)
    {
        if (missionText != null)
        {
            missionText.text = mission;
        }
    }
}