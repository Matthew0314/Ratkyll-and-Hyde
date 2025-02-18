using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 5;
    [SerializeField] int playerNum; //Either 0 or 1
    private Vector2 movementInput;
    private CharacterController controller;
    private PlayerInput playerInput;
    private Gamepad playerGamepad;

    void Start() {
      
        Debug.Log("Number of Gamepads: " + Gamepad.all.Count + " Using " + Gamepad.all[playerNum]);
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(Gamepad.all[playerNum]);
        // controller = gameObject.GetComponent<CharacterController>();
        // playerGamepad = Gamepad.all[playerNum];  // Assign Gamepad 1 to Player 1
        // playerInput.SwitchCurrentControlScheme(playerGamepad);

        // // Pair the player with the correct controller
        // playerInput.devices = new InputDevice[] { playerGamepad };
        
    }
    void Update()
    {
        Debug.Log(gameObject.name + " Input: " + movementInput);
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime);
        // Vector3 move = new Vector3(movementInput.x, 0, movementInput.x);
        // controller.Move(move * Time.deltaTime * speed);
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
}
