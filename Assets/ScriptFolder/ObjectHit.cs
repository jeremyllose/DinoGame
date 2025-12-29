using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    public Color hitColor = Color.red; // Color to change to on hit
    public string obstacleTag = "Obstacle"; // Tag for obstacle
    public string playerTag = "Player"; // Tag for player

    void OnCollisionEnter(Collision collision)
    {
        // Check if THIS object is an obstacle and the thing hitting it is the player
        if (CompareTag(obstacleTag) && collision.gameObject.CompareTag(playerTag))
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
