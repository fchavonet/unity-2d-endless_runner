using UnityEngine;

public class GroundController : MonoBehaviour
{
    // MeshRenderer component reference
    private MeshRenderer meshRenderer;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Assign the MeshRenderer components on script initialization
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Called every frame
    private void Update()
    {
        // Calculate scrolling speed based on game speed and ground scale
        float speed = GameManager.Instance.gameSpeed / transform.localScale.x;
    
        // Move the main texture offset of the material to create a scrolling effect
        meshRenderer.material.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
    }
}
