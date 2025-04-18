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
<<<<<<< HEAD
    private PlayerController lastPlayer;
=======

    private PlayerController lastPlayer; // Stores the last player

    [SerializeField] int points; // Stores how many points the food is worth, -1 if it can't be consumed
>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
    public void PickUpItem() {}
    public void UseItem() {}
    public void DestroyItem() {}

<<<<<<< HEAD
    public GameObject GameObject => gameObject; 

=======
    public GameObject GameObject => gameObject; // Returns the gameObject

    // Returns and sets the Last player
>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
    public PlayerController LastPlayer {
        get => lastPlayer;
        set => lastPlayer = value;
    }
<<<<<<< HEAD
=======

    public Transform SpawnPoint { get; set; }

    public int Points => points; // Returns the points
>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
    
}

