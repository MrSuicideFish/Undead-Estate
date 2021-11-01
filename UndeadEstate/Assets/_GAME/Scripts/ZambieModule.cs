using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ZambieModule : MonoBehaviour
{
    private const float SEARCH_DELAY_BUFFER = 0.4f;
    
    public float targetSearchDelay = 1.0f; 
    private GameObject target;
    private NavMeshAgent _navMeshAgent;

    private float targetSearchTime = 0.0f;
    private float searchTime = 0.0f;
    
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        targetSearchTime = Random.Range(targetSearchDelay - SEARCH_DELAY_BUFFER, 
            targetSearchDelay + SEARCH_DELAY_BUFFER);
        
        // set start target
        target = FindNearestTarget(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        // search for new target
        searchTime += Time.deltaTime;
        if (searchTime >= targetSearchTime)
        {
            target = FindNearestTarget(this.transform);
            searchTime = 0.0f;
        }

        // move towards target or stop
        if (target != null)
        {
            _navMeshAgent.SetDestination(target.transform.position);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    protected static GameObject FindNearestTarget(Transform zambie)
    {
        Collider[] hitColliders =
            Physics.OverlapSphere(zambie.position, 500, LayerMask.GetMask("Survivors"));

        if (hitColliders == null || hitColliders.Length == 0) return null;

        foreach (Collider col in hitColliders)
            if (col.gameObject != null) return col.gameObject;
        return null;
    }

}
