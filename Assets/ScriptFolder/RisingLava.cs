using UnityEngine;

public class RisingLava : MonoBehaviour
{
    [Header("Settings")]
    public float riseSpeed = 0.5f; // How fast it goes up
    public float damage = 9999f;   // Instant Death

    void Update()
    {
        // 1. Move the lava UP forever
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 2. Kill the player if they touch it
        if (other.CompareTag("Player"))
        {
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(damage, "Burned by Lava");
            }
        }
    }
}