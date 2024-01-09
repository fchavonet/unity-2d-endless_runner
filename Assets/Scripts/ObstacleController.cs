using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // Variable to store the left edge position
    private float leftEdge;

    // Called when the script instance is being loaded
    private void Start()
    {
        // Calculate the left edge position in world coordinates relative to the screen
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    // Called every frame
    private void Update()
    {
        // Move the obstacle to the left based on the game speed and frame time
        transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        // Check if the obstacle has moved beyond the left edge
        if (transform.position.x < leftEdge)
        {
            // Destroy the obstacle when it's outside the playable area
            Destroy(gameObject);
        }
    }
}
