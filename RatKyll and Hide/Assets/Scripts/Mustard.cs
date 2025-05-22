using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mustard : MonoBehaviour, IPickUpItem
{
    public GameObject mustardProjectilePrefab;
    [SerializeField] GameObject projectileSpawn;
    public GameObject GameObject => gameObject;
    public PlayerController LastPlayer { get; set; }
    public Transform SpawnPoint { get; set; }
    public int Points => points;
    private int points = -1;
    private AudioSource audioSource;
    private List<GameObject> splatters = new List<GameObject>();

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
        if (mustardProjectilePrefab != null) {
            GameObject projectile = Instantiate(mustardProjectilePrefab, projectileSpawn.transform.position, transform.rotation);
            projectile.GetComponent<MustardProjectile>().AddOriginBottle(this.gameObject);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null) {
                float shootForce = 2000f;
                rb.AddForce(transform.up * shootForce);
               
            }

            audioSource.Play();
            Debug.Log($"{gameObject.name} used: Shot ketchup!");
        } else {
            Debug.LogWarning("Mustard projectile prefab is not assigned!");
        }


    }

    public void DestroyItem() {

        // foodSpawnManager.NotifyFoodDestroyed(SpawnPoint);
        Destroy(this.gameObject);
    }

    public void AddSplatter(GameObject newSplat) {
        splatters.Add(newSplat);

        if (splatters.Count >= 4) {
            Destroy(splatters[0]);
            splatters.RemoveAt(0);
        }
    }


}
