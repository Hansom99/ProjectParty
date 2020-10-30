using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject myPlayer;

    int myPlayerID;

    Health Health;
    PlayerMovement playerMovement;

    Timer respawnTimer;


    public Transform[] respawnPoints;


    public float health { get { return myPlayer.GetComponent<Health>().health; } }
    public float maxHealth { get { return myPlayer.GetComponent<Health>().maxHealth; } }
    public int Ammo { get { return playerMovement.Ammo; } }

    public int kills = 0;
    public int deaths = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        
        respawnTimer = gameObject.AddComponent<Timer>();
        respawnTimer.timerStop += RespawnTimer_timerStop;
        myPlayer =  PhotonNetwork.Instantiate(GlobalSettings.selectedCharacter, rndRespawnPoint(), Quaternion.identity);
        myPlayerID = myPlayer.GetComponent<PhotonView>().ViewID;
        myPlayer.GetComponent<SkinHandler>().setCharacter(PhotonNetwork.LocalPlayer.ActorNumber + 2);
        Health = myPlayer.GetComponent<Health>();
        playerMovement = myPlayer.GetComponent<PlayerMovement>();
        Health.deathEvent += Health_deathEvent;
        Health.killEvent += Health_killEvent;



        //PhotonNetwork.SerializationRate = 10;
        //PhotonNetwork.SendRate = 20;
    }

    private void RespawnTimer_timerStop(object sender, System.EventArgs e)
    {
        myPlayer.SetActive(true);

        photonView.RPC("respawn", RpcTarget.All, rndRespawnPoint(),myPlayerID);
    }

    [PunRPC]
    void respawn(Vector3 newPos,int id)
    {
        Debug.Log(id);
        GameObject player =  PhotonNetwork.GetPhotonView(id).gameObject;
        player.SetActive(true);

        player.transform.position = newPos;

        player.GetComponent<Health>().respawn();

    }


    public void startRespawnTimer()
    {
        respawnTimer.timeRemaining = 3;
        respawnTimer.start();
    }


    private void Health_killEvent(object sender, System.EventArgs e)
    {
        kills++;
    }

    private void Health_deathEvent(object sender, System.EventArgs e)
    {
        deaths++;
    }

    Vector3 rndRespawnPoint()
    {
        System.Random rnd = new System.Random();

        int i = rnd.Next(0, respawnPoints.Length);
        while(i >= respawnPoints.Length) i = rnd.Next(0, respawnPoints.Length);

        return respawnPoints[i].position;
    }

}
 