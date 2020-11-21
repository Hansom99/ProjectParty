using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ZombieAI : MonoBehaviour
{
    public Transform Target;
    public float speed = 50f;

    public float climbSpeedScale = 0.5f;

    public float demagePerSecound = 2;
    public float maxHitDis = 2f;

    //public GameObject UI;

    Path path;
    int currentWayPoint = 0;
    private Vector3 velocity = Vector3.zero;

    Seeker seeker;

    Rigidbody2D rb;
    private float m_MovementSmoothing = .05f;
    private bool climb;


    


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        

    }

    void UpdatePath()
    {
        //suche nächsten Spieler
        PlayerMovement[] playerMovements = FindObjectsOfType<PlayerMovement>();
        if (playerMovements.Length == 0) return;
        float mindis = 100000f;
        foreach(PlayerMovement p in playerMovements)
        {
            if (mindis > Vector2.Distance(transform.position, p.transform.position)) Target = p.transform;
        }

        seeker.StartPath(transform.position, Target.position, OnPathFound); //sucht schnellsten Weg mit A* algo
    }


    void OnPathFound(Path p)
    {
       // Debug.LogWarning("Found Paht");

        if (p.error)
        {
            Debug.LogError(p.errorLog);
            return;
        }
        path = p;
        currentWayPoint = 0;
    }




    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (path == null) return;

        if (Vector2.Distance(transform.position, Target.position) < maxHitDis) Target.GetComponent<Health>().takeDamage(demagePerSecound*Time.fixedDeltaTime);

        if (currentWayPoint >= path.vectorPath.Count) return;

        Vector2 dir = ((Vector2)path.vectorPath[currentWayPoint] -  (Vector2)transform.position).normalized;


        Vector3 targetVelocity = new Vector2(dir.x * speed, rb.velocity.y);
        // And then smoothing it out and applying it to the character
        if (climb) {
            Vector2 force = transform.up * (rb.mass * 9.81f);

            rb.AddForce(force);
            // Move the character by finding the target velocity
            targetVelocity.y = dir.y * speed * climbSpeedScale;
        }

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        float dist = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);

        if (dist < 1f) currentWayPoint++;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {

            climb = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder") climb = false;
    }

}
