using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ShootFireBall : MonoBehaviourPunCallbacks, SpecialAttack , IOnEventCallback
{ 
    Animator animator;
    private float energy = 0;
    private bool isReady = false;
    float loadingSpeed = 10;

    private IEnumerator coroutine;

    [SerializeField] Transform hitPoint;
    private float damage = 80;
    private byte InstantiationEventCode;

    public float Energy { get { return energy; } }

    public bool IsReady { get { return isReady; } }

    public IEnumerator use()
    {
        
        isReady = false;
        energy = 0;
        int hashIdle = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        
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

    [PunRPC]
    void SpecialAttackRPC()
    {
        Debug.Log("easy");
        animator.SetTrigger("SpecialAttack");
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
                PhotonNetwork.RaiseEvent(1, new object[] { photonView.ViewID }, new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
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


    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == 1 && photonView.ViewID == (int)(((object[])photonEvent.CustomData)[0]))
        {
            
            animator.SetTrigger("SpecialAttack");
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
