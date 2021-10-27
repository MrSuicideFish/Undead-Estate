using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerModule : NetworkBehaviour
{
    public PlayerWeapon weapon;

    public float walkSpeed = 5;
    public float sprintSpeed = 5;
    
    public int health = 100;
    public int armor = 0;

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
        
        if (GetNetworkIdentity().isLocalPlayer)
        {
            this.gameObject.AddComponent<PlayerInput>();
            _camera.gameObject.SetActive(true);
        }
        else
        {
            _camera.gameObject.SetActive( false );
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
        
    }

    public void AltFire()
    {
        
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

    private void FixedUpdate()
    {
        float speed = walkSpeed;
        _rigidbody.MovePosition(transform.position + (moveDirection * speed * Time.deltaTime));
        
        // snap rotation
        //rotation = Snapping.Snap(rotation, 45.0f);
        _rigidbody.MoveRotation(quaternion.Euler(0, rotation, 0));
    }


}
