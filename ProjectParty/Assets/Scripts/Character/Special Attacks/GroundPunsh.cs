﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPunsh : MonoBehaviourPunCallbacks, SpecialAttack
{
    Animator animator;
    private float energy = 0;
    private bool isReady = false;
    float loadingSpeed = 10;
    float hitRadius = 4;

    private IEnumerator coroutine;

    [SerializeField] Transform hitPoint;
    private float damage = 80;

    public float Energy { get { return energy; } }

    public bool IsReady { get { return isReady; } }

    public IEnumerator use()
    {
        Debug.Log("isReady");
        isReady = false;
        energy = 0;
        int hashIdle = animator.GetCurrentAnimatorStateInfo(0).fullPathHash; 
        animator.SetTrigger("SpecialAttack");
        yield return new WaitForSeconds(0.8f);
        PhotonNetwork.Instantiate("DustInAir", hitPoint.position, hitPoint.rotation);
        RaycastHit2D[] hits= Physics2D.BoxCastAll(hitPoint.position, new Vector2(4, 1), 0, hitPoint.right);

        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Player" && hit.collider.gameObject != transform.root.gameObject)
                {
                    hit.collider.GetComponent<Health>().takeDamage(damage);
                }
            }
        }

    }




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (isReady)
        {
            if (Input.GetKeyDown("g"))
            {
                
                coroutine = use();
                StartCoroutine(coroutine);
            }

            return;
        }
        if (energy < 100)
        {
            energy += Time.deltaTime * loadingSpeed;

        }
        else isReady = true;
        Debug.Log(energy);
    }



}