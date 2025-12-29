using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float dropDelay = 3f;         // Time before it starts falling
    public Color hitColor = Color.red;   // Color when hitting ground/player
    public string groundTag = "Ground";
    public string playerTag = "Player";  // Player's tag
    public Transform shadow;             // Assign shadow object in Inspector
    public float spinSpeed = 90f;        // Degrees per second
    public float colorResetDelay = 1f;   // How long before reverting color

    private Rigidbody rb;
    private Renderer objRenderer;
    private Color originalColor;
    private bool hasDropped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objRenderer = GetComponent<Renderer>();
        originalColor = objRenderer.material.color;

        // Start invisible
        objRenderer.enabled = false;

        // Disable gravity at start
        rb.useGravity = false;
    }

    void Update()
    {
        // Rotate continuously
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);

        // Keep shadow aligned before falling
        if (!hasDropped && shadow != null)
        {
            shadow.position = new Vector3(transform.position.x, shadow.position.y, transform.position.z);
        }

        // Drop after delay
        if (!hasDropped && Time.time >= dropDelay)
        {
            hasDropped = true;
            rb.useGravity = true;
            objRenderer.enabled = true; // Make visible
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag) || collision.gameObject.CompareTag(playerTag))
        {
            objRenderer.material.color = hitColor;
            CancelInvoke(nameof(ResetColor)); // Avoid stacking invokes
            Invoke(nameof(ResetColor), colorResetDelay);
        }
    }

    void ResetColor()
    {
        objRenderer.material.color = originalColor;
    }
}
