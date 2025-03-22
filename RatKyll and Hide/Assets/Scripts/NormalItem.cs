using UnityEngine;

public class NormalItem : MonoBehaviour, IPickUpItem
{
    public void PickUpItem() {}
    public void UseItem() {}

    public GameObject GameObject => gameObject; 
    
}
