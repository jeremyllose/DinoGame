using UnityEngine;

public class Lava : MonoBehaviour
{
    [Header("Lava Settings")]
    public float damagePerSecond = 9999f;
    public bool instantDeath = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (instantDeath)
            {
                PlayerHealth.Instance.TakeDamage(
                    PlayerHealth.Instance.maxHealth,
                    "Lava"
                );
                Debug.Log("💀 Player touched lava — instant death by Lava!");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!instantDeath && other.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(
                damagePerSecond * Time.deltaTime,
                "Lava"
            );
        }
    }
}
