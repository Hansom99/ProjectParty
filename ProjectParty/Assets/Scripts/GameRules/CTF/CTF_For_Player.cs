using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class CTF_For_Player : MonoBehaviourPunCallbacks
{
    bool dropable = false;
    bool flagTaken = false;
    bool canTakeFlag = false;
    GameObject flagToTake;
    bool bringHome = false;

    CTF networkManager;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().deathEvent += CTF_For_Player_deathEvent;
        networkManager = GameObject.Find("NetworkManager").GetComponent<CTF>();
    }
    /// <summary>
    /// Diese Funtion wird aufgerufen wenn der Player stirb!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CTF_For_Player_deathEvent(object sender, System.EventArgs e)
    {
        dropFlag();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        //Spieler nimmt die Flagge
        if (canTakeFlag && Input.GetButtonDown("Take")) 
        {
            flagToTake.transform.parent = transform;
            flagTaken = true;
            
            canTakeFlag = false;
            photonView.RPC("takeFlag", RpcTarget.Others, flagToTake.layer);
            
        }
        //spieler dropt Falgge
        else if(flagTaken && Input.GetButtonDown("Take"))
        {
            
            Debug.Log("drop");
            photonView.RPC("dropFlagRpc", RpcTarget.All);
        }
    }


    void dropFlag()
    {

        flagToTake.transform.parent = null;
        flagToTake = null;
        flagTaken = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;
        //Der Spieler ist bei der Flagge und can sie nemen
        if (collision.gameObject.tag == "Flag")
        {
            canTakeFlag = true; 
            flagToTake = collision.gameObject;
            Debug.Log("TakeFlag");
            
        }
        // spielen bring flagge zu seiner Base
        if (flagTaken && collision.gameObject.tag == "TeamBase" && collision.gameObject.layer == gameObject.layer)
        {
            photonView.RPC("dropFlagRpc", RpcTarget.All);
            Debug.Log("bring home;");
            networkManager.broughtFlagHome();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;
        if (!flagTaken && collision.gameObject.tag == "Flag")
        {
            Debug.Log("walk away");
            canTakeFlag = false; //Der Spieler geht von der Flagge weg und kann sie nicht mehr nehmen 
            flagToTake = null;
        }
    }

    [PunRPC]
    void takeFlag(int FlagLayer)
    {
        if (networkManager.FlagA.layer == FlagLayer) networkManager.FlagA.transform.parent = transform;
        else networkManager.FlagB.transform.parent = transform;
    }
    [PunRPC]
    void dropFlagRpc()
    {
        dropFlag();
    }
}
