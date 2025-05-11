using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkTeleporter : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject teleportPt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player1 || other.gameObject == Player2)
        {
            // Teleport the player to the teleport point
            other.transform.position = teleportPt.transform.position;
        }
    }
}