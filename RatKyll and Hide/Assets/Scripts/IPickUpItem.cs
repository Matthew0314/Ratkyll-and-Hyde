using UnityEngine;

public interface IPickUpItem
{
    
    void PickUpItem();
    void UseItem();
    GameObject GameObject { get; }
    PlayerController LastPlayer { get; set; }
    
    
}
