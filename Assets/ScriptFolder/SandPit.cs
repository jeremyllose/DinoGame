using UnityEngine;
using StarterAssets; 

public class SandPit : MonoBehaviour
{
    [Header("Sand Pit Settings")]
    public float slowSpeed = 2.0f;       // Reduced speed
    public float pullForce = 2.0f;       // How strong it sucks you to the center
    
    private float _originalMoveSpeed;
    private float _originalSprintSpeed;
    private FirstPersonController _controller;
    private CharacterController _characterController; // Needed to move the player manually

    // 1. ENTER: Slow the player down
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _controller = other.GetComponent<FirstPersonController>();
            _characterController = other.GetComponent<CharacterController>();

            if (_controller != null)
            {
                _originalMoveSpeed = _controller.MoveSpeed;
                _originalSprintSpeed = _controller.SprintSpeed;

                _controller.MoveSpeed = slowSpeed;
                _controller.SprintSpeed = slowSpeed; 
                
                Debug.Log("Entered Sand Pit - Sinking!");
            }
        }
    }

    // 2. STAY: Suck the player towards the center
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _characterController != null)
        {
            // Calculate direction from Player -> Center of Sand Pit
            Vector3 directionToCenter = transform.position - other.transform.position;
            
            // Ignore Y (Height) so we don't pull them into the ground or sky
            directionToCenter.y = 0;
            directionToCenter.Normalize();

            // Move the player slightly towards the center
            _characterController.Move(directionToCenter * pullForce * Time.deltaTime);
        }
    }

    // 3. EXIT: Restore speed
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _controller != null)
        {
            _controller.MoveSpeed = _originalMoveSpeed;
            _controller.SprintSpeed = _originalSprintSpeed;
            
            Debug.Log("Escaped Sand Pit.");
        }
    }
}