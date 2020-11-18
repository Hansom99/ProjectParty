using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHandler : MonoBehaviourPunCallbacks
{
    GameObject skin;
    CharacterPrefabHandler cph;
    PhotonAnimatorView animatorView;

    public Transform skinPlace;
    public GameObject[] allCharacterPrefabs;

    bool isSet = false;


    public void setCharacter(int i)
    {

        photonView.RPC("addSkin", RpcTarget.All, i);

    }
    /// <summary>
    /// Ladet den Charakter aus dem Array allCharacterPrefabs und platziert in richtig und fügt die Waffen zum Charakter sodass sie richtig verhalten (drehen)
    /// </summary>
    /// <param name="i"></param>
    [PunRPC]
    void addSkin(int i)
    {
        GameObject oldSkin = null;
        if (isSet) oldSkin = skin;
        skin = Instantiate(allCharacterPrefabs[i]);
        skin.transform.parent = skinPlace.transform;
        skin.transform.localPosition = Vector3.zero;
        if (isSet) skin.transform.localScale = oldSkin.transform.localScale;
        if (i > 2) skin.transform.localScale = skin.transform.localScale * 1.5f; //wenn i>2 dann sind es Wraiths und die müssen etwas grösser sein (just for good looking)
        cph = skin.GetComponent<CharacterPrefabHandler>();

        animatorView = skin.AddComponent<PhotonAnimatorView>();
        animatorView.SetParameterSynchronized("Walking", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        

        GetComponent<CharacterController2D>().target = cph.Target.transform;
        GetComponent<PlayerMovement>().shape = skin;

        foreach(GameObject w in GetComponent<PlayerMovement>().weaponPrefabs)
        {
            w.transform.parent = cph.Hand.transform;
            w.transform.position = cph.Hand.transform.position;
            w.transform.rotation = Quaternion.Euler(0, 0, 0);
            Gun g = w.GetComponent<Gun>();
            if (g != null)
            {
                w.transform.rotation = Quaternion.Euler(0, 0, -13);
                w.transform.position +=  new Vector3(0.9f, 0, 0);
                g.target = cph.Target.transform;
            }

        }
        if (isSet) Destroy(oldSkin);
        isSet = true;


    }

}
