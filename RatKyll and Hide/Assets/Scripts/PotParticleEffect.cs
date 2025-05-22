//This is script is still used but the object it is used on is now invisible to the player.
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
        GameObject particleSystemObject = new GameObject("PotParticles");
        particleSystemObject.transform.SetParent(transform);
        particleSystemObject.transform.localPosition = Vector3.zero;

        potParticles = particleSystemObject.AddComponent<ParticleSystem>();

        var main = potParticles.main;
        main.startColor = particleColor;
        main.startSize = particleSize;
        main.startLifetime = particleLifetime;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = potParticles.emission;
        emission.rateOverTime = emissionRate;

        var velocityOverLifetime = potParticles.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.y = upwardForce;

        var shape = potParticles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = spreadAngle;

        potParticles.Play();
    }

    public void SetParticleIntensity(float intensity)
    {
        if (potParticles != null)
        {
            var emission = potParticles.emission;
            emission.rateOverTime = emissionRate * intensity;
        }
    }
    public void ChangeParticleColor(Color newColor)
    {
        if (potParticles != null)
        {
            var main = potParticles.main;
            main.startColor = newColor;
        }
    }
}
