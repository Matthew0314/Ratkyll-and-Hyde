using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAssigner : NetworkBehaviour
{
    public List<GameObject> playerObjects; // Assign these in the Inspector
    private int assignedPlayers = 0;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += AssignPlayer;
    }

    private void AssignPlayer(ulong clientId)
    {
        if (assignedPlayers < playerObjects.Count)
        {
            GameObject playerObject = playerObjects[assignedPlayers];
            NetworkObject netObj = playerObject.GetComponent<NetworkObject>();

            if (netObj != null && netObj.IsSpawned)
            {
                netObj.ChangeOwnership(clientId);
            }

            assignedPlayers++;
        }
    }
}
