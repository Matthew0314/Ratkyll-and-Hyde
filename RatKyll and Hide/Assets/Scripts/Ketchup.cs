using UnityEngine;

public class Ketchup : MonoBehaviour, IPickUpItem
{

    public GameObject ketchupProjectilePrefab;
    public GameObject GameObject => gameObject;
    public PlayerController LastPlayer { get; set; }
    public Transform SpawnPoint { get; set; }
    public int Points => points;
    private int points = -1;
    public void PickUpItem() {
        Debug.Log($"Item {gameObject.name} picked up");
        // isPickedUp = true;
        
        // Disable physics interactions
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
    }

    public void UseItem() {
        if (ketchupProjectilePrefab != null) {
            // Instantiate the projectile at this object's position and rotation
            GameObject projectile = Instantiate(ketchupProjectilePrefab, transform.position + transform.forward * 0.5f, transform.rotation);

            // Add force to the projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null) {
                float shootForce = 500f; // Adjust as needed
                rb.AddForce(transform.forward * shootForce);
            }

            Debug.Log($"{gameObject.name} used: Shot ketchup!");
        } else {
            Debug.LogWarning("Ketchup projectile prefab is not assigned!");
        }
    }

    public void DestroyItem() {

        // foodSpawnManager.NotifyFoodDestroyed(SpawnPoint);
        Destroy(this.gameObject);
    }

}
