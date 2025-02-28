using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 5; // Speed of character
    [SerializeField] int playerNum; // Either 0 for Player1 or 1 for Player2, set in the inspector
    [SerializeField] float climbSpeed = 3;
    private Vector2 movementInput;
    private PlayerInput playerInput;
    private Gamepad playerGamepad;
    private bool isClimbing = false;
    private Rigidbody rb;
    private float jumpForce = 5f;

    void Start() {
      
        //Assigns a controller to the player
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(Gamepad.all[playerNum]);
        playerGamepad = Gamepad.all[playerNum];

        // Gets the rigid body component
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Debug.Log("Movement x: " + movementInput.x + " y: " + movementInput.y);
        if (IsGrounded() && movementInput.y <= 0.5f) { isClimbing = false; rb.useGravity = true; }
        // Moves the Player
        if (isClimbing) {
            rb.linearVelocity = new Vector3(movementInput.x * climbSpeed, movementInput.y * climbSpeed, rb.linearVelocity.z);
        } else {
            transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime);
        }
    }

    // Moves the character using Unity's Input System
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    public Gamepad GetPlayerGamepad() { return playerGamepad; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            isClimbing = true;
            rb.useGravity = false; // Disable gravity to allow climbing
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            isClimbing = false;
            rb.useGravity = true; // Disable gravity to allow climbing
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.8f);
    }
}
