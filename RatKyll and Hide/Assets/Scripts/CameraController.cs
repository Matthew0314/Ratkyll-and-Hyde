using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private float sensitivity = 0.8f;
    private Transform playerBody; // Assign your player object for rotation.

    private Vector2 lookInput;
    private float xRotation = 0f;
    private PlayerInput playerInput;



    void Start() {
        playerBody = transform.parent;
        // playerInput = GetComponent<PlayerInput>();
        // playerInput.SwitchCurrentControlScheme(transform.parent?.gameObject.GetComponent<PlayerController>().GetPlayerGamepad());
    }
    void Update()
    {
        float lookX = lookInput.x * sensitivity;
        float lookY = lookInput.y * sensitivity;

        // Rotate the camera vertically (clamping to prevent flipping)
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookX); // Rotate the player horizontally
        
    }

    public void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();
}
