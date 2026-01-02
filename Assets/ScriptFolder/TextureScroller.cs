using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    [Header("Settings")]
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.1f;
    
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (_renderer != null)
        {
            // Calculate the new offset based on time
            float offsetX = Time.time * scrollSpeedX;
            float offsetY = Time.time * scrollSpeedY;

            // Apply it to the material
            _renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
    }
}