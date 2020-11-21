using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviourPunCallbacks
{
    public bool isPlayer = true;
    public float health;
    public float maxHealth = 100;
    public bool healing = true;
    

    public GameObject HealthbarPrefab;
    public GameObject blood;

    HealthBar healthbar;
    GameObject thatblood;
    Transform[] respawnPoints;

    bool alive = true;
    private float lastHitTime;

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
        if (!isPlayer) HealthbarPrefab.SetActive(false);
    }
    
    public void takeDamage(float damage)
    {
        if(!isPlayer) HealthbarPrefab.SetActive(true);
        if (!alive) return;
        health -= damage;
        this.lastHitTime = Time.time;
        photonView.RPC("updateHealth", RpcTarget.All, this.health, true);
        

    }
    public void heal(float health)
    {
        if(this.health < maxHealth)
        {
            if((this.health + health) > maxHealth) //falls schon fast bei maxhealth setze health auf max
            {
                this.health = maxHealth;
                return;
            }
            this.health += health;
            photonView.RPC("updateHealth", RpcTarget.All, this.health, false);

        }
        
    }

    private void FixedUpdate() //healen nach bestimmter zeit ohne schaden
    {
        if(!photonView.IsMine || !healing)
        {
            return;
        }
        float noDamageTime = Time.time - this.lastHitTime;
        if (noDamageTime > 5f && this.health < maxHealth)
        {
            heal(5*Time.fixedDeltaTime);
        }
    }

    [PunRPC]
    void updateHealth(float health, bool takeDamage)
    {
        
        this.health = health;
        if(takeDamage) this.lastHitTime = Time.time;


        healthbar.SetHealth(this.health);
        Debug.Log("Health: "+health);
        if(thatblood == null && takeDamage)
        {
            thatblood = Instantiate(blood, transform.position+transform.up*2, transform.rotation);
            Destroy(thatblood, 1f);
        }
        
        if ((photonView.IsMine || !isPlayer )&& this.health <= 0) death();
    }

    void death()
    {

        photonView.RPC("updateDeath", RpcTarget.All);
        if (photonView.IsMine && isPlayer) deathEvent?.Invoke(this, EventArgs.Empty);
        else killEvent?.Invoke(this, EventArgs.Empty);

        if (!isPlayer) GlobalSettings.kills++;
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
        if (!isPlayer)
        {
            Destroy(this.gameObject);
            return;
        }


        alive = false;
        Debug.Log("Died");
        if (photonView.IsMine)
        {
            networkManager.startRespawnTimer();
        }
        gameObject.SetActive(false);

    }





}
