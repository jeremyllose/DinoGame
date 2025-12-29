using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraTourManager : MonoBehaviour
{
    [Header("Tour Cameras (in order)")]
    public CinemachineCamera[] cameras; // Tour cameras (e.g. 0–3 corners, 4 = top view)

    [Header("Final Camera")]
    public CinemachineCamera playerFollowCam; // The final camera that follows the player

    [Header("Tour Settings")]
    public float switchInterval = 1.5f;

    private int currentIndex = 0;

    void Start()
    {
        if (cameras.Length == 0)
        {
            Debug.LogWarning("❗ No cameras assigned in CameraTourManager!");
            return;
        }

        // Set priorities: start with the first camera active
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].Priority = (i == 0) ? 10 : 0;

        // Player follow camera should be lowest at start
        if (playerFollowCam != null)
            playerFollowCam.Priority = 0;

        StartCoroutine(PlayTour());
    }

    IEnumerator PlayTour()
    {
        while (currentIndex < cameras.Length)
        {
            for (int i = 0; i < cameras.Length; i++)
                cameras[i].Priority = (i == currentIndex) ? 10 : 0;

            Debug.Log($"🎥 Showing camera {currentIndex + 1}/{cameras.Length}");
            yield return new WaitForSeconds(switchInterval);
            currentIndex++;
        }

        Debug.Log("✅ Camera tour finished! Switching to Player Follow Camera...");

        // Lower all tour cameras' priorities
        foreach (var cam in cameras)
            cam.Priority = 0;

        // Raise player follow camera so CinemachineBrain blends to it
        if (playerFollowCam != null)
        {
            playerFollowCam.Priority = 20; // higher = active
            Debug.Log("🎯 Now following the player again!");
        }
        else
        {
            Debug.LogWarning("⚠️ No Player Follow Camera assigned!");
        }
    }
}
