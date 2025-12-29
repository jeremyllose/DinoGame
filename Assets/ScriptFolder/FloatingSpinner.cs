using UnityEngine;

public class FloatingSpinner : MonoBehaviour
{
    public Transform shadow;              // Optional: assign shadow in Inspector
    public float spinSpeed = 90f;         // Degrees per second
    public Vector3 rotationAxis = Vector3.up; // Choose X, Y, Z rotation in Inspector

    void Update()
    {
        // Rotate continuously based on chosen axis
        transform.Rotate(rotationAxis.normalized * spinSpeed * Time.deltaTime);

        // Keep shadow aligned under spinner
        if (shadow != null)
        {
            shadow.position = new Vector3(transform.position.x, shadow.position.y, transform.position.z);
        }
    }
}
