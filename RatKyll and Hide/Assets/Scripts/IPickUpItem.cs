using UnityEngine;

public interface IPickUpItem
{
    
    void PickUpItem();
    void UseItem();
    void DestroyItem();
    GameObject GameObject { get; }
    PlayerController LastPlayer { get; set; }
    Transform SpawnPoint { get; set; }


    int Points { get; }
    
    
}
