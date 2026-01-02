using System.Collections;
using UnityEngine;
using StarterAssets; // to access your controller script

public class PteroAbility : MonoBehaviour
{
    public float boostMultiplier = 2f;  // how much faster
    public float duration = 2f;          // how long boost lasts
    public float cooldown = 5f;          // cooldown between uses

    private float lastUsedTime = -999f;
    private FirstPersonController controller;

    private void Start()
    {
        controller = GetComponent<FirstPersonController>();
        if (controller == null)
            Debug.LogError("PteroAbility: No FirstPersonController found!");
    }

    private void Update()
    {
        // Change KeyCode.LeftShift to your preferred boost key
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastUsedTime + cooldown)
        {
            StartCoroutine(Boost());
            lastUsedTime = Time.time;
        }
    }

    private IEnumerator Boost()
    {
        // Temporarily multiply movement speeds
        controller.MoveSpeed *= boostMultiplier;
        controller.SprintSpeed *= boostMultiplier;

        yield return new WaitForSeconds(duration);

        // Restore original speeds
        controller.MoveSpeed /= boostMultiplier;
        controller.SprintSpeed /= boostMultiplier;
    }
}
