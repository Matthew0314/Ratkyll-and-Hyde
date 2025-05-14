
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float walkSpeed = 35f; // Speed of character6
    private float sprintSpeed = 60f;
    [SerializeField] int playerNum; // 0 for Player1, 1 for Player2
    [SerializeField] float climbSpeed = 3f; // Speed which player climbs the wall
    [SerializeField] float jumpForce = 5f; // Force that is spplied when jumping
    [SerializeField] float pickupRange = 3f; 

    private Vector2 movementInput; // Used for movement
    private PlayerInput playerInput; // Input system
    // private Gamepad playerGamepad; // Stores the Gamepad that is being used
    private bool isClimbing = false;
    private Rigidbody rb;
    private Vector3 climbNormal;
    private Transform cameraTransform;

    private IPickUpItem _heldItem;

    // Used for throwing the item
    private float minThrowForce = 5f;
    private float maxThrowForce = 25f;
    public float chargeTime = 2f;
    private float currentForce;
    private bool isCharging = false;
    private float chargeStartTime;
    private IGameManager _gameManager;
    private bool isSprinting;
    private AudioSource audioSource;
    private bool canMove = true;
    private bool invinsible = false;


    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip swipeClip;

    void Start() {
        // Used later
        rb = GetComponent<Rigidbody>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        playerInput = GetComponent<PlayerInput>();
        _gameManager = GameObject.Find("GameManager").GetComponent<IGameManager>();
        audioSource = GetComponent<AudioSource>();


        // Debug.LogError(Gamepad.all.Count);

        // Assigns controllers based on player number
        if (playerNum == 0) {
        // Player 1 always uses the first available gamepad
            if (Gamepad.all.Count > 0) {
                playerInput.SwitchCurrentControlScheme(Gamepad.all[0]);
                // playerGamepad = Gamepad.all[0];
            } else {
                Debug.LogWarning("No gamepad found! Player 1 will use keyboard instead.");
                // playerInput.SwitchCurrentControlScheme("KeyboardWASD");
            }
        } 
        else if (playerNum == 1) {
            // Player 2 uses the second gamepad if available, otherwise uses keyboard
            if (Gamepad.all.Count > 1) {
                playerInput.SwitchCurrentControlScheme(Gamepad.all[1]);
                // playerGamepad = Gamepad.all[1];
            } else {
                Debug.Log("No second gamepad detected! Player 2 will use keyboard.");
                // playerInput.SwitchCurrentControlScheme("KeyboardWASD");
            }
        }
    }

    void Update() {
        
        if (_gameManager.GameOver) return;

        // Ensure "Sprint" is bound in your Input Actions
        if (playerInput.actions["Sprint"].IsPressed() && _heldItem == null) isSprinting = true;
        else isSprinting = false;

        // Either climbs or moves the character
        if (isClimbing) {
            ClimbMovement();
        } else {
            MoveCharacter();
        }

        // I forgot what this does tbh
        if (IsGrounded() && movementInput.y <= 0.5f) {
            isClimbing = false;
            rb.useGravity = true;
        }
        if (rb.linearVelocity.y < 0) {
            rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        }

        if (playerInput.actions["Jump"].WasPressedThisFrame()) {
            HandleJump();
        }
        // if (!IsGrounded()) {
        //     rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        // }

        if (playerInput.actions["UseItem"].WasPressedThisFrame() && _heldItem != null) _heldItem.UseItem();

        // Throws item
        if (playerInput.actions["PickUp"].WasPressedThisFrame() && _heldItem != null) StartCharging();
        if (isCharging) ChargeThrow();
        if (playerInput.actions["PickUp"].WasReleasedThisFrame() && _heldItem != null && isCharging) ThrowObject();

        // Attempts to pickup an item
        if (((playerInput.actions["PickUp"].triggered)||Input.GetKeyDown(KeyCode.E)) && _heldItem == null) TryPickupItem();
    }


    // Begins charging 
    void StartCharging() {
        isCharging = true;
        chargeStartTime = Time.time;
        currentForce = minThrowForce;
    }

    void ChargeThrow() {
        float chargeDuration = Time.time - chargeStartTime;
        currentForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargeDuration / chargeTime);
        Debug.Log("CHARGING");
    }

    // Throws the object
    // Throwing mechanic is broken
    void ThrowObject() {
        isCharging = false;
        _heldItem.GameObject.transform.SetParent(null); 
        

        Rigidbody rbObj = _heldItem.GameObject.GetComponent<Rigidbody>();

        rbObj.isKinematic = false;
        _heldItem.GameObject.GetComponent<Collider>().enabled = true;
        rbObj.AddForce(transform.forward * currentForce, ForceMode.Impulse);
        Debug.Log("Applying force: " + currentForce);
        _heldItem = null;
    }

    // Code for moving while climbing
    private void ClimbMovement() {
        Vector3 climbDirection = Vector3.Cross(Vector3.up, climbNormal).normalized; 
        Vector3 move = (climbDirection * (-1 * movementInput.x) + Vector3.up * movementInput.y) * climbSpeed;
        rb.linearVelocity = new Vector3(move.x, move.y, move.z);
    }

    private void MoveCharacter() {
        if (movementInput.sqrMagnitude > 0.01f) {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * movementInput.y + camRight * movementInput.x);
            moveDirection.y = 0f;
            moveDirection.Normalize();

            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Transform cameraChild = transform.Find("Camera");
            if (cameraChild != null) cameraChild.SetParent(null);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

            if (cameraChild != null) cameraChild.SetParent(transform);

            rb.AddForce(moveDirection * currentSpeed, ForceMode.Acceleration);
        }
    }

    


    private void TryPickupItem() {

        // If player already has an item, return back to update.
        if (_heldItem != null) return;

        // Get all colliders within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRange);

        // For every collider in range, check if 
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.CompareTag("Pickable")) {
                PickUpItem(hitCollider.gameObject);
                break; 
            }
        }
    }

    // Picks up first item in array
    // private void PickUpItem(GameObject item) {
    //     IPickUpItem usableItem = item.GetComponent<IPickUpItem>();
    //     if (usableItem != null) {
    //         _heldItem = usableItem; // Store the item
    //         usableItem.LastPlayer = this; // Stores this script in the LastPlayer variable
    //         item.transform.SetParent(transform); // Attach the item to the player
    //         item.transform.localPosition = new Vector3(0, 1, 1); // Adjust the position to appear in the player's hand
    //         item.GetComponent<Collider>().enabled = false; // Disable the collider to prevent it from interacting with the world
    //         item.GetComponent<Rigidbody>().isKinematic = true; // Make the item stop interacting with physics
    //     }
    // }
    private void PickUpItem(GameObject item) {
        IPickUpItem usableItem = item.GetComponent<IPickUpItem>();
        if (usableItem != null) {
            _heldItem = usableItem; // Store the item
            usableItem.LastPlayer = this; // Stores this script in the LastPlayer variable
            // usableItem.PickUpItem(); // Call the PickUpItem method from the interface
            
            item.transform.SetParent(transform); // Attach the item to the player
            item.transform.localPosition = new Vector3(0, 1, 1); // Adjust the position to appear in the player's hand
            if (!audioSource.isPlaying) {
                audioSource.clip = swipeClip;
                audioSource.Play();
            }
            _heldItem.PickUpItem();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Climbable")) {
            isClimbing = true;
            rb.useGravity = false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f)) {
                climbNormal = hit.normal;
                Quaternion targetRotation = Quaternion.LookRotation(-climbNormal, Vector3.up);
                transform.rotation = targetRotation;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Climbable")) {
            isClimbing = false;
            rb.useGravity = true;
        }
    }

    // Checks if the player is on the ground using a raycast
    private bool IsGrounded() { 
        // Debug.LogError("HIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII");
        return Physics.Raycast(transform.position, Vector3.down, 0.8f);

    }

    public void StunPlayer() {
        if (!invinsible) {
            canMove = false;
            invinsible = true;
            Invoke("RemoveStun", 7f);
        }
    }

    private void RemoveStun() {
        canMove = true;
        Invoke("RemoveInvinsibility", 3f);

    }

    private void RemoveInvinsibility() => invinsible = false;

    private void HandleJump() {
        if (isClimbing) {
            isClimbing = false;
            rb.useGravity = true;
            Vector3 forceDirection = -transform.forward; // Opposite direction
            rb.AddForce(forceDirection * jumpForce, ForceMode.Impulse);
            if (!audioSource.isPlaying) {
                audioSource.clip = jumpClip;
                audioSource.Play();
            }
        } else if (IsGrounded()) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            if (!audioSource.isPlaying) {
                audioSource.clip = jumpClip;
                audioSource.Play();
            }
        }
    }

    // Gets the player number that is using in the PotGameManager Script
    public int GetPlayerNum() => playerNum;

    // Used for movement
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    // Used for jumping
    // public void OnJump(InputAction.CallbackContext context) {
    //     if (isClimbing && context.performed) {
    //         isClimbing = false;
    //         rb.useGravity = true;
    //         Vector3 forceDirection = -transform.forward; // Opposite direction
    //         rb.AddForce(forceDirection * jumpForce, ForceMode.Impulse);
    //         if (!audioSource.isPlaying) {
    //             audioSource.clip = jumpClip;
    //             audioSource.Play();
    //         }
    //     } else if (context.performed && IsGrounded()) {
    //         Debug.LogError("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
    //         rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    //         if (!audioSource.isPlaying) {
    //             audioSource.clip = jumpClip;
    //             audioSource.Play();
    //         }
    //     }

        
    // }
}
