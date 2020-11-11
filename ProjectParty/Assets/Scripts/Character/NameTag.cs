using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NameTag : MonoBehaviourPunCallback
{

    public Text nameTag;

    private void Start()
    {
        setName();
    }
    void setName()
    {
        nameTag.text = photonView.Owner.NickName;
    }
}
