using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float demagePerFrame = 10f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        collision.gameObject.GetComponent<Health>().takeDamage(demagePerFrame);
    }
}

