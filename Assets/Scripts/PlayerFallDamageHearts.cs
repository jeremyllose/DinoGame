using UnityEngine;
using StarterAssets; 

[RequireComponent(typeof(CharacterController))]
public class PlayerFallDamage : MonoBehaviour
{
    [Header("Settings")]
    public int velociraptorLevelID = 1; 
    
    [Header("Fall Damage Settings")]
    public float minFallHeight = 5f;   
    public float maxFallHeight = 20f;  
    
    private float fallStartY;
    private bool isFalling = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (LevelManager.Instance != null)
        {
            if (LevelManager.Instance.currentLevelIndex != velociraptorLevelID)
            {
                isFalling = false; 
                return; 
            }
        }

        DetectFallStart();
        DetectLanding();
    }

    void DetectFallStart()
    {
        if (!controller.isGrounded && !isFalling)
        {
            fallStartY = transform.position.y;
            isFalling = true;
        }
    }

    void DetectLanding()
    {
        if (controller.isGrounded && isFalling)
        {
            float fallDistance = fallStartY - transform.position.y;

            if (fallDistance > minFallHeight)
            {
                ApplyFallDamage(fallDistance);
            }

            isFalling = false;
        }
    }

    void ApplyFallDamage(float fallDistance)
    {
        if (PlayerHealth.Instance == null) return;
        float t = Mathf.InverseLerp(minFallHeight, maxFallHeight, fallDistance);
        float damage = Mathf.Lerp(1, PlayerHealth.Instance.maxHealth, t); 
        PlayerHealth.Instance.TakeDamage(damage, "fall damage");
    }

    // --- THE NEW FUNCTION ---
    public void ResetFall()
    {
        isFalling = false;
        fallStartY = transform.position.y; // Reset "safe" height to current position
    }
}