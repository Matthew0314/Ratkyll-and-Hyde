// using UnityEngine;
// using System.Collections;

// public class SplatterStun : MonoBehaviour
// {
//     [SerializeField] private float slowFactor = 0.5f; // How much to slow the player (0.5 = 50% speed)
//     [SerializeField] private float slowDuration = 4f; // Duration of the slow effect in seconds

//     void Update() {
//         if (IsGrounded()) Destroy(this.gameObject);
//     }

//     private void OnCollisionEnter(Collision collision) {
//         if (collision.gameObject.CompareTag("Player")) {
//             // Get the player's movement script
//             PlayerController playerMovement = collision.gameObject.GetComponent<PlayerController>();
            
//             // If the player has a movement script, apply the slow effect
//             if (playerMovement != null) {
//                 StartCoroutine(SlowPlayer(playerMovement));
//             }
//         }
//     }

//     private IEnumerator SlowPlayer(PlayerController playerMovement) {
//         // Store the original movement speed
//         float originalSpeed = playerMovement.walkSpeed;
        
//         // Apply slow effect
//         playerMovement.moveSpeed *= slowFactor;
        
//         // Wait for the specified duration
//         yield return new WaitForSeconds(slowDuration);
        
//         // Restore original speed
//         playerMovement.moveSpeed = originalSpeed;
//     }

//     private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.5f);
// }