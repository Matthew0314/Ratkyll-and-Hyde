using UnityEngine;

public class MustardProjectile : MonoBehaviour
{
    [SerializeField] public GameObject splatterPrefab;
    
    void Update() {
        if (IsGrounded()) Destroy(this.gameObject);
    }
    
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Surface"))
        {
            // Create a position 5 units higher on the Y axis
            Vector3 spawnPosition = new Vector3(
                transform.position.x, 
                transform.position.y + 2f, 
                transform.position.z
            );
            
            // Instantiate the splatter at the elevated position
            Instantiate(splatterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.5f);
}