using UnityEngine;

public class TriggerProjectile : MonoBehaviour
{
    public GameObject missile;      // Assign the missile in Inspector
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && missile != null)
        {
            missile.SetActive(true);
        }
    }
}
