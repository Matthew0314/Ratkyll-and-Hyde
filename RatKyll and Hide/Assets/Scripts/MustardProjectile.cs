using UnityEngine;

public class MustardProjectile : MonoBehaviour
{
    [SerializeField] public GameObject splatterPrefab;
    private GameObject mustardBottle;
    
    void Update() {
        // if (IsGrounded()) Destroy(this.gameObject);
    }
    
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Surface"))
        {
            // Create a position 5 units higher on the Y axis
            Vector3 spawnPosition = new Vector3(
                transform.position.x, 
                transform.position.y + 7f, 
                transform.position.z
            );
            
            // Instantiate the splatter at the elevated position
            GameObject newSplat = Instantiate(splatterPrefab, spawnPosition, Quaternion.identity);
            NewSplatter(newSplat);
            Destroy(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.5f);

    public void AddOriginBottle(GameObject must) {
        mustardBottle = must;
    }

    public void NewSplatter(GameObject newSplat) {
        mustardBottle.GetComponent<Mustard>().AddSplatter(newSplat);
    }
}