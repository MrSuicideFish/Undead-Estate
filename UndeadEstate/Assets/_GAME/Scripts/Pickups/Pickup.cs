using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Pickup : MonoBehaviour
{
    private SphereCollider _collider;

    protected abstract bool DoPickup(PlayerModule mod);
    
    private void OnEnable()
    {
        int pickupLayer = LayerMask.NameToLayer("Pickups");
        if (this.gameObject.layer != pickupLayer)
            this.gameObject.layer = pickupLayer;
        
        _collider = this.GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerModule mod = other.GetComponent<PlayerModule>();
        if (mod != null)
        {
            if (this.DoPickup(mod))
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}