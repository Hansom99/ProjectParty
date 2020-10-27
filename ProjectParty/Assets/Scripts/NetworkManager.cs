using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject myPlayer;

    Health Health;
    PlayerMovement playerMovement;


    public float health { get { return myPlayer.GetComponent<Health>().health; } }
    public float maxHealth { get { return myPlayer.GetComponent<Health>().maxHealth; } }
    public int Ammo { get { return playerMovement.Ammo; } }

    public int kills = 0;
    public int deaths = 0;

    

    // Start is called before the first frame update
    void Awake()
    {
         
        
        myPlayer =  PhotonNetwork.Instantiate("Player", new Vector3(PhotonNetwork.LocalPlayer.ActorNumber, 0,0), Quaternion.identity);
        Health = myPlayer.GetComponent<Health>();
        playerMovement = myPlayer.GetComponent<PlayerMovement>();
        Health.deathEvent += Health_deathEvent;
        Health.killEvent += Health_killEvent;



        //PhotonNetwork.SerializationRate = 10;
        //PhotonNetwork.SendRate = 20;
    }

    private void Health_killEvent(object sender, System.EventArgs e)
    {
        kills++;
    }

    private void Health_deathEvent(object sender, System.EventArgs e)
    {
        deaths++;
    }
}
 