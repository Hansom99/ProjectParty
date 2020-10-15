using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
   // public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
         
        
        PhotonNetwork.Instantiate("Player", new Vector3(PhotonNetwork.LocalPlayer.ActorNumber, 0,0), Quaternion.identity);
        //PhotonNetwork.SerializationRate = 10;
        //PhotonNetwork.SendRate = 20;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
