using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public float damage = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(damage, "Crushed by Meteor");
            }
            // Optional: Destroy meteor on impact
            Destroy(gameObject);
        }
    }
}