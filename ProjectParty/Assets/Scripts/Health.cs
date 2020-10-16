using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviourPunCallbacks
{
    public float health;
    public float maxHealth = 100;

    public GameObject blood;

    GameObject thatblood;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }
    
    public void takeDamage(float damage)
    {
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
        
        Debug.Log("Health: "+health);
        if(thatblood == null)
        {
            thatblood = Instantiate(blood, transform.position+transform.up*2, transform.rotation);
            Destroy(thatblood, 1f);
        }
        
        if (photonView.IsMine && this.health < 0) death();
    }

    void death()
    {
        photonView.RPC("updateDeath", RpcTarget.All, Vector3.zero);

    }
    IEnumerator Wait()
    {    
        yield return new WaitForSeconds(2);
        
    }
    [PunRPC]
    void updateDeath(Vector3 respawnPoint)
    {
        transform.position = respawnPoint;
        health = maxHealth;
        Debug.Log("Died");
    }
}
