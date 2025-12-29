using UnityEngine;
using Unity.Cinemachine;
public class CameraTourSetup : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera topLeft;
    public CinemachineCamera topRight;
    public CinemachineCamera bottomLeft;
    public CinemachineCamera bottomRight;

    [Header("Center & Distance Settings")]
    public Transform focusTarget;
    public float distance = 30f;
    public float height = 25f;

    [ContextMenu("Arrange Cameras Symmetrically")]
    void ArrangeCameras()
    {
        if (focusTarget == null)
        {
            Debug.LogWarning("❗ Please assign a Focus Target (e.g., your map center).");
            return;
        }

        Vector3 center = focusTarget.position;

        // Calculate 4 symmetric positions
        Vector3[] positions = new Vector3[4];
        positions[0] = center + new Vector3(-distance, height, distance); // Top-Left
        positions[1] = center + new Vector3(distance, height, distance);  // Top-Right
        positions[2] = center + new Vector3(-distance, height, -distance); // Bottom-Left
        positions[3] = center + new Vector3(distance, height, -distance);  // Bottom-Right

        CinemachineCamera[] cams = { topLeft, topRight, bottomLeft, bottomRight };
        string[] names = { "TopLeft", "TopRight", "BottomLeft", "BottomRight" };

        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i] == null) continue;

            cams[i].transform.position = positions[i];
            cams[i].transform.LookAt(center);

            Debug.Log($"📸 Positioned {names[i]} at {positions[i]} looking at {center}");
        }

        Debug.Log("✅ Camera tour setup completed successfully!");
    }
}
