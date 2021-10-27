
using Mirror;

public struct CreateSurvivorCharacterMessage : NetworkMessage
{
    public string survivorId;
    public string startingWeaponId;
}