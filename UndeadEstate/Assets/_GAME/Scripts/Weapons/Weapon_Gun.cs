
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : PlayerWeapon
{
    public int MagazineSize = 10;
    public int ammoInMag = 0;

    public WeaponParticleManager[] bulletParticles;

    public override void Reload()
    {
        base.Reload();
        ammoInMag = MagazineSize;
    }

    protected void GetBulletsFx(out WeaponParticleManager[] fx, params int[] indicies)
    {
        List<WeaponParticleManager> bulletFxs = new List<WeaponParticleManager>();
        foreach (int idx in indicies)
        {
            if (idx < 0 || idx > bulletParticles.Length || bulletParticles[idx] == null)
            {
                Debug.LogError("Bullet FX index out of range: " + idx);
                continue;
            }
            
            bulletFxs.Add(bulletParticles[idx]);
        }
        
        fx = bulletFxs.ToArray();
    }

    protected void FireBulletsFx(params int[] indexes)
    {
        foreach (int index in indexes)
        {
            if (index < 0
                || index > bulletParticles.Length
                || bulletParticles[index] == null)
            {
                Debug.LogError("Missing index in bullets FX: " + index.ToString());
                continue;
            }
            
            bulletParticles[index].Fire();
        }
    }
}