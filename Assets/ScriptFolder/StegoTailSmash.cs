using UnityEngine;

public class StegoTailSmash : MonoBehaviour
{
    public float range = 2f;
    public LayerMask breakableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, breakableLayer))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
