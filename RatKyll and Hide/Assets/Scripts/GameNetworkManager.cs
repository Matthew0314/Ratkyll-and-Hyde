using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : NetworkBehaviour
{
    public GameObject playerPrefab;

    public void StartHost() => NetworkManager.Singleton.StartHost();
    public void StartClient() => NetworkManager.Singleton.StartClient();

    public override void OnNetworkSpawn()
    {
        if (IsServer) 
        {
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<NetworkObject>().Spawn();
        }
    }
}
