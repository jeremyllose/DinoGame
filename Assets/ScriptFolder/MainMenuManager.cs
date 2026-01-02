using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels (DRAG THESE IN!)")]
    public GameObject mainButtonsPanel; 
    public GameObject levelSelectPanel; 
    public GameObject creditsPanel;     

    private void Start()
    {
        // Sanity Check
        if (mainButtonsPanel == null || levelSelectPanel == null || creditsPanel == null)
        {
            Debug.LogError("MAIN MENU ERROR: Panels are missing! Drag them into the Inspector.");
            return;
        }

        ShowMainButtons();
        
        if (AudioManager.Instance != null) 
            AudioManager.Instance.CheckMusic(0);
    }

    // --- MAIN BUTTONS ---

    public void OnClick_Play()
    {
        mainButtonsPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void OnClick_Settings()
    {
        mainButtonsPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnClick_Quit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    // --- LEVEL SELECT ---

    public void OnClick_Level1() { LoadLevel(1); }
    public void OnClick_Level2() { LoadLevel(2); }
    public void OnClick_Level3() { LoadLevel(3); }
    public void OnClick_Level4() { LoadLevel(4); }

    private void LoadLevel(int levelIndex)
    {
        // Save the level choice so LevelManager knows where to teleport us
        PlayerPrefs.SetInt("StartLevel", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    // --- SEPARATE BACK BUTTONS ---

    // Assign this to the Back button in the LEVEL SELECT Panel
    public void OnClick_BackFromLevels()
    {
        ShowMainButtons();
    }

    // Assign this to the Exit/Back button in the CREDITS Panel
    public void OnClick_BackFromCredits()
    {
        ShowMainButtons();
    }

    // --- HELPER FUNCTION ---
    private void ShowMainButtons()
    {
        mainButtonsPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
}