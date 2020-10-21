using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPunCallbacks, Weapon
{
    public Transform bulletSpawn;
    public float maxShootDistance = 20;

    public float bulletForce = 1000f;

    public GameObject gunShot;
    public GameObject bullet;

    public float damage = 25f;
    private float lastShot;

    public bool isSpikes { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isGun { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Gun gunScript { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float shotsPerSecound { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int ammunition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int maxAmmu { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
        if (gunScript != null) isGun = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Shot Animationen auf jedem bildschirm aufgerufen vom Server
    /// </summary>
    /// <param name="endpoint"></param>

    public void showShot(Vector3 endpoint)
    {
        if (!isGun) return;

        GameObject fire = Instantiate(gunShot, bulletSpawn.position, bulletSpawn.rotation);
        fire.transform.localScale = transform.root.localScale;
        fire.transform.parent = transform;
        Destroy(fire, 0.25f);
        GameObject shot = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        shot.transform.localScale = transform.root.localScale;
        Vector3 speed = Vector3.zero;
        shot.GetComponent<Rigidbody2D>().velocity = ((endpoint).normalized * bulletForce);
        Destroy(shot, (endpoint).magnitude / bulletForce);
    }

    public void reload()
    {
        ammunition = maxAmmu;
    }

    public void attack()
    {

        if (isGun && (Time.time - lastShot) >= (1 / shotsPerSecound) && ammunition > 0)
        {
            shoot();
            lastShot = Time.time;
            ammunition--;
        }
    }

    public void shoot()
    {
        RaycastHit2D hits = Physics2D.Raycast(bulletSpawn.position, transform.root.localScale.x * bulletSpawn.right, maxShootDistance);
        Debug.DrawLine(bulletSpawn.position, bulletSpawn.position + transform.root.localScale.x * bulletSpawn.right * maxShootDistance, Color.red);

        Vector3 endPoint = transform.root.localScale.x * bulletSpawn.right * maxShootDistance;

        if (hits.collider != null)
        {
            Debug.Log(hits.transform.tag);
            if (hits.transform.tag == "Player")
            {
                hits.transform.GetComponent<Health>().takeDamage(damage);
            }
            //endPoint = hits.point;
        }

        photonView.RPC("showShot", RpcTarget.All, endPoint);

    }

}
