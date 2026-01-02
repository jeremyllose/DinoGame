using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject asteroidPrefab;
    public Transform playerTransform; // Drag player here, or it auto-finds them

    [Header("Spawn Settings")]
    public float spawnInterval = 1.0f; // Seconds between rocks
    public Vector3 spawnAreaSize = new Vector3(50f, 1f, 50f); // Width, Height, Length of spawn box

    [Header("Targeting Logic")]
    [Range(0f, 100f)] 
    public float accuracyChance = 30f; // 30% chance to aim at player
    public float shootSpeed = 20f;     // How fast accurate shots fly

    private float timer;

    private void Start()
    {
        // Auto-find player if you forgot to drag them in
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRock();
            timer = 0;
        }
    }

    void SpawnRock()
    {
        if (asteroidPrefab == null) return;

        // 1. Calculate a random position inside the "Green Box"
        Vector3 randomPos = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Vector3 spawnPoint = transform.position + randomPos;

        // 2. Create the Rock
        GameObject rock = Instantiate(asteroidPrefab, spawnPoint, Random.rotation);
        
        // 3. Decide: Shoot at Player OR Just Drop?
        // Random.value returns 0.0 to 1.0. If accuracyChance is 30, we check against 0.3.
        bool isAccurateShot = (Random.value * 100f) < accuracyChance;

        if (isAccurateShot && playerTransform != null)
        {
            // AIM AT PLAYER
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (playerTransform.position - spawnPoint).normalized;
                rb.linearVelocity = direction * shootSpeed; // Fire!
            }
        }
        else
        {
            // RANDOM DROP (Optional: Add slight random drift so they don't fall perfectly straight)
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(Random.Range(-5f, 5f), -5f, Random.Range(-5f, 5f));
            }
        }

        // 4. Cleanup to prevent lag
        Destroy(rock, 10f);
    }

    // --- VISUAL AID ---
    // This draws a GREEN BOX in the Scene View so you can position it easily
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Semi-transparent Green
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
}