using UnityEngine;

public class CookingPot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Food")) {
            if (collision.gameObject.GetComponent<IPickUpItem>().LastPlayer != null) {

            }

            Destroy(collision.gameObject);
        }
    }


}
