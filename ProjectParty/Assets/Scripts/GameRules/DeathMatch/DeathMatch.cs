using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathMatch : MonoBehaviourPunCallbacks
{
    NetworkManager manager;
    

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<NetworkManager>();
        manager.Health.deathEvent += Health_deathEvent;
    }

    private void Health_deathEvent(object sender, EventArgs e)
    {
        if (GlobalSettings.myTeam == "TeamA") GlobalSettings.PointsTeamA++;
        else GlobalSettings.PointsTeamB++;
        photonView.RPC("pointUpdate", RpcTarget.Others, GlobalSettings.PointsTeamA, GlobalSettings.PointsTeamA);
    }

    [PunRPC]
    void pointUpdate(int Pta,int Ptb)
    {
        GlobalSettings.PointsTeamA = Pta;
        GlobalSettings.PointsTeamB = Ptb;
    }
}
