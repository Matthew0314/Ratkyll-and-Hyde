using UnityEngine;

public class FanZone : MonoBehaviour
{
    public string playerTag = "Player";
    public float pushForce = 15f;
    public float maxUpwardVelocity = 20f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && rb.linearVelocity.y < maxUpwardVelocity)
            {
                rb.AddForce(Vector3.up * pushForce, ForceMode.Acceleration);
            }
        }
    }
}
