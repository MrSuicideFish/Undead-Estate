using System;
using UnityEngine;

[ExecuteInEditMode]
public class WeaponParticleManager : MonoBehaviour
{
    private ParticleSystem _system;

    public void OnEnable()
    {
        _system = this.GetComponent<ParticleSystem>();
    }

    public void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Collision");
    }

    public void OnParticleTrigger()
    {
        Debug.Log("Particle Trigger");
    }

    public void Fire()
    {
        _system.Play(withChildren: false);
    }
}