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

    [PunRPC]
    void addSkin(int i)
    {
        GameObject oldSkin = null;
        if (isSet) oldSkin = skin;
        skin = Instantiate(allCharacterPrefabs[i]);
        skin.transform.parent = skinPlace.transform;
        skin.transform.localPosition = Vector3.zero;
        cph = skin.GetComponent<CharacterPrefabHandler>();

        animatorView = skin.AddComponent<PhotonAnimatorView>();
        animatorView.SetParameterSynchronized("Walking", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        

        GetComponent<CharacterController2D>().target = cph.Target.transform;
        GetComponent<PlayerMovement>().shape = skin;

        foreach(GameObject w in GetComponent<PlayerMovement>().weaponPrefabs)
        {
            w.transform.parent = cph.Hand.transform;
            w.transform.position = cph.Hand.transform.position + new Vector3(0.9f,0, 0);
            
            Gun g = w.GetComponent<Gun>();
            if (g != null)
            {
                w.transform.rotation = Quaternion.Euler(0, 0, -13);
                g.target = cph.Target.transform;
            }

        }
        if (isSet) Destroy(oldSkin);
        isSet = true;


    }

}
