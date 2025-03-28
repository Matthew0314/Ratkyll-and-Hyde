using UnityEngine;

public class KetchupBottlerController : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate = 0.5f;
    public float range = 100f;
    public GameObject ketchupProjectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    private float nextFireTime = 0f;

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

    void Shoot()
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
}