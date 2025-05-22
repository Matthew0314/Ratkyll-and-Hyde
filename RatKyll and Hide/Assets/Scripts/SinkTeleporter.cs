using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkTeleporter : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject teleportPt;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player1 || other.gameObject == Player2)
        {
            other.transform.position = teleportPt.transform.position;
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 300f, ForceMode.Impulse);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
