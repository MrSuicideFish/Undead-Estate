using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    public CinemachineVirtualCamera defaultSurvivorCam;
    
    [Header("Testing / Debugging")]
    public bool SpawnPlayerOnStart = true;

    public PlayerModule PlayerToSpawn = null;

    private List<PlayerModule> _spawnedPlayers;
    private List<PlayerModule> _deadPlayers;

    public void OnEnable()
    {
        _current = this;
    }

    public void Start()
    {
        if (SpawnPlayerOnStart)
        {
            if (PlayerToSpawn != null)
            {
                SpawnPlayer("Debug Player", PlayerToSpawn);
            }
        }
    }

    public void SpawnPlayer(string playerId, PlayerModule module)
    {
        if (_spawnedPlayers == null)
        {
            _spawnedPlayers = new List<PlayerModule>();
        }

        Vector3 spawnLocation = new Vector3(0, 10, 0);
        PlayerModule mod = GameObject.Instantiate(module, spawnLocation, Quaternion.Euler(0, 0, 0));
        mod.AddComponent<PlayerInput>();
        mod.playerId = playerId;
        
        // setup camera
        CinemachineVirtualCamera playerCam = GameObject.Instantiate(defaultSurvivorCam);
        playerCam.Follow = mod.transform;
        playerCam.transform.position = mod.transform.position;
        
        CinemachineTransposer transposer = playerCam.AddCinemachineComponent<CinemachineTransposer>();
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;

        var position = this.transform.position;
        playerCam.transform.position = new Vector3()
        {
            x = position.x,
            y = position.y + 10,
            z = position.z - 10
        };
        
        playerCam.transform.eulerAngles = new Vector3(30, 0, 0);
        transposer.m_FollowOffset = new Vector3(0, 10, -15);
        
        mod.SetPlayerCamera(playerCam);

        _spawnedPlayers.Add(mod);
    }

    public void KillPlayer(PlayerModule mod)
    {
        if (_deadPlayers == null)
        {
            _deadPlayers = new List<PlayerModule>();
        }
        
        mod.gameObject.SetActive(false);
        _deadPlayers.Add(mod);
    }
}
