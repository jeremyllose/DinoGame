using UnityEngine;

public class EnemyFollowAndKill : MonoBehaviour
{
    [Header("Chase Settings")]
    public float detectionRadius = 10f; // Distance to detect player
    public float moveSpeed = 3f;        // Movement speed
    public string playerTag = "Player"; // Tag to detect player

    private Transform player;
    private Vector3 startPosition;
    private bool isChasing = false;

    private void Start()
    {
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        // 🟡 Warn if collider is not set as trigger
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
            Debug.LogWarning($"{name}: Collider should be set to 'Is Trigger' for proper kill detection.");
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Begin chasing if player is close enough
        if (distance <= detectionRadius)
        {
            isChasing = true;
        }
        // Stop chasing if player escapes
        else if (isChasing && distance > detectionRadius * 1.5f)
        {
            isChasing = false;
        }

        // Movement behavior
        if (isChasing)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(player);
        }
        else
        {
            // Return home if not chasing
            Vector3 direction = (startPosition - transform.position);
            if (direction.magnitude > 0.1f)
            {
                transform.position += direction.normalized * moveSpeed * Time.deltaTime;
                transform.LookAt(startPosition);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        if (PlayerHealth.Instance != null && !PlayerHealth.Instance.isDead)
        {
            // 💀 Kill player instantly — same as Lava script
            PlayerHealth.Instance.TakeDamage(PlayerHealth.Instance.maxHealth, "Dinosaurs");
            Debug.Log("💀 Player killed by Dinosaurs!");
        }
        else
        {
            Debug.LogWarning("⚠️ PlayerHealth.Instance missing or player already dead.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
