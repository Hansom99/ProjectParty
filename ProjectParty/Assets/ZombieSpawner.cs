using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ZombieSpawner : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnZombie", 0f, 1f);
    }

    void SpawnZombie()
    {
        PhotonNetwork.Instantiate("Zombie", transform.position, transform.rotation);
    }
}
