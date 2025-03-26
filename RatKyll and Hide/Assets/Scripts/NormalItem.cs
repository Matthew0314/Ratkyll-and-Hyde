using UnityEngine;

public class NormalItem : MonoBehaviour, IPickUpItem
{
    private PlayerController lastPlayer;
    public void PickUpItem() {}
    public void UseItem() {}

    public GameObject GameObject => gameObject; 

    public PlayerController LastPlayer {
        get => lastPlayer;
        set => lastPlayer = value;
    }
    
}
