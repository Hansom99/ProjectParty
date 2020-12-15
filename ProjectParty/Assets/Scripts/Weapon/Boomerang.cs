using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// fires a boomerang that slows down and returns to player.
// Not yet completed
public class Boomerang : MonoBehaviourPunCallbacks, Weapon
{

    // Boomerang Variablen


    public float damage = 10f;
    public float boomerangSpeed = 50f;
    /// <summary>
    /// Distanz, die vom Boomerang zurückgelegt wurde.
    /// </summary>
    private float distanceCovered = 0.0f;
    private bool flying = false;
    private bool goingForward = false;
    /// <summary>
    /// Speicher Zeit des letzten Schusses.
    /// </summary>
    private float lastShot;
    /// <summary>
    /// Rigidbody of the boomerang
    /// </summary>
    private Rigidbody2D rb;
    Camera camera;
    /// <summary>
    /// Dort wo Boomerang in hand von player ist
    /// </summary>
    Vector2 origin;
    /// <summary>
    /// so viel links und rechts vom Boomerang werden Kollisionen detektiert.
    /// </summary>
    [SerializeField] private float collisionTolerance = 1f; 



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
        origin = this.transform.localPosition; // Speichert die Position der Hand ab, damit der Boomerang am Schluss wieder in der Hand des Players ist.
        Vector3 target = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 force = target - transform.position;  // Vector von player zu mouseposition.
        Vector3.Normalize(force);  // Force hat Länge 1
        rb.AddForce(force * boomerangSpeed);
        goingForward = true;
        flying = true;
    }


    /// <summary>
    /// Returned Rayacast, der Kollisionen links und rechts vom Boomerang detektiert.
    /// </summary>
    /// <returns></returns>
    RaycastHit2D BoomerangRaycastCreator()
    {
        Vector3 collisionToleranceVec = new Vector3(collisionTolerance, 0, 0);
        // Raycast, der in einer kleinen Toleranz Kollisionen links und rechts des Boomerangs detektiert.
        RaycastHit2D result = Physics2D.Raycast(this.transform.position - collisionToleranceVec, collisionToleranceVec, 2 * collisionTolerance);
        return result;
    }


    /// <summary>
    /// Detektiert Collisions mit Player und Terrain.
    /// Zieht Leben ab, wenn Player getroffen wird.
    /// Kommt zurück wenn Terrain getroffen wird.
    /// </summary>
    /// <param name="CollisionDetector"></param>
    void detectCollision(RaycastHit2D CollisionDetector)
    {
        if (CollisionDetector.collider != null)  // Falls etwas getroffen wurde
        {
            if (CollisionDetector.transform.tag == "Player") // Falls Spieler getroffen wird
            {
                CollisionDetector.transform.GetComponent<Health>().takeDamage(damage);      // Leben werden abgezogen
            }
            else  // Falls Terrain getroffen wird
            {
                rb.velocity = new Vector2(0, 0);  // Wenn Terrain getroffen wird, steht der Boomerang still
                goingForward = false; // Der Boomerang fliegt jetzt zurück
            }
        }

    }


    /// <summary>
    /// Falls Boomerang auf Rückweg und genug nahe am Player ist, hört er auf zu fliegen und geht in Hand des Players zurück.
    /// </summary>
    void chechWeatherBoomerangReturned()
    {
        if (!goingForward) // Falls Boomerang auf Rückweg ist
        {
            float distance = Vector2.Distance(this.transform.position, this.transform.root.position); // Distanz zwischen Spieler und Boomerang.
            if(distance < collisionTolerance)
            {
                flying = false; // Boomerang fliegt nicht mehr
                this.transform.localPosition = origin; // Boomerang geht in Hand von Player zurück
            }
        }
    }


    /// <summary>
    /// Gibt dem Boomerang eine kleine Kraft in Richtung Player zurück.
    /// setzt goingForward auf false Falls Boomerang richtung Player fliegt.
    /// </summary>
    void addBackwardsForce() 
    {
        Vector3 direction = this.transform.root.position - this.transform.position;
        Vector3.Normalize(direction);
        rb.AddForce(direction * (boomerangSpeed / 10));
        float angle = Vector2.Angle(direction, rb.velocity);
        if(angle < 90f)
        {
            goingForward = false;
        }
    }


    void FixedUpdate()
    {
        if (flying)
        {
            // Create Raycast around boomerang
            RaycastHit2D boomerangCollision = BoomerangRaycastCreator();

            // Detect Collisions with player and terrain.
            detectCollision(boomerangCollision);

            // add a backwards velocity to boomerang
            // detect weather boomerang is still flying forwards
            addBackwardsForce();
                        
            // Check waeather boomerang has reached us again.
            chechWeatherBoomerangReturned();
        }
    }



    // Interface Funktionen


    public void attack()
    {
        if (!flying)
        {
            shoot();
        }
    }

    public void reload()
    {
        return;
    }

    public void showShot(Vector3 endpoint)
    {
        throw new System.NotImplementedException();
    }
}
