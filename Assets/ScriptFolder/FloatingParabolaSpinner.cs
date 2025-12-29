using UnityEngine;

public class FloatingParabolaSpinner : MonoBehaviour
{
    [Header("Floating & Spinning")]
    public Transform shadow;
    public float spinSpeed = 90f;
    public Vector3 rotationAxis = Vector3.up;
    public float parabolaHeight = 1f;   // max height of parabola
    public float parabolaSpeed = 1f;    // speed of bobbing

    [Header("Hit Settings")]
    public Color hitColor = Color.red; 
    public string playerTag = "Player"; 

    private Vector3 startPos;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        startPos = transform.position;

        // ✅ disable physics if Rigidbody exists
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void Update()
    {
        // Rotate
        transform.Rotate(rotationAxis.normalized * spinSpeed * Time.deltaTime);

        // Float in parabola motion
        float t = Time.time * parabolaSpeed;
        float parabola = Mathf.Sin(t) * parabolaHeight; 
        transform.position = new Vector3(startPos.x, startPos.y + parabola, startPos.z);

        // Keep shadow aligned
        if (shadow != null)
        {
            shadow.position = new Vector3(transform.position.x, shadow.position.y, transform.position.z);
        }
    }

    // ✅ Trigger detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (rend != null)
            {
                rend.material.color = hitColor;
            }
            Debug.Log("Spinner hit by player (trigger). Turning red!");
        }
    }
}
