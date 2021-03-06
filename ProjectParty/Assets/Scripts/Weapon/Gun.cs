﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// shoots bullets towards mouseposition with a certain range, speed and damage
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
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject gunShot;
    [SerializeField] private GameObject bullet;
    public Transform target;
    Camera camera;

    // Interface Variablen:
    private float shotsPerSecond = 2;
    public float ShotsPerSecond { get { return shotsPerSecond; } set { shotsPerSecond = value; } }
    private int ammunition;
    public int Ammunition { get { return ammunition; } set { ammunition = value; } }


    // Funktionen
    public void shoot()
    {
        Vector3 target = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hits = Physics2D.Raycast(bulletSpawn.position,-bulletSpawn.position+target, maxShootDistance);       // Raycast von der Waffe aus
        

        Vector2 endPoint = target; // Punkt bis zu dem hits geht.
        Debug.DrawLine(bulletSpawn.position, endPoint, Color.red, 1f);

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
        GlobalSettings.ammunition = ammunition;
        photonView.RPC("showShot", RpcTarget.All,new Vector3(endPoint.x,endPoint.y,0));

    }
    
    void Awake()
    {
        ammunition = maxAmmunition;
        lastShot = Time.time;
        GlobalSettings.ammunition = ammunition;
    }

    void Start()
    {
        camera = Camera.main;
    }


    // Interface Funktionen
    public void attack()
    {
        Debug.Log("attack");
        if(Time.time - lastShot >= 1 / shotsPerSecond && ammunition > 0)
        {
            shoot();
            lastShot = Time.time;
        }
    }
    public void showShot(Vector3 endPoint)
    {
        Debug.Log("ssssss");
        GameObject fire = Instantiate(gunShot, bulletSpawn.position, bulletSpawn.rotation);    
        fire.transform.localScale = transform.root.localScale;
        fire.transform.parent = transform;
        Destroy(fire, 0.25f);
        GameObject shot = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        shot.transform.localScale = transform.root.localScale;
        Vector3 speed = Vector3.zero;
        Vector3 target = camera.ScreenToWorldPoint(Input.mousePosition);
        shot.GetComponent<Rigidbody2D>().velocity = ((target-bulletSpawn.position).normalized * bulletForce);
        Destroy(shot, (endPoint - bulletSpawn.position).magnitude/bulletForce);

    }

    public void reload()
    {
        ammunition = maxAmmunition;               // Munition ist wieder voll
        lastShot = Time.time;                       // Man muss wieder kurz warten bis man schiessen kann.
        GlobalSettings.ammunition = ammunition;
    }
}
