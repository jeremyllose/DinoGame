using UnityEngine;

public class FlyAtPlayer : MonoBehaviour
{
    public Transform player;            // Assign player in Inspector
    public float speed = 5f;             // Movement speed
    public float stopDistance = 0.5f;    // Distance before destroying
    public Color hitColor = Color.red;   // Color change on hit
    public string targetTag = "Obstacle"; // Tag to count as a hit

    private Renderer objRenderer;
    private Color originalColor;

    void OnEnable()
    {
        // Called when missile becomes active from TriggerProjectile
        if (objRenderer == null)
            objRenderer = GetComponent<Renderer>();

        if (objRenderer != null)
            originalColor = objRenderer.material.color;

        // Auto-destroy after 3 seconds regardless
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (player == null) return;

        // Move towards the player's position
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // Destroy if close enough (backup check)
        if (Vector3.Distance(transform.position, player.position) <= stopDistance)
        {
            Destroy(gameObject, 0.3f); // Small delay so Scorer can register
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            if (objRenderer != null)
                objRenderer.material.color = hitColor;

            Destroy(gameObject, 0.3f); // Small delay for Scorer
        }
    }
}
