using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpoint;
    private CharacterController controller;

    void Start()
    {
        // Default checkpoint = starting position
        lastCheckpoint = transform.position;
        controller = GetComponent<CharacterController>();
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint set: " + lastCheckpoint);
    }

    public void Respawn()
    {
        Debug.Log("Respawning at: " + lastCheckpoint);

        // Disable controller to safely move player
        if (controller != null) controller.enabled = false;

        transform.position = lastCheckpoint;

        if (controller != null) controller.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FallZone"))
        {
            Debug.Log("FallZone (Collision) -> Respawn");
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallZone"))
        {
            Debug.Log("FallZone (Trigger) -> Respawn");
            Respawn();
        }
    }
}
