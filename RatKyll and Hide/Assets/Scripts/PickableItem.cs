
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickUpItem
{
    [SerializeField] private int points = 1; // Default points value
    [SerializeField] private int minPoints = 1;
    [SerializeField] private int maxPoints = 1;
    private bool isPickedUp = false;
    private PlayerController currentPlayer;

    // Interface Properties
    public GameObject GameObject => gameObject;
    public PlayerController LastPlayer { get; set; }
    public Transform SpawnPoint { get; set; }
    public int Points => points;

    private FoodSpawnManager foodSpawnManager;

    

    private void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        gameObject.tag = "Pickable";
        foodSpawnManager = GameObject.Find("GameManager").GetComponent<FoodSpawnManager>();

        if (points != -1) points = Random.Range(minPoints, maxPoints + 1);


    }
        
    public void PickUpItem()
    {
        Debug.Log($"Item {gameObject.name} picked up");
        isPickedUp = true;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        
        // Notify the KetchupBottlerController if it exists
        // KetchupBottlerController bottler = GetComponent<KetchupBottlerController>();
        // if (bottler != null)
        // {
        //     bottler.OnItemPickedUp(LastPlayer);
        // }
    }

    public void DestroyItem() {
        foodSpawnManager.NotifyFoodDestroyed(SpawnPoint);
        Destroy(this.gameObject);
    }

    public void UseItem()
    {
        Debug.Log($"Item {gameObject.name} used");
        
        // Call the Shoot method on the KetchupBottlerController
        // KetchupBottlerController bottler = GetComponent<KetchupBottlerController>();
        // if (bottler != null)
        // {
        //     bottler.Shoot();
        //     return; // Don't destroy the item if it's a ketchup bottle
        // }
        
        // Only destroy if it's not a special item with its own controller
        // Destroy(gameObject);
    }

    
}
