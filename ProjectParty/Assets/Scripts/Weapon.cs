using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isSpikes = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Player" && !isSpikes)
        {
            //Debug.Log(collision.gameObject.tag);
            if (!gameObject.transform.root.GetComponent<PlayerMovement>().attack) return;
            collision.GetComponent<Health>().takeDamage(10);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isSpikes)
           {
                //Debug.Log(collision.gameObject.tag);
                
                collision.GetComponent<Health>().takeDamage(10);
            }
        
    }
}
