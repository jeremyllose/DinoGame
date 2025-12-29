using UnityEngine;

public class Dropper : MonoBehaviour
{
    public GameObject objectToDrop;   // The object that will fall
    public string playerTag = "Player";
    public string obstacleTag = "Obstacle"; // Set this in Inspector or here
    public Color hitColor = Color.red;      // Default to red

    private Rigidbody dropRb;

    void Start()
    {
        if (objectToDrop != null)
        {
            dropRb = objectToDrop.GetComponent<Rigidbody>();
            if (dropRb != null)
            {
                dropRb.isKinematic = false;   // Lock it in place at start
                dropRb.useGravity = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player triggered the drop
        if (other.CompareTag(playerTag) && dropRb != null)
        {
            dropRb.isKinematic = true;  // Unlock it
            dropRb.useGravity = true;    // Make it fall

            Debug.Log("Dropper triggered! Object is falling.");
        }

        // If the falling object (this GameObject) collides with the player
        if (CompareTag(obstacleTag) && other.CompareTag(playerTag))
        {
            Renderer obstacleRenderer = GetComponent<Renderer>();
            if (obstacleRenderer != null)
            {
                obstacleRenderer.material.color = hitColor;
                Debug.Log("Obstacle hit by player! Turning red.");
            }
        }
    }
}
