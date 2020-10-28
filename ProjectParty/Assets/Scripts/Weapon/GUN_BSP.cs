using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUN_BSP : MonoBehaviourPunCallback, Weapon
{


    private float shotsPerSecound;
    public float ShotsPerSecound { get { return shotsPerSecound; } set { shotsPerSecound = value; } }
    private int ammunition;
    public int Ammunition { get { return ammunition; } set { ammunition = value; } }

    public void attack()
    {
        throw new System.NotImplementedException();
    }

    public void reload()
    {
        throw new System.NotImplementedException();
    }

    public void showShot(Vector3 endpoint)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
