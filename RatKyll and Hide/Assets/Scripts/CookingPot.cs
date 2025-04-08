using UnityEngine;

public class CookingPot : MonoBehaviour
{
    private PotGameManager potGameManager;
    void Start() {
        // Get the potGameManager script using gameobject.Find();
        potGameManager = GameObject.Find("GameManager").GetComponent<PotGameManager>();
    }

    /*private void OnCollisionEnter(Collision collision) {
        // Might have to change the tag
        // Supposed to check if a consumable food so it can destroy the object and add to the score
        if (collision.gameObject.CompareTag("Pickable")) {
            if (collision.gameObject.GetComponent<IPickUpItem>().LastPlayer != null) {
                IPickUpItem item = collision.gameObject.GetComponent<IPickUpItem>();
                potGameManager.IncScore(item.LastPlayer, item);
            }

            Destroy(collision.gameObject);
        }

        // In the event that its a ketchup bottle add code to spit it out, can be done for next sprint
    }*/

    private void OnTriggerEnter(Collider collider) {
        // Might have to change the tag
        // Supposed to check if a consumable food so it can destroy the object and add to the score
        if (collider.gameObject.CompareTag("Pickable")) {
            if (collider.gameObject.GetComponent<IPickUpItem>().LastPlayer != null) {
                IPickUpItem item = collider.gameObject.GetComponent<IPickUpItem>();
                potGameManager.IncScore(item.LastPlayer, item);
            }

            Destroy(collider.gameObject);
        }

        // In the event that its a ketchup bottle add code to spit it out, can be done for next sprint
    }


}
