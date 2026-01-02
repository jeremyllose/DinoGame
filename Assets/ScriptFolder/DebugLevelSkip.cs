using UnityEngine;

public class DebugLevelSkip : MonoBehaviour
{
    public KeyCode skipKey = KeyCode.F12;

    void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            Debug.Log("DEBUG: Skipping to next zone...");
            if (LevelManager.Instance != null)
            {
                // CHANGE: ProceedToNextLevel() -> OnClick_NextLevel()
                LevelManager.Instance.OnClick_NextLevel();
            }
        }
    }
}