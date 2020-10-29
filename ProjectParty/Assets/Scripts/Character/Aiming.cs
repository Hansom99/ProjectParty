using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using TreeEditor;

public class Aiming : MonoBehaviourPunCallbacks
{


    [SerializeField] bool isAiming = true;
    [SerializeField] Transform target;


    LineRenderer aimLine;
    CharacterController2D characterController;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {

        if (!photonView.IsMine) return;
        camera = Camera.main;
        characterController = transform.root.GetComponent<CharacterController2D>();
        aimLine = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAiming || !photonView.IsMine) return;


        
        target.position = camera.ScreenToWorldPoint(Input.mousePosition);
        
        var pos = camera.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        aimLine.SetPosition(0, new Vector3(transform.position.x, transform.position.y));
        aimLine.SetPosition(1, new Vector3(target.position.x,target.position.y));

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (!characterController.m_FacingRight) angle += 180;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        

    }

    [PunRPC]
    void updateRot()
    {

    }

}
