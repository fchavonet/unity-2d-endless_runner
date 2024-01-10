using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Animator component reference
    public Animator animator;

    // UI elementsz
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public Button retryButton;

    // Singleton instance of the GameManager
    public static GameManager Instance { get; private set; }

    // Reference to the PlayerController and the ObstacleGenerator
    private PlayerController player;
    private ObstacleGenerator spawner;

    // Initial game speed and the rate at which it increases
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;

    // Current game speed
    public float gameSpeed { get; private set; }

    // Current player score in the game
    public float score;

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
    public void NewGame()
    {
        // Deactivate the player to reset animation
        player.gameObject.SetActive(false);

        // Find all existing ObstacleController instances and destroy them
        ObstacleController[] obstacles = FindObjectsOfType<ObstacleController>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        // Set the initial game speed
        gameSpeed = initialGameSpeed;

        // Reset the score
        score = 0f;

        // Enable the GameManager script
        enabled = true;

        // Activate the player and obstacle spawner
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        
        // Hide game over UI elements
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        // Reset the game over and dead animation state
        isGameOver = false;
        animator.SetBool("isDead", false);

        // Update the high score display
        UpdateHiscore();
    }

    // Triggered when the game is over
    public void GameOver()
    {
        // Stop the game speed increase and disable GameManager script
        gameSpeed = 0f;
        enabled = false;

        // Set up the death animation
        animator.SetBool("isDead", true);

        // Deactivate the obstacle spawner
        spawner.gameObject.SetActive(false);
        //
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        // Update the game over state
        isGameOver = true;

        // Update the high score display
        UpdateHiscore();
    }

    // Called every frame
    private void Update()
    {
        // Increase the game speed over time
        gameSpeed += gameSpeedIncrease * Time.deltaTime;

        // Update the score based on the current game speed
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }

    private void UpdateHiscore()
    {
        // Retrieve the current high score from player preferences
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        // If the current score is higher than the stored high score, update it
        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        // Update the UI with the high score
        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
