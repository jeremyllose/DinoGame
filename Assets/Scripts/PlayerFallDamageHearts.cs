using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFallDamage : MonoBehaviour
{
    [Header("Fall Damage Settings")]
    public float minFallHeight = 5f;   // Start taking damage beyond this
    public float maxFallHeight = 20f;  // Instant death beyond this
    private float fallStartY;
    private bool isFalling = false;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
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
        float t = Mathf.InverseLerp(minFallHeight, maxFallHeight, fallDistance);
        float damage = Mathf.Lerp(1, PlayerHealth.Instance.maxHealth, t); 
        // 1 minimum heart, full health loss if maxFallHeight exceeded

        // ✅ Added cause of death: "fall damage"
        PlayerHealth.Instance.TakeDamage(damage, "fall damage");

        Debug.Log($"☠️ Fall distance: {fallDistance:F2} | Damage: {damage:F1}");
    }
}
