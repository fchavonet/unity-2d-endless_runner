using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Animator component reference
    public Animator animator; 

    // Singleton instance of the GameManager
    public static GameManager Instance { get; private set; }

    private PlayerController player;
    private ObstacleGenerator spawner;

    // Initial game speed and the rate at which it increases
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;

    // Current game speed
    public float gameSpeed { get; private set; }

    // Variable to track the game state
    public bool isGameOver = false;

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
            // If another instance already exists, destroy this one
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
        // Find and store references to the PlayerController and ObstacleGenerator
        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<ObstacleGenerator>();

        NewGame();
    }

    // Initialize a new game
    private void NewGame()
    {
        // Find all existing ObstacleController instances and destroy them
        ObstacleController[] obstacles = FindObjectsOfType<ObstacleController>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        // Set the initial game speed
        gameSpeed = initialGameSpeed;
        // Enable the GameManager script
        enabled = true;

        // Activate the player and obstacle spawner
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);

        // Reset the game over and dead animation state
        isGameOver = false;
        animator.SetBool("isDead", false);
    }

    // Triggered when the game is over
    public void GameOver()
    {
        // Stop the game speed increase and disable GameManager script
        gameSpeed = 0f;
        enabled = false;

        // Set up the death animation
        animator.SetBool("isDead", true);

        // Deactivate the player (remove //) and obstacle spawner
        // player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        // Update the game over state
        isGameOver = true;
    }

    // Called every frame
    private void Update()
    {
        // Increase the game speed over time
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
    }
}
