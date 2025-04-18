using UnityEngine;

public class CookingPot : MonoBehaviour
{
<<<<<<< HEAD
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Food")) {
            if (collision.gameObject.GetComponent<IPickUpItem>().LastPlayer != null) {

=======
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
>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
            }

            Destroy(collision.gameObject);
        }
<<<<<<< HEAD
=======

        // In the event that its a ketchup bottle add code to spit it out, can be done for next sprint
    }*/

    private void OnTriggerEnter(Collider collider) {
        // Might have to change the tag
        // Supposed to check if a consumable food so it can destroy the object and add to the score
        if (collider.gameObject.CompareTag("Pickable")) {
            IPickUpItem pickUp = collider.gameObject.GetComponent<IPickUpItem>();
            if (pickUp.LastPlayer != null) {
                IPickUpItem item = collider.gameObject.GetComponent<IPickUpItem>();
                potGameManager.IncScore(item.LastPlayer, item);
            }
            pickUp.DestroyItem();
        }

        // In the event that its a ketchup bottle add code to spit it out, can be done for next sprint
>>>>>>> b6cc0bf6b576ae9c4114342cda1821025f478e88
    }


}
