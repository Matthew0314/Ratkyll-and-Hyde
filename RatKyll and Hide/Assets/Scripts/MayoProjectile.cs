using UnityEngine;

public class MayoProjectile : MonoBehaviour
{
    void Update() {
        if (IsGrounded()) Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.EnableMayoSplatter();
        }
    }


    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.5f);
}
