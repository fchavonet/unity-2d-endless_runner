using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager Instance { get; private set; }

    // Initial game speed and the rate at which it increases
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;

    // Current game speed
    public float gameSpeed { get; private set; }

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // If this instance is the current GameManager instance, set it to null on destruction
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Called when the script instance is being loaded
    private void Start()
    {
        NewGame();
    }

    // Initialize a new game
    private void NewGame()
    {
        // Set the initial game speed
        gameSpeed = initialGameSpeed;
    }

    // Called every frame
    private void Update()
    {
        // Increase the game speed over time
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
    }
}
