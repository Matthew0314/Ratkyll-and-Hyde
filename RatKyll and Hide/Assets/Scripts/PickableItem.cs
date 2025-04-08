using UnityEngine;

public class PickableItem : MonoBehaviour, IPickUpItem
{
    [SerializeField] private int points = 1; // Default points value
    private bool isPickedUp = false;
    private PlayerController currentPlayer;

    // Interface Properties
    public GameObject GameObject => gameObject;
    public PlayerController LastPlayer { get; set; }
    public int Points => points;

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
    }

    // Called when the item is used
    public void UseItem()
    {
        Debug.Log($"Item {gameObject.name} used");
        // Add specific item use logic here
        
        // Optionally destroy or deactivate the item after use
        Destroy(gameObject);
    }

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
    }
}