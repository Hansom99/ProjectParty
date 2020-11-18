using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boomerang : MonoBehaviourPunCallbacks, Weapon
{
    // Boomerang Variablen

    public float maxShootDistance = 30f;
    public float force = 1000f;
    public float damage = 100f;
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


    // Interface Variablen

    private float shotsPerSecond = 1;
    public float ShotsPerSecond { get { return shotsPerSecond; } set { shotsPerSecond = value; } }
    private int ammunition = 1;
    public int Ammunition { get { return ammunition; } set { ammunition = value; } }


    // Funktionen

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
}
