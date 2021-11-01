using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Weapon_Pistol : Weapon_Gun
{
    private bool triggerRelease = false;
    
    private void Awake()
    {
        ammoInMag = 50;
    }

    public override void StopFire()
    {
        base.StopFire();
        triggerRelease = false;
    }

    public override void Fire()
    {
        if (triggerRelease) return;
        if (ammoInMag > 0)
        {
            WeaponParticleManager[] fx;
            GetBulletsFx(out fx, 0);
            if (fx != null)
            {
                // fire bullet fx
                WeaponParticleManager muzzle = fx[0];
                muzzle.Fire();

                // hit result
                Damage.DamageHitInfo hitInfo;
                
                // try hit something
                if (Damage.Hitscan(muzzle.transform.position, muzzle.transform.forward,
                    Damage.EDamageType.Bullet, Damage.EDamageLayer.All, this.damage,
                    out hitInfo, armorPenPerc, knockbackScale))
                {
                    
                }
            }

            triggerRelease = true;
            ammoInMag--;
        }
    }
}
