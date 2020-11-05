using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject networkManager;

    [SerializeField] Transform player;
    [SerializeField] private Vector3 offSet;

    public float FollowSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        player = networkManager.GetComponent<NetworkManager>().myPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) Start();
        Vector3 newPosition = player.position + offSet;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}
