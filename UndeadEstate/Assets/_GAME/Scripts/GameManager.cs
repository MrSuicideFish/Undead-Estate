using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _current;
    public static GameManager Current
    {
        get
        {
            return _current;
        }
    }
    
    public bool IsOfflineMode = true;

    [Header("Camera")]
    public CinemachineBrain StageCamera;

    public void OnEnable()
    {
        _current = this;
    }
    
    public PlayerModule SpawnPlayer(string survivorId, string startingWeapon, NetworkConnection conn = null)
    {
        // find survivor
        PlayerModule module = Resources.Load<PlayerModule>($"Characters/Survivors/PrototypeChar");
        
        // spawn survivor
        return EstateNetworkManager.Instantiate(module, 
            new Vector3(0, 10, 0),
        Quaternion.Euler(0, 0, 0));
        
    }

    public void KillPlayer(PlayerModule mod)
    {
        mod.gameObject.SetActive(false);
    }
}
