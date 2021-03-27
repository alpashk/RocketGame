using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem ThrustParticles;
    [SerializeField] private ParticleSystem StationaryParticles;

    private void Awake()
    {
        Controls.FlightControl += MaxParticles;
        Controls.OffThrottle += MinParticles;
    }

    private void Start()
    {
        MinParticles();
    }

    private void MaxParticles()
    {
        ThrustParticles.Play();
        StationaryParticles.Stop();
    }

    private void MinParticles()
    {
        StationaryParticles.Play();
        ThrustParticles.Stop(); 
    }

    private void OnDestroy()
    {
        Controls.FlightControl -= MaxParticles;
        Controls.OffThrottle -= MinParticles;
    }
}
