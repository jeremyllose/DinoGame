using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 3f;
    public TextMeshProUGUI timerText;
    public bool autoStart = true;

    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        if (autoStart) StartTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = Mathf.Ceil(currentTime).ToString();
        }
        else
        {
            currentTime = 0;
            isRunning = false;
            timerText.text = "GO!";
            StartCoroutine(HideGoText()); // hide after delay
        }
    }

    public void StartTimer()
    {
        currentTime = startTime;
        isRunning = true;
    }

    public void ResetTimer()
    {
        isRunning = false;
        currentTime = startTime;
        timerText.text = startTime.ToString();
    }

    private IEnumerator HideGoText()
    {
        yield return new WaitForSeconds(1f); // wait 1 second
        timerText.text = ""; // clear text
    }
}
