using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToasterLaunch : MonoBehaviour
{
    public float launchForce = 20f;
    public float toastTime = 2f;
    public float shakeIntensity = 0.05f;
    public string playerTag = "Player";
    public float cooldownTime = 1.5f;

    private HashSet<Rigidbody> recentlyToasted = new HashSet<Rigidbody>();
    private BoxCollider toasterCollider;
    private AudioSource audioSource;


    private void Awake()
    {
        toasterCollider = GetComponent<BoxCollider>();
        if (toasterCollider == null || !toasterCollider.isTrigger)
        {
            Debug.LogWarning("Toaster must have a BoxCollider set as Trigger.");
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !recentlyToasted.Contains(rb))
            {
                StartCoroutine(ToastPlayer(rb));
            }
        }
    }

    private IEnumerator ToastPlayer(Rigidbody playerRb)
    {
        recentlyToasted.Add(playerRb);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        // Move player to center of toaster
        Vector3 centerWorldPosition = toasterCollider.transform.TransformPoint(toasterCollider.center);
        playerRb.transform.position = centerWorldPosition;

        playerRb.isKinematic = true;

        float elapsed = 0f;
        while (elapsed < toastTime)
        {
            elapsed += Time.deltaTime;

            // Shake
            Vector3 offset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity)
            );

            playerRb.transform.position = centerWorldPosition + offset;

            yield return null;
        }

        // Reset to center
        playerRb.transform.position = centerWorldPosition;

        // Unfreeze and launch
        playerRb.isKinematic = false;
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);
        playerRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

        // Cooldown
        yield return new WaitForSeconds(cooldownTime);
        recentlyToasted.Remove(playerRb);
    }
}
