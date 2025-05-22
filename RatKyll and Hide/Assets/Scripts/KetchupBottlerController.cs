//This is an old file that is no longer used, it should be moved to the archive
using UnityEngine;


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
        
        pickableItem = GetComponent<PickableItem>();
        
        
        if (pickableItem != null)
        {
            
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
           
        }
    }
    
    public void OnItemPickedUp(PlayerController player)
    {
        playerTransform = player.transform;
        
        transform.rotation = Quaternion.identity;
        
        transform.Rotate(90f, 0f, 0f);
        transform.forward = playerTransform.forward;
        
        Debug.Log("Ketchup bottle rotated to align with player");
    }
}
