using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boomerang : MonoBehaviourPunCallbacks, Weapon
{
    // Boomerang Variablen

    public float maxShootDistance = 30f;
    public float damage = 100f;
    public float boomerangSpeed = 50f;
    private bool inHand = true;
    [SerializeField] private GameObject flyingBoomerang;
    /// <summary>
    /// Dort fängt der Boomerang an zu fliegen
    /// </summary>
    [SerializeField] private Transform boomerangSpawn;
    /// <summary>
    /// Speicher Zeit des letzten Schusses.
    /// </summary>
    private float lastShot;
    /// <summary>
    /// Rigidbody of the boomerang
    /// </summary>
    private Rigidbody2D rb;

    Camera camera;

    // Interface Variablen

    private float shotsPerSecond = 1;
    public float ShotsPerSecond { get { return shotsPerSecond; } set { shotsPerSecond = value; } }
    private int ammunition = 1;
    public int Ammunition { get { return ammunition; } set { ammunition = value; } }


    // Funktionen

    void Start()
    {
        camera = Camera.main; 
        rb = GetComponent<Rigidbody2D>();  // Setzt rb zum rigidbody des Boomerangs
    }

    void shoot()
    {
        inHand = false;
        Vector3 target = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 force = target - transform.position;  // Vector von player zu mouseposition.
        Vector3.Normalize(force);  // Force hat Länge 1
        rb.AddForce(force);
    }


    // Interface Funktionen

    public void attack()
    {
        if (inHand)
        { 
            shoot();       
        }
    }

    public void reload()
    {
        throw new System.NotImplementedException();
    }

    public void showShot(Vector3 endpoint)
    {
        throw new System.NotImplementedException();
    }
}
