
public class WeaponPickup : Pickup
{
    public EWeapon weapon;
    
    protected override bool DoPickup(PlayerModule mod)
    {
        if (mod.HasWeapon(weapon))
        {
            return false;
        }
        
        mod.GiveWeapon(weapon);
        return true;
    }
}