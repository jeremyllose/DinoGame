using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject asteroidPrefab;
    public Transform playerTransform; 
    public int activeLevelID = 4; // Only spawn on Level 4

    [Header("Spawn Settings")]
    public float spawnInterval = 0.5f; 
    public Vector3 spawnAreaSize = new Vector3(20f, 1f, 20f); 

    [Header("Targeting Logic")]
    [Range(0f, 100f)] 
    public float accuracyChance = 50f; 
    public float shootSpeed = 40f;    

    private float timer;
    private CharacterController playerController; 

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }

        if (playerTransform != null)
            playerController = playerTransform.GetComponent<CharacterController>();
    }

    private void Update()
    {
        // --- THE FIX ---
        // If we are not on Level 4, stop the timer.
        if (LevelManager.Instance != null)
        {
            if (LevelManager.Instance.currentLevelIndex != activeLevelID)
            {
                return; 
            }
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRock();
            timer = 0;
        }
    }

    void SpawnRock()
    {
        // (Same logic as before)
        if (asteroidPrefab == null || playerTransform == null) return;

        Vector3 randomPos = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );
        Vector3 spawnPoint = transform.position + randomPos;

        GameObject rock = Instantiate(asteroidPrefab, spawnPoint, Random.rotation);
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        if (rb == null) return;

        bool isAccurateShot = (Random.value * 100f) < accuracyChance;

        if (isAccurateShot)
        {
            Vector3 targetPos = playerTransform.position;
            if (playerController != null)
            {
                float dist = Vector3.Distance(spawnPoint, playerTransform.position);
                float travelTime = dist / shootSpeed;
                targetPos += playerController.velocity * travelTime;
            }
            Vector3 direction = (targetPos - spawnPoint).normalized;
            rb.linearVelocity = direction * shootSpeed; 
        }
        else
        {
            rb.linearVelocity = new Vector3(Random.Range(-5f, 5f), -15f, Random.Range(-5f, 5f));
        }

        Destroy(rock, 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f); 
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
}