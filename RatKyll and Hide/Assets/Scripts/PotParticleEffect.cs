using UnityEngine;

public class PotParticleEffect : MonoBehaviour
{
    [Header("Particle System Settings")]
    public Color particleColor = Color.yellow;
    public float particleSize = 0.1f;
    public float emissionRate = 20f;
    public float particleLifetime = 1f;
    public float particleSpeed = 1f;

    [Header("Effect Dynamics")]
    public float upwardForce = 2f;
    public float spreadAngle = 30f;

    private ParticleSystem potParticles;

    void Start()
    {
        CreateParticleSystem();
    }

    void CreateParticleSystem()
    {
        // Create a new GameObject for the Particle System
        GameObject particleSystemObject = new GameObject("PotParticles");
        particleSystemObject.transform.SetParent(transform);
        particleSystemObject.transform.localPosition = Vector3.zero;

        // Add Particle System Component
        potParticles = particleSystemObject.AddComponent<ParticleSystem>();

        // Configure the Particle System
        var main = potParticles.main;
        main.startColor = particleColor;
        main.startSize = particleSize;
        main.startLifetime = particleLifetime;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        // Emission module
        var emission = potParticles.emission;
        emission.rateOverTime = emissionRate;

        // Velocity over Lifetime module
        var velocityOverLifetime = potParticles.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.y = upwardForce;

        // Shape module
        var shape = potParticles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = spreadAngle;

        // Start the Particle System
        potParticles.Play();
    }

    // Optional: Method to adjust particle effect intensity
    public void SetParticleIntensity(float intensity)
    {
        if (potParticles != null)
        {
            var emission = potParticles.emission;
            emission.rateOverTime = emissionRate * intensity;
        }
    }

    // Optional: Method to change particle color
    public void ChangeParticleColor(Color newColor)
    {
        if (potParticles != null)
        {
            var main = potParticles.main;
            main.startColor = newColor;
        }
    }
}
