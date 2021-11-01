using Mirror;

public enum EWeapon
{
    Undefined,
    Pistol
}

public interface IDamageable
{
    int damage { get; set; }
    void Hit(Damage.DamageHitInfo hitInfo)
    {
        
    }
}

public struct CreateSurvivorCharacterMessage : NetworkMessage
{
    public string survivorId;
    public string startingWeaponId;
}