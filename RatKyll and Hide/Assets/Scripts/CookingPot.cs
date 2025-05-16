using UnityEngine;

public class CookingPot : MonoBehaviour
{
    private PotGameManager potGameManager;
    private AudioSource audioSource;
    private AudioSource splashAudioSource;  // For the one-time splash sound
    public float detectionRadius = 5f; // How close the player needs to be
    public string playerTag = "Player"; // Tag of the player object
    public AudioClip splashSound;
    public AudioClip playerSplashSound; // New audio for when a player falls in
    public float throwForce = 300f; // Force to throw the player upward, similar to your SinkTeleporter
    public float horizontalForce = 150f; // Optional horizontal force for more dramatic ejection

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

    private void OnTriggerEnter(Collider collider)
    {
        // Check if it's a player
        if (collider.gameObject.CompareTag(playerTag))
        {
            // Get the player's Rigidbody
            Rigidbody playerRb = collider.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Calculate random horizontal direction (optional)
                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    0,
                    Random.Range(-1f, 1f)
                ).normalized;
                
                // Apply upward force and random horizontal force
                playerRb.linearVelocity = Vector3.zero; // Reset velocity first for consistent behavior
                playerRb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
                playerRb.AddForce(randomDirection * horizontalForce, ForceMode.Impulse);
                
                // Play sound effect for player being ejected
                if (splashAudioSource != null && playerSplashSound != null)
                {
                    splashAudioSource.PlayOneShot(playerSplashSound);
                }
            }
        }
        // Process pickable items as before
        else if (collider.gameObject.CompareTag("Pickable"))
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
