using UnityEngine;

public class CookingPot : MonoBehaviour
{
    private PotGameManager potGameManager;
    private AudioSource audioSource;
    private AudioSource splashAudioSource;  // For the one-time splash sound
    public float detectionRadius = 5f; // How close the player needs to be
    public string playerTag = "Player"; // Tag of the player object
     public AudioClip splashSound;
    void Start()
    {
        // Get the potGameManager script using gameobject.Find();
        potGameManager = GameObject.Find("GameManager").GetComponent<PotGameManager>();
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if it doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure audio to loop but start off
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Stop();
        
        splashAudioSource = gameObject.AddComponent<AudioSource>();
        splashAudioSource.loop = false;
        splashAudioSource.playOnAwake = false;
        // Assign the splash sound clip if provided in the inspector
        if (splashSound != null) {
            splashAudioSource.clip = splashSound;
        }
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

    private void OnTriggerEnter(Collider collider)
    {
        // Might have to change the tag
        // Supposed to check if a consumable food so it can destroy the object and add to the score
        if (collider.gameObject.CompareTag("Pickable"))
        {
            if (splashAudioSource != null && splashSound != null) {
                splashAudioSource.PlayOneShot(splashSound);
            }

            IPickUpItem pickUp = collider.gameObject.GetComponent<IPickUpItem>();
            if (pickUp.LastPlayer != null)
            {
                IPickUpItem item = collider.gameObject.GetComponent<IPickUpItem>();
                potGameManager.IncScore(item.LastPlayer, item);

            }
            pickUp.DestroyItem();
        }

        // In the event that its a ketchup bottle add code to spit it out, can be done for next sprint
    }
    
    void Update()
    {
        // Check if any player is in range
        bool anyPlayerInRange = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
        
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRadius)
            {
                anyPlayerInRange = true;
                break;
            }
        }
        
        // Play or stop audio based on player proximity
        if (anyPlayerInRange && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!anyPlayerInRange && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }


}
