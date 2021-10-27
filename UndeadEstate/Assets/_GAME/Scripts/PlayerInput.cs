using System;
using UnityEngine;

[RequireComponent(typeof(PlayerModule))]
public class PlayerInput : MonoBehaviour
{
    private PlayerModule _module;
    public bool debugMode = false;


    private Vector3 worldMousePos;
    
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