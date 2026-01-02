using UnityEngine;
using TMPro; 
using System.Collections;

public class LevelTitleFade : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public CanvasGroup canvasGroup;
    public float displayDuration = 3.0f;
    public float fadeDuration = 2.0f;

    // We keep track of the current fade so we can stop it if we switch levels fast
    private Coroutine fadeCoroutine;

    private void Start()
    {
        // Optional: If you want it to run automatically on game launch
        // ShowLevelTitle(LevelManager.Instance.currentLevelIndex);
        
        // BETTER: Start invisible and let LevelManager control it
        canvasGroup.alpha = 0f;
    }

    // --- NEW PUBLIC FUNCTION ---
    public void ShowLevelTitle(int levelIndex)
    {
        // 1. Set the text
        SetTitleText(levelIndex);

        // 2. Stop any previous fading (in case you skipped levels fast)
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        // 3. Start the new fade animation
        fadeCoroutine = StartCoroutine(ShowAndFade());
    }

    private void SetTitleText(int levelIndex)
    {
        if (titleText == null) return;

        switch (levelIndex)
        {
            case 1: titleText.text = "Level 1\nTutorial Stage"; break;
            case 2: titleText.text = "Level 2\nSurvival Stage"; break;
            case 3: titleText.text = "Level 3\nFlight Stage"; break;
            case 4: titleText.text = "Level 4\nEnd Game"; break;
            default: titleText.text = "Level " + levelIndex; break;
        }
    }

    private IEnumerator ShowAndFade()
    {
        // Reset to fully visible
        canvasGroup.alpha = 1f; 
        
        // Wait
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        // Ensure invisible
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}