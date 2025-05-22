using UnityEngine;

public class CookingPot : MonoBehaviour
{
    private PotGameManager potGameManager;
    private AudioSource audioSource;
    private AudioSource splashAudioSource;  // For the one-time splash sound
    public float detectionRadius = 5f;
    public string playerTag = "Player";
    public AudioClip splashSound;
    public AudioClip playerSplashSound; // audio for when a player falls in
    public float throwForce = 300f; 
    public float horizontalForce = 150f; 

    void Start()
    {
        potGameManager = GameObject.Find("GameManager").GetComponent<PotGameManager>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Stop();
        
        splashAudioSource = gameObject.AddComponent<AudioSource>();
        splashAudioSource.loop = false;
        splashAudioSource.playOnAwake = false;
        if (splashSound != null) {
            splashAudioSource.clip = splashSound;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(playerTag))
        {
            Rigidbody playerRb = collider.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    0,
                    Random.Range(-1f, 1f)
                ).normalized;
                
                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
                playerRb.AddForce(randomDirection * horizontalForce, ForceMode.Impulse);
                
                // Play sound effect for player being ejected
                if (splashAudioSource != null && playerSplashSound != null)
                {
                    splashAudioSource.PlayOneShot(playerSplashSound);
                }
            }
        }
        else if (collider.gameObject.CompareTag("Pickable"))
        {
            if (splashAudioSource != null && splashSound != null) {
                splashAudioSource.PlayOneShot(splashSound);
            }

            IPickUpItem pickUp = collider.gameObject.GetComponent<IPickUpItem>();
            if (pickUp.Points <= 0) {
                pickUp.DestroyItem();
                return;
            }
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
