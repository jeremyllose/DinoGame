using UnityEngine;
using StarterAssets; 

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter Setup")]
    public Transform destination;
    public string targetTag = "Player";
    public bool useEffects = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && destination != null)
        {
            GameObject obj = other.gameObject;

            // 1. Reset Fall Damage (The Fix)
            // We do this BEFORE moving so it forgets any current fall state
            PlayerFallDamage fallScript = obj.GetComponent<PlayerFallDamage>();
            if (fallScript != null)
            {
                fallScript.ResetFall();
            }

            // 2. Handle CharacterController
            CharacterController controller = obj.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                
                // Move
                obj.transform.position = destination.position;
                obj.transform.rotation = destination.rotation;

                // Reset Physics/Gravity in FirstPersonController
                FirstPersonController fps = obj.GetComponent<FirstPersonController>();
                if (fps != null)
                {
                    // Reset internal gravity variables
                    fps.enabled = false; 
                    fps.enabled = true; 
                }

                controller.enabled = true;
            }
            // 3. Handle Physics Objects
            else
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.position = destination.position;
                    rb.rotation = destination.rotation;
                    rb.linearVelocity = Vector3.zero;
                }
                else
                {
                    obj.transform.position = destination.position;
                    obj.transform.rotation = destination.rotation;
                }
            }

            // 4. Reset Fall Damage AGAIN (Double Safety)
            // We do this AFTER moving to ensure the new position is registered as "safe"
            if (fallScript != null)
            {
                fallScript.ResetFall();
            }

            // 5. Set Checkpoint
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.SetCheckpoint(destination.position);
            }

            if (useEffects) Debug.Log($"Teleported to {destination.name}");
        }
    }
}