using UnityEngine;

public interface IPickUpItem
{
    
    void PickUpItem();
    void UseItem();
    void DestroyItem();
    GameObject GameObject { get; }
    PlayerController LastPlayer { get; set; }
<<<<<<< HEAD
=======
    Transform SpawnPoint { get; set; }


    int Points { get; }
>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
    
    
}
