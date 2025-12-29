using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LosePanelUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text causeOfDeathText; // assign your "Text (TMP)" object here

    private string cause = "unknown";

    public void SetCauseOfDeath(string cause)
    {
        this.cause = cause;
        if (causeOfDeathText != null)
            causeOfDeathText.text = $"The Dinosaurs were apparently killed by {cause}.";
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        // TODO: replace with actual menu scene later
        SceneManager.LoadScene("MainMenu");
    }
}
