using UnityEngine;

public class Ketchup : MonoBehaviour, IPickUpItem
{

    public GameObject ketchupProjectilePrefab;
    [SerializeField] GameObject projectileSpawn;
    public GameObject GameObject => gameObject;
    public PlayerController LastPlayer { get; set; }
    public Transform SpawnPoint { get; set; }
    public int Points => points;
    private int points = -1;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PickUpItem()
    {
        Debug.Log($"Item {gameObject.name} picked up");
        // isPickedUp = true;

        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Transform parent = transform.parent;
        Transform ketchupPoint = transform.parent.Find("KetchupPoint");
        if (ketchupPoint != null)
        {
            transform.localPosition = ketchupPoint.localPosition;
            transform.localRotation = Quaternion.Euler(
                90f,
                ketchupPoint.localEulerAngles.y,
                ketchupPoint.localEulerAngles.z
            );
        }

    }

    public void UseItem() {
        if (ketchupProjectilePrefab != null) {
            GameObject projectile = Instantiate(ketchupProjectilePrefab, projectileSpawn.transform.position, transform.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null) {
                float shootForce = 2000f; 
                rb.AddForce(transform.up * shootForce);
               
            }

            audioSource.Play();
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
