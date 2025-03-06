using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f; // Speed of character
    [SerializeField] int playerNum; // 0 for Player1, 1 for Player2
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float jumpForce = 5f;

    private Vector2 movementInput;
    private PlayerInput playerInput;
    private Gamepad playerGamepad;
    private bool isClimbing = false;
    private Rigidbody rb;
    private Vector3 climbNormal;
    private Transform cameraTransform;

    void Start() {
        
        rb = GetComponent<Rigidbody>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        playerInput = GetComponent<PlayerInput>();
        // playerInput.SwitchCurrentControlScheme(Gamepad.all[playerNum]);
        // playerGamepad = Gamepad.all[playerNum];
        // Debug.Log(Gamepad.all[0]);
        // Debug.Log(Gamepad.all[1]);
        // Debug.Log(Gamepad.all[2]);
        if (playerNum == 0) {
        // Player 1 always uses the first available gamepad
            if (Gamepad.all.Count > 1) {
                playerInput.SwitchCurrentControlScheme(Gamepad.all[0]);
                playerGamepad = Gamepad.all[0];
            } else {
                Debug.LogWarning("No gamepad found! Player 1 will use keyboard instead.");
                // playerInput.SwitchCurrentControlScheme("KeyboardWASD");
            }
        } 
        else if (playerNum == 1) {
            // Player 2 uses the second gamepad if available, otherwise uses keyboard
            if (Gamepad.all.Count > 2) {
                playerInput.SwitchCurrentControlScheme(Gamepad.all[1]);
                playerGamepad = Gamepad.all[1];
            } else {
                Debug.Log("No second gamepad detected! Player 2 will use keyboard.");
                // playerInput.SwitchCurrentControlScheme("KeyboardWASD");
            }
        }
    }

    void Update()
    {
        if (isClimbing) {
            ClimbMovement();
        } else {
            MoveCharacter();
        }

        if (IsGrounded() && movementInput.y <= 0.5f) {
            isClimbing = false;
            rb.useGravity = true;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context) {
        if (context.performed && IsGrounded()) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    private void ClimbMovement() {
        Vector3 climbDirection = Vector3.Cross(Vector3.up, climbNormal).normalized; 
        Vector3 move = (climbDirection * (-1 * movementInput.x) + Vector3.up * movementInput.y) * climbSpeed;
        rb.linearVelocity = new Vector3(move.x, move.y, move.z);
    }

    private void MoveCharacter()
    {
        if (movementInput.sqrMagnitude > 0.01f) {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // Get movement direction
            Vector3 moveDirection = (camForward * movementInput.y + camRight * movementInput.x).normalized;

            // Rotate player to face movement direction
            Transform cameraChild = transform.Find("Camera");
            if (cameraChild != null) cameraChild.SetParent(null); // Temporarily detach camera

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

            if (cameraChild != null) cameraChild.SetParent(transform); // Reattach camera

            // Move the player
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            isClimbing = true;
            rb.useGravity = false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f))
            {
                climbNormal = hit.normal;
                Quaternion targetRotation = Quaternion.LookRotation(-climbNormal, Vector3.up);
                transform.rotation = targetRotation;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            isClimbing = false;
            rb.useGravity = true;
        }
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.8f);
}
