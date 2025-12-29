using UnityEngine;
using TMPro; // For TextMeshPro
using System.Collections.Generic;

public class Scorer : MonoBehaviour
{
    public string targetTag = "Obstacle"; // Tag of objects that count as hits
    public TMP_Text scoreText; // Assign your TMP text here in the Inspector

    private int score = 0;
    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();

    void Start()
    {
        UpdateScoreUI();
    }

    // For solid obstacles (using colliders, not triggers)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag) && !hitObjects.Contains(collision.gameObject))
        {
            AddScore(collision.gameObject);
        }
    }

    // For floating spinners / parabola objects (set collider as Trigger)
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !hitObjects.Contains(other.gameObject))
        {
            AddScore(other.gameObject);
        }
    }

    void AddScore(GameObject obj)
    {
        hitObjects.Add(obj);
        score++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Hits Taken: " + score;
        }
    }
}
