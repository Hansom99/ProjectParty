using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] bool playGround = false;
    [SerializeField]GameObject player;

    [SerializeField] GameObject networkManager;

    [SerializeField] Vector3 cameraOfset;

    void Start()
    {
        if(!playGround) player = networkManager.GetComponent<NetworkManager>().myPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + cameraOfset;
    }
}
