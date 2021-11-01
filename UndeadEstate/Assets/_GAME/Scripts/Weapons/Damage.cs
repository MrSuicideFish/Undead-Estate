using System;
using Mirror;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public static class Damage
{
    public enum EDamageType
    { 
        Bullet
    }

    [Flags]
    public enum EDamageLayer : byte
    {
        Zambies = 1 << 6,
        Structures = 1 << 7,
        All
    }

    public class DamageHitInfo
    {
        public IDamageable hitObject;
        public EDamageType damageType;
        
        public int damage;
        public float armorPenPerc;
        public float knockbackScale;
    }

    public static LayerMask GetDamageMask(EDamageLayer layer)
    {
        LayerMask mask;

        if (layer == EDamageLayer.All)
        {
            mask = EDamageLayer.Zambies.GetHashCode() & EDamageLayer.Structures.GetHashCode();
        }
        else
        {
            mask = layer.GetHashCode();
        }

        return mask;
    }
    
    [Command]
    public static bool Hitscan(Vector3 pos, Vector3 dir,
                                EDamageType damageType, EDamageLayer layer,
                                int damage, out DamageHitInfo dmgHitInfo,
                                float armorPenPerc = 0.0f, float knockbackScale = 0.0f)
    {
        dmgHitInfo = null;
        
        RaycastHit hitInfo;
        Ray ray = new Ray(pos, dir);

        if (Physics.Raycast(ray, out hitInfo, GetDamageMask(layer)))
        {
            dmgHitInfo = new DamageHitInfo();

            IDamageable dmgable = hitInfo.collider.gameObject.GetComponentInChildren<IDamageable>();
            if (dmgable != null)
            {
                dmgHitInfo.hitObject = dmgable;
                dmgHitInfo.damage = damage;
                dmgHitInfo.armorPenPerc = armorPenPerc;
                dmgHitInfo.knockbackScale = knockbackScale;
                
                // apply hit
                dmgable.Hit(dmgHitInfo);
                
                return true;
            }
        }

        return false;
    }
}