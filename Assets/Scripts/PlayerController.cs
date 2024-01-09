using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Animator component reference
    private Animator animator; 
    // CharacterController component reference
    private CharacterController character;

    // Movement direction vector
    private Vector3 direction;

    // Gravitational force applied to the player and force for jumping
    public float gravity = 9.81f * 2f;
    public float jumpForce = 8f;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Assign necessary components on script initialization
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
    }

    // Called when the object becomes enabled and active
    private void OnEnable()
    {
        // Reset movement direction when player becomes active
        direction = Vector3.zero;
    }

    // Called every frame
    private void Update()
    {
        // Apply gravity to the player's vertical movement
        direction += Vector3.down * gravity * Time.deltaTime;

        // Check if the game is not over to allow player input
        if (!GameManager.Instance.isGameOver)
        {
            // Check if player is grounded
            if (character.isGrounded)
            {
                // Reset vertical movement when grounded and set up animation
                direction = Vector3.down;
                animator.SetBool("isGrounded", true);

                // Check for jump input
                if (Input.GetButtonDown("Jump"))
                {
                    // Apply upward force for jumping and set up animation
                    direction = Vector3.up * jumpForce;
                    animator.SetBool("isGrounded", false);
                }
            }
        }

        // Move the character based on the calculated direction
        character.Move(direction * Time.deltaTime);
    }

    // Triggered when colliding with other colliders
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is tagged as an obstacle
        if (other.CompareTag("Obstacle"))
        {
             // Trigger the GameOver method in the GameManager
            GameManager.Instance.GameOver();
        }
    }
}
