using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Spin around Y (Up)
    public float speed = 50f; // How fast it spins

    void Update()
    {
        // Spin forever
        transform.Rotate(rotationAxis * speed * Time.deltaTime);
    }
}