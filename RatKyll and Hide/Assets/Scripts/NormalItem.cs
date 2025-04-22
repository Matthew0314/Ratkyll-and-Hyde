// using UnityEngine;

// public class NormalItem : MonoBehaviour, IPickUpItem
// {
//     public void PickUpItem() {}
//     public void UseItem() {}

//     public GameObject GameObject => gameObject; 
    
// }
using UnityEngine;

public class NormalItem : MonoBehaviour, IPickUpItem
{

    private PlayerController lastPlayer; // Stores the last player

    [SerializeField] int points; // Stores how many points the food is worth, -1 if it can't be consumed
    public void PickUpItem() {}
    public void UseItem() {}
    public void DestroyItem() {}

    public GameObject GameObject => gameObject; // Returns the gameObject

    // Returns and sets the Last player
    public PlayerController LastPlayer {
        get => lastPlayer;
        set => lastPlayer = value;
    }

    public Transform SpawnPoint { get; set; }

    public int Points => points; // Returns the points
    
}

