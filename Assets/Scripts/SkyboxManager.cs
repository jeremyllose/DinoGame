using UnityEngine;
using UnityEngine.InputSystem; // Required for new input system

public class SkyboxManager : MonoBehaviour
{
    [Header("Assign skybox materials for each level")]
    public Material[] skyboxes;

    private int currentLevel = 0;

    void Start()
    {
        if (skyboxes.Length > 0)
            ApplySkybox(currentLevel);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return; // safety if no keyboard is detected

        // Forward (N key)
        if (keyboard.nKey.wasPressedThisFrame)
        {
            currentLevel = (currentLevel + 1) % skyboxes.Length;
            ApplySkybox(currentLevel);
            Debug.Log("Skybox changed to Level " + currentLevel);
        }

        // Backward (B key)
        if (keyboard.bKey.wasPressedThisFrame)
        {
            currentLevel = (currentLevel - 1 + skyboxes.Length) % skyboxes.Length;
            ApplySkybox(currentLevel);
            Debug.Log("Skybox changed to Level " + currentLevel);
        }
    }

    public void ApplySkybox(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < skyboxes.Length)
        {
            RenderSettings.skybox = skyboxes[levelIndex];
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogWarning("No skybox assigned for level " + levelIndex);
        }
    }
}
