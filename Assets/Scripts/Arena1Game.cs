using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Arena1Game : NetworkBehaviour
{

    public Player playerPrefab;
    public Player hostPrefab;
    public Camera arenaCamera;

    private NetworkedPlayers networkedPlayers;

    private int positionIndex = 0;
    private Vector3[] startPositions = new Vector3[]
    {
        new Vector3(-15, 2, -88),
        new Vector3(-12, 2, -88),
        new Vector3(-10, 2, -88),
        new Vector3(-8, 2, -88)

    };





    // Start is called before the first frame update
    void Start()
    {

        arenaCamera.enabled = !IsClient;
        arenaCamera.GetComponent<AudioListener>().enabled = !IsClient;

        networkedPlayers = GameObject.Find("NetworkedPlayers").GetComponent<NetworkedPlayers>();

        if (IsServer)
        {
            SpawnPlayers();
        }
        
    }

    private Vector3 NextPosition()
    {
        Vector3 pos = startPositions[positionIndex];
        positionIndex += 1;
        if (positionIndex > startPositions.Length - 1)
        {
            positionIndex = 0;
        }
        return pos;
    }


 


    private void SpawnPlayers()
    {

        foreach (NetworkPlayerInfo info in networkedPlayers.allNetPlayers)
        {
           // if (clientId == NetworkManager.LocalClientId)
            //{

           //     Player playerSpawn = Instantiate(hostPrefab, NextPosition(), Quaternion.identity);
           //     playerSpawn.playerColorNetVar.Value = NextColor();
           //     playerSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
          //  }
          //  else
            {
                Player playerSpawn = Instantiate(playerPrefab, NextPosition(), Quaternion.identity);
                playerSpawn.playerColorNetVar.Value = info.color;
                playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(info.clientId);

            }
        }
    }
}
