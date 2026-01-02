using System.Collections;
using UnityEngine;

public class TRexAbility : MonoBehaviour
{
    [Header("Bite Settings")]
    public float biteRange = 3f;        // Distance in front of T-Rex
    public float cooldown = 5f;         // Cooldown between bites
    private float lastUsedTime = -999f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("TRexAbility: No Animator found!");
    }

    private void Update()
    {
        // Change KeyCode.F to your preferred ability key
        if (Input.GetKeyDown(KeyCode.F) && Time.time > lastUsedTime + cooldown)
        {
            Bite();
            lastUsedTime = Time.time;
        }
    }

    private void Bite()
    {
        // Trigger bite animation
        animator.SetTrigger("isAttacking");

        // Raycast forward to detect hazards
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up; // approx head height
        if (Physics.Raycast(origin, transform.forward, out hit, biteRange))
        {
            if (hit.collider.CompareTag("Hazard"))
            {
                hit.collider.gameObject.SetActive(false); // temporarily disable hazard
                StartCoroutine(ReenableHazard(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator ReenableHazard(GameObject hazard)
    {
        yield return new WaitForSeconds(3f); // hazard comes back after 3 seconds
        hazard.SetActive(true);
    }
}
