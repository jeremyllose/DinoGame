using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter Setup")]
    [Tooltip("The destination to teleport the player or object to.")]
    public Transform destination;

    [Tooltip("Tag of the object that will trigger the teleport.")]
    public string targetTag = "Player";

    [Tooltip("Optional effect or sound toggle.")]
    public bool useEffects = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag) && destination != null)
        {
            GameObject obj = collision.gameObject;

            // Handle CharacterController (e.g., FPS Controller)
            CharacterController controller = obj.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                obj.transform.position = destination.position;
                controller.enabled = true;
            }
            else
            {
                // Handle Rigidbody-based objects
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.position = destination.position;
                    rb.linearVelocity = Vector3.zero; // optional: stop momentum
                }
                else
                {
                    obj.transform.position = destination.position;
                }
            }

            if (useEffects)
            {
                Debug.Log($"{obj.name} teleported to {destination.name}");
                // You can later add particle or sound effect here
            }
        }
    }
}
