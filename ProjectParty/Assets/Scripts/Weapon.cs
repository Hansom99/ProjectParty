using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public bool isSpikes = false;

    public bool isGun = false;

    public Gun gunScript;

    public float shotsPerSecound = 1;

    public int ammunition = 15;

    public int maxAmmu = 15;


    //bool cooledDown = true;
    float lastShot;

    // Start is called before the first frame update
    void Start()
    {

        lastShot = Time.time;
        if (gunScript != null) isGun = true;
    }

    public void attack()
    {

        if (isGun && (Time.time-lastShot)>=(1/shotsPerSecound) && ammunition >0)
        {
            gunScript.shoot();
            lastShot = Time.time;
            ammunition--;
        }
    }
    public void showShot(Vector3 endpoint)
    {
        if(isGun)gunScript.showShot(endpoint);
    }
   
    public void reload()
    {
        ammunition = maxAmmu;
    }

}
