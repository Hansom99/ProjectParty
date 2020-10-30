using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootFireBall : MonoBehaviourPunCallbacks, SpecialAttack
{ 
    Animator animator;
    private float energy = 0;
    private bool isReady = false;
    float loadingSpeed = 10;

    private IEnumerator coroutine;

    [SerializeField] Transform hitPoint;
    private float damage = 80;

    public float Energy { get { return energy; } }

    public bool IsReady { get { return isReady; } }

    public IEnumerator use()
    {
        
        isReady = false;
        energy = 0;
        int hashIdle = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        animator.SetTrigger("SpecialAttack");
        yield return new WaitForSeconds(0.8f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, hitPoint.position - transform.position, (hitPoint.position - transform.position).magnitude);
        Debug.DrawLine(transform.position, hitPoint.position, Color.red, 1);
        Debug.Log(hits.Length);
        foreach (RaycastHit2D hit in hits)
        {
            
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player" && hit.collider.gameObject != transform.root.gameObject)
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
        if(isReady) Debug.Log("isReady");
    }



}
