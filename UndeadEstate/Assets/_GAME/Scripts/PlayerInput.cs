using System;
using UnityEngine;

[RequireComponent(typeof(PlayerModule))]
public class PlayerInput : MonoBehaviour
{
    private PlayerModule _module;
    public bool debugMode = false;

    private Vector3 worldMousePos;
    private bool isFiring;
    private bool isAltFiring;
    
    private PlayerModule module
    {
        get
        {
            if (_module == null)
            {
                _module = this.GetComponent<PlayerModule>();
            }
            return _module;
        }
    }
    
    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        module.Move(moveX, moveY);

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, new Vector3(0, this.transform.position.y, 0));
        float dist = 0;
        if (p.Raycast(r, out dist))
        {
            worldMousePos = r.GetPoint(dist);
            module.SetLookPosition(worldMousePos);
        }

        if (isFiring && (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0)))
        {
            isFiring = false;
            module.StopFire();
        }

        if (isAltFiring && (Input.GetMouseButtonUp(1) || !Input.GetMouseButton(1)))
        {
            isAltFiring = false;
            module.StopAltFire();
        }
        
        if (!isFiring && Input.GetMouseButton(0))
        {
            module.Fire();
            isFiring = true;
        }
        
        if(!isAltFiring && Input.GetMouseButton(1))
        {
            module.AltFire();
            isAltFiring = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            module.Reload();
        }
    }

    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(worldMousePos, 0.3f);
        }
    }
}