using UnityEngine;

public class ThunderCloud : MonoBehaviour
{
    public float damage = 25f; // 4 hits = Death
    public float knockbackForce = 10f; // Optional push

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Deal Damage
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(damage, "Electrocuted by Cloud");
            }



            // 3. Optional: Push the player back slightly so they don't get stuck inside
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                Vector3 pushDir = other.transform.position - transform.position;
                controller.Move(pushDir.normalized * knockbackForce * Time.deltaTime);
            }
        }
    }
}