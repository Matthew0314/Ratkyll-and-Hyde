using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour //MonoBehaviour
{

    [SerializeField] float speed = 5; // Speed of character
    [SerializeField] int playerNum; // Either 0 for Player1 or 1 for Player2, set in the inspector
    private Vector2 movementInput;
    private PlayerInput playerInput;
    private Gamepad playerGamepad;

    void Start() {
      
        //Assigns a controller to the player
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(Gamepad.all[playerNum]);
        playerGamepad = Gamepad.all[playerNum];
    }
    void Update()
    {
        if (!IsOwner) return;
        // Moves the Player
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime);
    }

    // Moves the character using Unity's Input System
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public Gamepad GetPlayerGamepad() { return playerGamepad; }
}
