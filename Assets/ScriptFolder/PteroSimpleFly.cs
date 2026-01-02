using UnityEngine;

public class PteroSimpleFly : MonoBehaviour
{
    [Header("Flying Motion")]
    public float bobSpeed = 2f;      // How fast it floats up/down
    public float bobHeight = 0.5f;   // How high it floats
    public float tiltAmount = 30f;   // How much it tilts when turning
    public float smoothTime = 5f;    // How smooth the movement is

    private float _startY;
    private StarterAssets.StarterAssetsInputs _input;
    private Quaternion _startRotation;

    void Start()
    {
        // Try to find the input on the parent (PlayerCapsule)
        _input = GetComponentInParent<StarterAssets.StarterAssetsInputs>();
        
        // Save starting position/rotation relative to the parent
        _startY = transform.localPosition.y; 
        _startRotation = transform.localRotation;
    }

    void Update()
    {
        // 1. Bob Up and Down (Floating Effect)
        // We use localPosition so it stays attached to the player correctly
        float newY = _startY + (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        // 2. Tilt Body when turning
        if (_input != null)
        {
            // Calculate tilt based on A/D keys
            float targetZ = -_input.move.x * tiltAmount; // Bank Left/Right
            float targetX = _input.move.y * (tiltAmount * 0.5f); // Pitch Forward/Back slightly

            // Apply the rotation smoothly
            Quaternion targetRot = _startRotation * Quaternion.Euler(targetX, 0, targetZ);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smoothTime);
        }
    }
}