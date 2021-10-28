using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class ZambieModule : MonoBehaviour
{
    private GameObject target;

    private NavMeshAgent _navMeshAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkClient.isConnected && NetworkClient.Ready() && NetworkClient.isHostClient)
        {
            if (target == null)
            {
                target = NetworkServer.spawned[NetworkServer.localConnection.identity.netId].gameObject;
                return;
            }
            
            _navMeshAgent.SetDestination(target.transform.position);
        }
    }
}
