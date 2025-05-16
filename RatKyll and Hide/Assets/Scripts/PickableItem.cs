// using UnityEngine;

// public class PickableItem : MonoBehaviour, IPickUpItem
// {
//     [SerializeField] private int points = 1; // Default points value
//     private bool isPickedUp = false;
//     private PlayerController currentPlayer;

//     // Interface Properties
//     public GameObject GameObject => gameObject;
//     public PlayerController LastPlayer { get; set; }
//     public int Points => points;
        

//     // Called when the item is picked up
//     public void PickUpItem()
//     {
//         Debug.Log($"Item {gameObject.name} picked up");
//         isPickedUp = true;
        
//         // Disable physics interactions
//         Rigidbody rb = GetComponent<Rigidbody>();
//         if (rb != null)
//         {
//             rb.isKinematic = true;
//         }

//         Collider collider = GetComponent<Collider>();
//         if (collider != null)
//         {
//             collider.enabled = false;
//         }
       
//     }

//     // Called when the item is used
//     public void UseItem()
//     {
//         Debug.Log($"Item {gameObject.name} used");
//         // Add specific item use logic here
        
//         // Optionally destroy or deactivate the item after use
//         Destroy(gameObject);
//     }

//     // Optional: Ensure required components are present
//     private void Start()
//     {
//         // Add Rigidbody if not present
//         if (GetComponent<Rigidbody>() == null)
//         {
//             Rigidbody rb = gameObject.AddComponent<Rigidbody>();
//             rb.useGravity = true;
//             rb.isKinematic = false;
//         }

//         // Add Collider if not present
//         if (GetComponent<Collider>() == null)
//         {
//             gameObject.AddComponent<BoxCollider>();
//         }

//         // Ensure the object is tagged as Pickable
//         gameObject.tag = "Pickable";

//     }
// }

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

    

    // Optional: Ensure required components are present
    private void Start()
    {
        // Add Rigidbody if not present
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // Add Collider if not present
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        // Ensure the object is tagged as Pickable
        gameObject.tag = "Pickable";
        foodSpawnManager = GameObject.Find("GameManager").GetComponent<FoodSpawnManager>();

        if (points != -1) points = Random.Range(minPoints, maxPoints + 1);


    }
        
    // Called when the item is picked up
    public void PickUpItem()
    {
        Debug.Log($"Item {gameObject.name} picked up");
        isPickedUp = true;
        
        // Disable physics interactions
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

    // Called when the item is used
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