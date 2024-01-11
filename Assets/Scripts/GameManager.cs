using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager Instance { get; private set; }

    [Space(10)]
    // Animator component reference
    public Animator animator;

    [Space(10)]
    // UI elements for game start and game over
    public TextMeshProUGUI gameStartText;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;

    [Space(10)]
    // Current player score in the game
    public float score;

    [Space(10)]
    // UI elements for displaying score and high score
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;

    // Reference to the PlayerController and the ObstacleGenerator
    private PlayerController player;
    private ObstacleGenerator spawner;

    [Space(10)]
    // Initial game speed and the rate at which it increases
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;

    // Current game speed
    public float gameSpeed { get; private set; }

    [Space(10)]
    // Variables to track the game state
    public bool isGameStarted = false;
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
        // Hide the mouse cursor at the beginning of the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find and store references to the PlayerController and ObstacleGenerator
        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<ObstacleGenerator>();

        // Initialize the "isGameStarted" animator parameter to false.
        animator.SetBool("isGameStarted", false);

        // Set up the game speed increase to 0
        gameSpeedIncrease = 0;

        // Deactivate the obstacle spawner
        spawner.gameObject.SetActive(false);
    }

    // Initialize a new game
    public void NewGame()
    {
        // Set up the game speed increase to default
        gameSpeedIncrease = 0.1f;
        
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
        animator.SetBool("isGameStarted", true);
    }

    // Triggered when the game starts
    public void GameStart()
    {
        // Hide game over UI elements
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        // Update the high score display
        UpdateHiscore();

        // Check if the initial jump has been performed and the game hasn't started yet
        if (Input.GetKeyDown(KeyCode.Space) && !isGameStarted)
        {
            isGameStarted = true;
            // Call the NewGame method to initialize the game
            NewGame();
            // Hide game start UI elements and reset animation state
            gameStartText.gameObject.SetActive(false);
            animator.SetBool("isGameStarted", true);
        }
    }

    // Called every frame
    private void Update()
    {
        // Check if the game is not over to allow player input
        if (!isGameOver)
        {
            // Call GameStart() to check for the initial jump and start the game
            GameStart();

            // Increase the game speed over time
            gameSpeed += gameSpeedIncrease * Time.deltaTime;

            // Update the score based on the current game speed
            score += gameSpeed * Time.deltaTime;
            scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        }
    }

    // Update and save the high score
    private void UpdateHiscore()
    {
        // Retrieve the current high score from player preferences
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        // Update the UI with the high score
        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");

        // Check if the game is over before updating the high score
        if (isGameOver)
        {
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

    // Triggered when the game is over
    public void GameOver()
    {
        // Stop the game speed increase and disable GameManager script
        gameSpeed = 0f;
        enabled = false;

        // Set up the death animation
        animator.SetBool("isGameOver", true);

        // Deactivate the obstacle spawner
        spawner.gameObject.SetActive(false);

        // Show game over UI elements
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        // Update the game over state
        isGameOver = true;

        // Update the high score display
        UpdateHiscore();
    }
}
