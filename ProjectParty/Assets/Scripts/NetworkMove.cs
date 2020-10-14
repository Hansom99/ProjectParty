using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class NetworkMove : MonoBehaviourPunCallbacks
{
    private float speed =10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey("w"))
            {
                transform.Translate(transform.right * speed*Time.deltaTime);
                photonView.RPC("updatePosition", RpcTarget.Others, transform.position);
            }
        }
    }

    [PunRPC]
    void updatePosition(Vector3 pos)
    {
        transform.position= pos;
    }

}
