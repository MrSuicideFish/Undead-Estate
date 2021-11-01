using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class PlayerWeapon : NetworkBehaviour
{
    private static PlayerWeapon[] _allWeapons;
    private static PlayerWeapon[] AllWeapons
    {
        get
        {
            if (_allWeapons == null || _allWeapons.Length == 0)
            {
                _allWeapons = Resources.LoadAll<PlayerWeapon>("Weapon");
            }
            return _allWeapons;
        }
    }

    public EWeapon weaponType;
    public int damage;
    public float armorPenPerc;
    public float knockbackScale;
    
    public virtual void Fire()
    {
        
    }
    
    public virtual void StopFire()
    {
        
    }

    public virtual void AltFire()
    {
        
    }

    public virtual void StopAltFire()
    {
        
    }

    public virtual void Reload()
    {
        
    }

    public static PlayerWeapon Create(EWeapon weaponType)
    {
        foreach (PlayerWeapon weap in AllWeapons)
        {
            if (weap.weaponType == weaponType)
            {
                PlayerWeapon inst = EstateNetworkManager.Instantiate(weap);
                return inst;
            }
        }
        return null;
    }
}
