using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPunCallbacks
{
    public Transform bulletSpawn;
    public float maxShootDistance = 20;

    public float bulletForce = 1000f;

    public GameObject gunShot;
    public GameObject bullet;

    public float damage = 25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shoot()
    {
        RaycastHit2D hits = Physics2D.Raycast(bulletSpawn.position, transform.root.localScale.x*bulletSpawn.right, maxShootDistance);
        Debug.DrawLine(bulletSpawn.position, bulletSpawn.position+ transform.root.localScale.x * bulletSpawn.right*maxShootDistance,Color.red);

        Vector3 endPoint = transform.root.localScale.x * bulletSpawn.right * maxShootDistance;

        if (hits.collider != null)
        {
            Debug.Log(hits.transform.tag);
            if(hits.transform.tag == "Player")
            {
                hits.transform.GetComponent<Health>().takeDamage(damage);
            }
            //endPoint = hits.point;
        }

        photonView.RPC("showShot", RpcTarget.All,endPoint);

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
}
