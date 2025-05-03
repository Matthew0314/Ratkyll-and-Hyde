using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float minDistance = 1f; // Minimum camera distance from player
    [SerializeField] private float maxDistance = 5f;   // Maximum allowed distance
    [SerializeField] private LayerMask obstacleMask;
    private Transform playerBody;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float distanceFromPlayer;
    private float currentDistance;

    void Start() {
        playerBody = transform.parent;

        if (playerBody == null) {
            Debug.LogError("Camera must be a child of the player!");
            return;
        }

        distanceFromPlayer = Vector3.Distance(transform.position, playerBody.position);

        obstacleMask = LayerMask.GetMask("Default");
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

        // AdjustCameraDistance();
    }

    private void AdjustCameraDistance()
    {
        Vector3 desiredPosition = playerBody.position - transform.forward * maxDistance;
        RaycastHit hit;

        if (Physics.Raycast(playerBody.position, -transform.forward, out hit, maxDistance, obstacleMask)) {
            // If obstacle detected, set camera closer
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else {
            // No obstacle, use max distance
            currentDistance = maxDistance;
        }

        transform.position = playerBody.position + new Vector3(0, 0.2f, 0) - transform.forward * currentDistance;
    }


    public void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();
}
