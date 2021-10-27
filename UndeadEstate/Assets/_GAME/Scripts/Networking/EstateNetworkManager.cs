using System;
using Mirror;
using UnityEngine;

public class EstateNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreateSurvivorCharacterMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        CreateSurvivorCharacterMessage characterMessage = new CreateSurvivorCharacterMessage()
        {
            survivorId = "",
            startingWeaponId = ""
        };
        
        conn.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnection conn, CreateSurvivorCharacterMessage message)
    {
        PlayerModule newPlayer = GameManager.Current.SpawnPlayer(message.survivorId,
            message.startingWeaponId, conn);
        newPlayer.gameObject.name = $"Player_{conn.connectionId}";
        
        NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject);
    }
}