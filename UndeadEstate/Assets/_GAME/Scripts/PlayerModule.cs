using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cinemachine;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerModule : NetworkBehaviour
{
    public PlayerWeapon weapon;

    [SyncVar] public float walkSpeed = 5;
    [SyncVar] public float sprintSpeed = 5;

    [SyncVar] public int health = 100;
    [SyncVar] public int armor = 0;
    [SyncVar] public float stamina = 1.0f;

    private Rigidbody _rigidbody;
    private CapsuleCollider _moveCollider;
    private CinemachineVirtualCamera _camera;

    private Vector3 moveDirection;
    private float rotation;

    private void OnEnable()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _moveCollider = this.GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        float speed = walkSpeed;
        _rigidbody.MovePosition(transform.position + (moveDirection * speed * Time.deltaTime));

        // snap rotation
        _rigidbody.MoveRotation(quaternion.Euler(0, rotation, 0));
    }

    public override void OnStartClient()
    {
        // setup camera
        CinemachineVirtualCamera camRes = Resources.Load<CinemachineVirtualCamera>($"PLAYER_CAMERA");
        CinemachineVirtualCamera playerCam = EstateNetworkManager.Instantiate(camRes);
        playerCam.Follow = this.transform;
        playerCam.transform.position = this.transform.position;

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

        // set camera
        _camera = playerCam;

        PlayerInput input = this.gameObject.AddComponent<PlayerInput>();
        if (GetNetworkIdentity().isLocalPlayer)
        {
            _camera.gameObject.SetActive(true);
            input.enabled = true;
        }
        else
        {
            _camera.gameObject.SetActive(false);
            input.enabled = false;
        }
    }

    public NetworkIdentity GetNetworkIdentity()
    {
        return this.GetComponent<NetworkIdentity>();
    }

    public void SetPlayerCamera(CinemachineVirtualCamera cam)
    {
        _camera = cam;
    }

    public CinemachineVirtualCamera GetPlayerCamera()
    {
        return _camera;
    }
    
    public void Fire()
    {
        if (this.weapon != null) this.weapon.Fire();
        if (isLocalPlayer)
        {
            if (isServer)
            {
                RpcFire();
            }
            else if(isClientOnly)
            {
                CmdFire();
            }
        }
    }

    [ClientRpc]
    public void RpcFire()
    {
        if (this.weapon != null) this.weapon.Fire();
    }

    [Command]
    public void CmdFire()
    {
        if (this.weapon != null) this.weapon.Fire();
    }

    public void StopFire()
    {
        if (this.weapon != null) this.weapon.StopFire();
        if (isLocalPlayer)
        {
            if (isServer)
            {
                RpcStopFire();
            }
            else if(isClientOnly)
            {
                CmdStopFire();
            }
        }
    }

    [Command]
    public void CmdStopFire()
    {
        if (this.weapon != null) this.weapon.StopFire();
    }
    
    [ClientRpc]
    public void RpcStopFire()
    {
        if (this.weapon != null) this.weapon.StopFire();
    }

    public void AltFire()
    {
        if (this.weapon != null) this.weapon.AltFire();
        if (isLocalPlayer)
        {
            if (isServer)
            {
                RpcAltFire();
            }
            else if(isClientOnly)
            {
                CmdAltFire();
            }
        }
    }
    
    [Command]
    public void CmdAltFire()
    {
        if (this.weapon != null) this.weapon.AltFire();
    }
    
    [ClientRpc]
    public void RpcAltFire()
    {
        if (this.weapon != null) this.weapon.AltFire();
    }

    public void StopAltFire()
    {
        if (this.weapon != null) this.weapon.StopAltFire();
        if (isLocalPlayer)
        {
            if (isServer)
            {
                RpcStopAltFire();
            }
            else if(isClientOnly)
            {
                CmdStopAltFire();
            }
        }
    }
    
        
    [Command]
    public void CmdStopAltFire()
    {
        if (this.weapon != null) this.weapon.AltFire();
        
    }
    
    [ClientRpc]
    public void RpcStopAltFire()
    {
        if (this.weapon != null) this.weapon.AltFire();
    }
    
    public void Reload()
    {
        if (this.weapon != null)
        {
            this.weapon.Reload();
        }
        
        if (isLocalPlayer)
        {
            if (isServer)
            {
                RpcReload();
            }
            else if(isClientOnly)
            {
                CmdReload();
            }
        }
    }

    [Command]
    public void CmdReload()
    {
        if (this.weapon != null)
        {
            this.weapon.Reload();
        }
    }
    
    [ClientRpc]
    public void RpcReload()
    {
        if (this.weapon != null)
        {
            this.weapon.Reload();
        }
    }

    public void Move(float x, float z)
    {
        moveDirection = new Vector3(x, 0, z);
    }

    public void SetLookPosition(Vector3 mouseWorldPos)
    {
        Vector3 dir = mouseWorldPos - transform.position;
        rotation = Mathf.Atan2(dir.x, dir.z);
    }
    
    [Client]
    public void GiveWeapon(EWeapon weaponType)
    {
        if (this.weapon != null)
        {
            GameObject.Destroy(this.weapon.gameObject);
        }
        
        PlayerWeapon newWeapon = PlayerWeapon.Create(weaponType);
        if (newWeapon == null)
        {
            Debug.LogError("Failed to create weapon: " + weaponType);
        }
        
        newWeapon.transform.SetParent(this.transform, true);
        newWeapon.transform.localPosition = new Vector3(0.5f, 1, 0);
        newWeapon.transform.localEulerAngles = Vector3.zero;

        this.weapon = newWeapon;
    }

    public bool HasWeapon(EWeapon weapon)
    {
        if (this.weapon == null)
        {
            return false;
        }

        return this.weapon.weaponType == weapon;
    }
}
