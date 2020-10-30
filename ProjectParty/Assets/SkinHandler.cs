using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHandler : MonoBehaviourPunCallbacks
{
    GameObject skin;
    CharacterPrefabHandler cph;

    public Transform skinPlace;
    public GameObject[] allCharacterPrefabs;
    



    public void setCharacter(int i)
    {

        photonView.RPC("addSkin", RpcTarget.All, i);

    }

    [PunRPC]
    void addSkin(int i)
    {
        skin = Instantiate(allCharacterPrefabs[i]);
        skin.transform.parent = skinPlace.transform;
        skin.transform.localPosition = Vector3.zero;
        cph = skin.GetComponent<CharacterPrefabHandler>();

        GetComponent<CharacterController2D>().target = cph.Target.transform;
        GetComponent<PlayerMovement>().shape = skin;

        foreach(GameObject w in GetComponent<PlayerMovement>().weaponPrefabs)
        {
            w.transform.parent = cph.Hand.transform;
            w.transform.position = cph.Hand.transform.position + new Vector3(0.9f,0, 0);
            Gun g = w.GetComponent<Gun>();
            if (g != null) g.target = cph.Target.transform;

        }


    }

}
