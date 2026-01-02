using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.05f; // How violent
    
    private Vector3 originalPos;

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        // Constant low rumble
        transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
    }
}