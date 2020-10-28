﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPunCallbacks, Weapon
{ 
    // Gun Variablen:
    public int maxAmmunition = 3;
    public float maxShootDistance = 20;
    public float bulletForce = 1000f;
    public float damage = 25f;

    /// <summary>
    /// Speicher Zeit des letzten Schusses.
    /// </summary>
    private float lastShot;
    private Transform bulletSpawn;
    private GameObject gunShot;
    private GameObject bullet;

    // Interface Variablen:
    private float shotsPerSecond = 1;
    public float ShotsPerSecond { get { return shotsPerSecond; } set { shotsPerSecond = value; } }
    private int ammunition;
    public int Ammunition { get { return ammunition; } set { ammunition = value; } }


    // Funktionen
    public void shoot()
    {
        RaycastHit2D hits = Physics2D.Raycast(bulletSpawn.position, transform.root.localScale.x*bulletSpawn.right, maxShootDistance);       // Raycast von der Waffe aus
        Debug.DrawLine(bulletSpawn.position, bulletSpawn.position+ transform.root.localScale.x * bulletSpawn.right*maxShootDistance,Color.red);

        Vector2 endPoint = transform.root.localScale.x * bulletSpawn.right * maxShootDistance; // Punkt bis zu dem hits geht.

        if (hits.collider != null)
        {
            Debug.Log(hits.transform.tag);
            if(hits.transform.tag == "Player")      // falls ein Spieler getroffen wird
            {
                hits.transform.GetComponent<Health>().takeDamage(damage);      // Leben werden abgezogen
            }
            endPoint = hits.point;                           // Der Endpunkt wird dort gesetzt wo etwas getroffen wurde.
        }
        ammunition--;                                       // Es wird 1 Munition verbraucht
        photonView.RPC("showShot", RpcTarget.All,endPoint);

    }
    
    void Awake()
    {
        ammunition = maxAmmunition;
        lastShot = Time.time;
    }

    // Interface Funktionen
    public void attack()
    {
        if(Time.time - lastShot >= 1 / shotsPerSecond && ammunition > 0)
        {
            shoot();
            lastShot = Time.time;
        }
    }
    public void showShot(Vector3 endPoint)
    {
        GameObject fire = Instantiate(gunShot, bulletSpawn.position, bulletSpawn.rotation);    
        fire.transform.localScale = transform.root.localScale;
        fire.transform.parent = transform;
        Destroy(fire, 0.25f);
        GameObject shot = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        shot.transform.localScale = transform.root.localScale;
        Vector3 speed = Vector3.zero;
        shot.GetComponent<Rigidbody2D>().velocity = ((endPoint).normalized * bulletForce);
        Destroy(shot, (endPoint).magnitude/bulletForce);

    }

    public void reload()
    {
        ammunition = maxAmmunition;               // Munition ist wieder voll
        lastShot = Time.time;                       // Man muss wieder kurz warten bis man schiessen kann.
    }
}
