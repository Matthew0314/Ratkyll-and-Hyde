using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1f;
    private Transform playerBody;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float distanceFromPlayer;

    void Start() {
        playerBody = transform.parent;
        if (playerBody == null) {
            Debug.LogError("Camera must be a child of the player!");
            return;
        }
        distanceFromPlayer = Vector3.Distance(transform.position, playerBody.position);
    }

    void Update()
    {
        if (playerBody == null) return;

        float lookX = lookInput.x * sensitivity;
        float lookY = lookInput.y * sensitivity;

        // Rotate around the player horizontally
        transform.RotateAround(playerBody.position, Vector3.up, lookX);

        // Apply vertical rotation separately
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f); // Adjust for better angles

        // Rotate camera up/down without affecting orbiting
        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, 0);

        // Maintain fixed distance from player
        transform.position = playerBody.position - transform.forward * distanceFromPlayer;
    }

    public void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();
}
