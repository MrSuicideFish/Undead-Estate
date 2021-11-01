using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRangeTestScript : MonoBehaviour
{
    [Serializable]
    public class WeaponEntry
    {
        [SerializeField] public EWeapon weaponType;
        [SerializeField] public PlayerWeapon weapon;
    }

    public Camera rangeCamera;
    public Vector3 cameraOffset;
    public WeaponEntry[] weaponRegistry;

    private void MoveCameraToWeapon(EWeapon weaponType)
    {
        foreach (WeaponEntry entry in weaponRegistry)
        {
            if (entry.weaponType == weaponType)
            {
                rangeCamera.transform.position = entry.weapon.transform.position + cameraOffset;
            }
        }
    }
    
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            foreach (WeaponEntry entry in weaponRegistry)
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
                        entry.weapon.Fire();
                    }
                    
                    if (GUILayout.Button("Alt Fire"))
                    {
                        entry.weapon.AltFire();
                    }
                    
                    if (GUILayout.Button("Reload"))
                    {
                        entry.weapon.Reload();
                    }
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndHorizontal();
    }
}
