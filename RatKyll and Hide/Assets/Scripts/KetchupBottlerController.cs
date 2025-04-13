// using UnityEngine;

// public class KetchupBottlerController : MonoBehaviour, IPickUpItem
// {
//     [SerializeField] private int points = 1; // Default points value
//     public float damage = 10f;
//     public float fireRate = 0.5f;
//     public float range = 100f;
//     public GameObject ketchupProjectilePrefab;
//     public Transform firePoint;
//     public float projectileSpeed = 20f;
//     private float nextFireTime = 0f;
//     private Transform playerForwardDirection;

//     public GameObject GameObject => gameObject;
//     public PlayerController LastPlayer { get; set; }
//     public int Points => points;

//     void Update()
//     {
//         if ((Input.GetButton("Fire1") || Input.GetMouseButton(0)) && CanShoot())
//         {
//             UseItem();
//         }
//     }

//     bool CanShoot()
//     {
//         return Time.time >= nextFireTime;
//     }

//     public void UseItem()
//     {
//         nextFireTime = Time.time + fireRate;

//         if (ketchupProjectilePrefab != null && firePoint != null)
//         {
//             GameObject projectile = Instantiate(ketchupProjectilePrefab, firePoint.position, firePoint.rotation);
//             Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            
//             if (projectileRb != null)
//             {
//                 projectileRb.linearVelocity = firePoint.forward * projectileSpeed;
//             }

//             Destroy(projectile, 5f);
//         }

//         RaycastHit hit;
//         if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
//         {
          
//         }
//     }

//     // public void PickUpItem()
//     // {
//     //     playerForwardDirection = LastPlayer.transform;
//     //     Vector3 playerForward = playerForwardDirection.forward;
//     //     transform.Rotate(90f, 0f, playerForward.z);
//     // }
//     public void PickUpItem()
//     {
//         // Store the player's transform reference
//         playerForwardDirection = LastPlayer.transform;
        
//         // Reset the rotation first
//         transform.rotation = Quaternion.identity;
        
//         // Rotate the bottle to hold it properly (90 degrees around X axis)
//         transform.Rotate(90f, 0f, 0f);
        
//         // Then align with the player's forward direction
//         transform.forward = playerForwardDirection.forward;
//     }
// }

using UnityEngine;

// Modified to no longer implement IPickUpItem
public class KetchupBottlerController : MonoBehaviour
{
    [SerializeField] private int points = 1;
    public float damage = 10f;
    public float fireRate = 0.5f;
    public float range = 100f;
    public GameObject ketchupProjectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    private float nextFireTime = 0f;
    private Transform playerTransform;
    
    private PickableItem pickableItem;
    
    void Awake()
    {
        // Get reference to the PickableItem component
        pickableItem = GetComponent<PickableItem>();
        
        // Subscribe to the PickableItem's events or use a reference to it
        if (pickableItem != null)
        {
            // You could subscribe to events here if needed
        }
    }
    
    void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetMouseButton(0)) && CanShoot())
        {
            Shoot();
        }
    }
    
    bool CanShoot()
    {
        return Time.time >= nextFireTime;
    }
    
    public void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        
        if (ketchupProjectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(ketchupProjectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            
            if (projectileRb != null)
            {
                projectileRb.linearVelocity = firePoint.forward * projectileSpeed;
            }
            
            Destroy(projectile, 5f);
        }
        
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            // Handle hit logic
        }
    }
    
    // Called by PickableItem when picked up
    public void OnItemPickedUp(PlayerController player)
    {
        playerTransform = player.transform;
        
        // Reset the rotation first
        transform.rotation = Quaternion.identity;
        
        // Rotate the bottle to hold it properly (90 degrees around X axis)
        transform.Rotate(90f, 0f, 0f);
        
        // Then align with the player's forward direction
        transform.forward = playerTransform.forward;
        
        Debug.Log("Ketchup bottle rotated to align with player");
    }
}