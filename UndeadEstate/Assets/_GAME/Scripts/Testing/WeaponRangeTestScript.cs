using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRangeTestScript : MonoBehaviour
{
    public Camera rangeCamera;
    public Vector3 cameraOffset;
    public PlayerWeapon[] weaponRegistry;

    private void MoveCameraToWeapon(EWeapon weaponType)
    {
        foreach (PlayerWeapon entry in weaponRegistry)
        {
            if (entry.weaponType == weaponType)
            {
                rangeCamera.transform.position = entry.transform.position + cameraOffset;
            }
        }
    }
    
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            foreach (PlayerWeapon entry in weaponRegistry)
            {
                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button($"Go To {entry.weaponType.ToString()}"))
                    {
                        MoveCameraToWeapon(entry.weaponType);
                    }
                    
                    GUILayout.Space(20);
                    
                    if (GUILayout.Button("Fire"))
                    {
                        entry.Fire();
                        entry.StopFire();
                    }
                    
                    if (GUILayout.Button("Alt Fire"))
                    {
                        entry.AltFire();
                    }
                    
                    if (GUILayout.Button("Reload"))
                    {
                        entry.Reload();
                    }
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndHorizontal();
    }
}
