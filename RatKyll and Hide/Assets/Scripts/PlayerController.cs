// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerController : MonoBehaviour
// {
//     [SerializeField] float speed = 5f; // Speed of character6
//     [SerializeField] int playerNum; // 0 for Player1, 1 for Player2
//     [SerializeField] float climbSpeed = 3f; // Speed which player climbs the wall
//     [SerializeField] float jumpForce = 5f; // Force that is spplied when jumping
//     [SerializeField] float pickupRange = 3f; 

//     private Vector2 movementInput; // Used for movement
//     private PlayerInput playerInput; // Input system
//     // private Gamepad playerGamepad; // Stores the Gamepad that is being used
//     private bool isClimbing = false;
//     private Rigidbody rb;
//     private Vector3 climbNormal;
//     private Transform cameraTransform;
//     // private GameObject currItem;
//     private IPickUpItem _heldItem;

//     private float minThrowForce = 5f;
//     private float maxThrowForce = 25f;
//     public float chargeTime = 2f;

//     private float currentForce;
//     private bool isCharging = false;
//     private float chargeStartTime;

//     void Start() {
        
//         rb = GetComponent<Rigidbody>();
//         cameraTransform = GetComponentInChildren<Camera>().transform;
//         playerInput = GetComponent<PlayerInput>();

//         // Assigns controllers based on player number
//         if (playerNum == 0) {
//         // Player 1 always uses the first available gamepad
//             if (Gamepad.all.Count > 0) {
//                 playerInput.SwitchCurrentControlScheme(Gamepad.all[0]);
//                 // playerGamepad = Gamepad.all[0];
//             } else {
//                 Debug.LogWarning("No gamepad found! Player 1 will use keyboard instead.");
//                 // playerInput.SwitchCurrentControlScheme("KeyboardWASD");
//             }
//         } 
//         else if (playerNum == 1) {
//             // Player 2 uses the second gamepad if available, otherwise uses keyboard
//             if (Gamepad.all.Count > 1) {
//                 playerInput.SwitchCurrentControlScheme(Gamepad.all[1]);
//                 // playerGamepad = Gamepad.all[1];
//             } else {
//                 Debug.Log("No second gamepad detected! Player 2 will use keyboard.");
//                 // playerInput.SwitchCurrentControlScheme("KeyboardWASD");
//             }
//         }
//     }

//     void Update() {

//         if (isClimbing) {
//             ClimbMovement();
//         } else {
//             MoveCharacter();
//         }

//         if (IsGrounded() && movementInput.y <= 0.5f) {
//             isClimbing = false;
//             rb.useGravity = true;
//         }

        

//         // Throws item
//         if (playerInput.actions["PickUp"].WasPressedThisFrame() && _heldItem != null) StartCharging();
//         if (isCharging) ChargeThrow();
//         if (playerInput.actions["PickUp"].WasReleasedThisFrame() && _heldItem != null && isCharging) ThrowObject();

//         // Attempts to pickup an item
//         if (playerInput.actions["PickUp"].triggered && _heldItem == null) TryPickupItem();
//     }

//     void StartCharging() {
//         isCharging = true;
//         chargeStartTime = Time.time;
//         currentForce = minThrowForce;
//     }

//     void ChargeThrow() {
//         float chargeDuration = Time.time - chargeStartTime;
//         currentForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargeDuration / chargeTime);
//         Debug.Log("CHARGING");
//     }

//     void ThrowObject() {
//         isCharging = false;
//         _heldItem.GameObject.transform.SetParent(null); 
        

//         Rigidbody rbObj = _heldItem.GameObject.GetComponent<Rigidbody>();

//         rbObj.isKinematic = false;
//         _heldItem.GameObject.GetComponent<Collider>().enabled = true;
//         rbObj.AddForce(Camera.main.transform.forward * currentForce, ForceMode.Impulse);
//         Debug.Log("Applying force: " + currentForce);
//         _heldItem = null;
//     }

//     private void ClimbMovement() {
//         Vector3 climbDirection = Vector3.Cross(Vector3.up, climbNormal).normalized; 
//         Vector3 move = (climbDirection * (-1 * movementInput.x) + Vector3.up * movementInput.y) * climbSpeed;
//         rb.linearVelocity = new Vector3(move.x, move.y, move.z);
//     }

//     private void MoveCharacter() {
//         if (movementInput.sqrMagnitude > 0.01f) {
//             Vector3 camForward = cameraTransform.forward;
//             Vector3 camRight = cameraTransform.right;

//             camForward.y = 0;
//             camRight.y = 0;
//             camForward.Normalize();
//             camRight.Normalize();

//             // Get movement direction
//             Vector3 moveDirection = (camForward * movementInput.y + camRight * movementInput.x).normalized;

//             // Rotate player to face movement direction
//             Transform cameraChild = transform.Find("Camera");
//             if (cameraChild != null) cameraChild.SetParent(null); // Temporarily detach camera

//             Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
//             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

//             if (cameraChild != null) cameraChild.SetParent(transform); // Reattach camera

//             // Move the player
//             transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
//         }
//     }

//     private void TryPickupItem() {

//         // If player already has an item, return back to update.
//         if (_heldItem != null) return;

//         // Get all colliders within range
//         Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRange);

//         // For every collider in range, check if 
//         foreach (var hitCollider in hitColliders) {
//             if (hitCollider.CompareTag("Pickable")) {
//                 PickUpItem(hitCollider.gameObject);
//                 break; 
//             }
//         }
//     }

//     private void PickUpItem(GameObject item)
//     {
//         IPickUpItem usableItem = item.GetComponent<IPickUpItem>();
//         if (usableItem != null)
//         {
//             _heldItem = usableItem; // Store the item
//             item.transform.SetParent(transform); // Attach the item to the player
//             item.transform.localPosition = new Vector3(0, 1, 1); // Adjust the position to appear in the player's hand
//             item.GetComponent<Collider>().enabled = false; // Disable the collider to prevent it from interacting with the world
//             item.GetComponent<Rigidbody>().isKinematic = true; // Make the item stop interacting with physics
//         }
//     }

//     private void ThrowItem() {

//     }

//     void OnTriggerEnter(Collider other) {
//         if (other.CompareTag("Climbable")) {
//             isClimbing = true;
//             rb.useGravity = false;

//             RaycastHit hit;
//             if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f)) {
//                 climbNormal = hit.normal;
//                 Quaternion targetRotation = Quaternion.LookRotation(-climbNormal, Vector3.up);
//                 transform.rotation = targetRotation;
//             }
//         }
//     }

//     void OnTriggerExit(Collider other) {
//         if (other.CompareTag("Climbable")) {
//             isClimbing = false;
//             rb.useGravity = true;
//         }
//     }

//     private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.8f);
//     // private GameObject GetCurrItem() => currItem;
//     // private void RemoveCurrItem() => currItem = null;
//     // public void SetCurrItem(GameObject item) {
//     //     if (_heldItem == null) _heldItem = item;
//     // }
    
//     public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

//     public void OnJump(InputAction.CallbackContext context) {
//         if (isClimbing && context.performed) {
//             isClimbing = false;
//             rb.useGravity = true;
//             Vector3 forceDirection = -transform.forward; // Opposite direction
//             rb.AddForce(forceDirection * jumpForce, ForceMode.Impulse);
//         } else if (context.performed && IsGrounded()) {
//             rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
//         }
//     }
// }
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f; // Speed of character6
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

    void Start() {
        // Used later
        rb = GetComponent<Rigidbody>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        playerInput = GetComponent<PlayerInput>();
        _gameManager = GameObject.Find("GameManager").GetComponent<IGameManager>();

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
        rbObj.AddForce(Camera.main.transform.forward * currentForce, ForceMode.Impulse);
        Debug.Log("Applying force: " + currentForce);
        _heldItem = null;
    }

    // Code for moving while climbing
    private void ClimbMovement() {
        Vector3 climbDirection = Vector3.Cross(Vector3.up, climbNormal).normalized; 
        Vector3 move = (climbDirection * (-1 * movementInput.x) + Vector3.up * movementInput.y) * climbSpeed;
        rb.linearVelocity = new Vector3(move.x, move.y, move.z);
    }

    // Code for moving character
    private void MoveCharacter() {
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
        usableItem.PickUpItem(); // Call the PickUpItem method from the interface
        
        item.transform.SetParent(transform); // Attach the item to the player
        item.transform.localPosition = new Vector3(0, 1, 1); // Adjust the position to appear in the player's hand
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
    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.8f);

    // Gets the player number that is using in the PotGameManager Script
    public int GetPlayerNum() => playerNum;

    // Used for movement
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    // Used for jumping
    public void OnJump(InputAction.CallbackContext context) {
        if (isClimbing && context.performed) {
            isClimbing = false;
            rb.useGravity = true;
            Vector3 forceDirection = -transform.forward; // Opposite direction
            rb.AddForce(forceDirection * jumpForce, ForceMode.Impulse);
        } else if (context.performed && IsGrounded()) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }
}
