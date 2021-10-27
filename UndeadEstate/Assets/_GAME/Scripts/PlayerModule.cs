using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerModule : MonoBehaviour
{
    public string playerId;
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

    public void SetPlayerCamera(CinemachineVirtualCamera cam)
    {

        _camera = cam;
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
