using UnityEngine;
using StarterAssets; // Needed to talk to the Player Controller

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter Setup")]
    [Tooltip("The destination to teleport the player or object to.")]
    public Transform destination;

    [Tooltip("Tag of the object that will trigger the teleport.")]
    public string targetTag = "Player";

    [Tooltip("Optional effect or sound toggle.")]
    public bool useEffects = false;

    // Switch to OnTriggerEnter so you don't have to "bump" into it physically
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && destination != null)
        {
            GameObject obj = other.gameObject;

            // 1. Handle CharacterController (The Player)
            CharacterController controller = obj.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Disable controller to allow instant movement
                controller.enabled = false;

                // Move Player
                obj.transform.position = destination.position;
                
                // Rotation (Optional: Face the direction of the destination arrow)
                obj.transform.rotation = destination.rotation;

                // FIX FOR "FALLING FAST" (Reset Gravity)
                // We try to find the StarterAssets script to reset its internal fall speed
                FirstPersonController fps = obj.GetComponent<FirstPersonController>();
                if (fps != null)
                {
                    // This is a trick: Toggling the script resets some internal physics variables
                    fps.enabled = false; 
                    fps.enabled = true; 
                }

                // Re-enable controller
                controller.enabled = true;
            }
            // 2. Handle Physics Objects (Rigidbodies)
            else
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.position = destination.position;
                    rb.rotation = destination.rotation;
                    rb.linearVelocity = Vector3.zero; // STOP momentum so you don't fly out
                    rb.angularVelocity = Vector3.zero;
                }
                else
                {
                    obj.transform.position = destination.position;
                    obj.transform.rotation = destination.rotation;
                }
            }

            // 3. SET CHECKPOINT (The Update)
            // Now if you die, you respawn at this destination, not the start of the game.
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.SetCheckpoint(destination.position);
            }

            if (useEffects)
            {
                Debug.Log($"{obj.name} teleported to {destination.name}");
            }
        }
    }
}