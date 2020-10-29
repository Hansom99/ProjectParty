using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviourPunCallbacks
{
    public float health;
    public float maxHealth = 100;
    


    HealthBar healthbar;
    public GameObject HealthbarPrefab;
    public GameObject blood;
    GameObject thatblood;
    Transform[] respawnPoints;

    bool alive = true;

    NetworkManager networkManager;


    public event EventHandler deathEvent;
    public event EventHandler killEvent;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        healthbar = HealthbarPrefab.GetComponent<HealthBar>();
        health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        healthbar.SetHealth(maxHealth);
    }
    
    public void takeDamage(float damage)
    {
        if (!alive) return;
        health -= damage;
        Debug.Log("damage");
        photonView.RPC("updateHealth", RpcTarget.All, this.health);
        

    }
    public void heal(float health)
    {
        this.health += health;
        photonView.RPC("updateHealth", RpcTarget.All, this.health);
    }

    [PunRPC]
    void updateHealth(float health)
    {
        
        this.health = health;
        healthbar.SetHealth(this.health);
        
        Debug.Log("Health: "+health);
        if(thatblood == null)
        {
            thatblood = Instantiate(blood, transform.position+transform.up*2, transform.rotation);
            Destroy(thatblood, 1f);
        }
        
        if (photonView.IsMine && this.health <= 0) death();
    }

    void death()
    {

        photonView.RPC("updateDeath", RpcTarget.All);
        if (photonView.IsMine) deathEvent?.Invoke(this, EventArgs.Empty);
        else killEvent?.Invoke(this, EventArgs.Empty);
    }

    public void respawn()
    {
        health = maxHealth;
        healthbar.SetHealth(this.health);
        alive = true;
    }
    [PunRPC]
    void updateDeath()
    {
        alive = false;
        Debug.Log("Died");
        if (photonView.IsMine)
        {
            networkManager.startRespawnTimer();
        }
        gameObject.SetActive(false);

    }





}
