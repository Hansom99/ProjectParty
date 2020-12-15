using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//     Loading energy  
//     When G then play Animation 
public class GroundPunsh : MonoBehaviourPunCallbacks, SpecialAttack , IOnEventCallback
{
    Animator animator;
    private float energy = 0;
    private bool isReady = false;
    float loadingSpeed = 10;
    float hitRadius = 4;

    private IEnumerator coroutine;

    [SerializeField] Transform hitPoint;
    private float damage = 100;

    public float Energy { get { return energy; } }

    public bool IsReady { get { return isReady; } }

    public IEnumerator use()
    {
        Debug.Log("isReady");
        isReady = false;
        energy = 0;
        int hashIdle = animator.GetCurrentAnimatorStateInfo(0).fullPathHash; 
       // animator.SetTrigger("SpecialAttack");
        yield return new WaitForSeconds(0.8f);
        PhotonNetwork.Instantiate("DustInAir", hitPoint.position, hitPoint.rotation);
        RaycastHit2D[] hits= Physics2D.BoxCastAll(hitPoint.position, new Vector2(20, 20), 0, hitPoint.right);

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
                PhotonNetwork.RaiseEvent(2, new object[] { photonView.ViewID }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                coroutine = use();
                StartCoroutine(coroutine);
            }

            return;
        }
        if (energy < 100)
        {
            energy += Time.deltaTime * loadingSpeed;
            GlobalSettings.specialAttackEnergy = energy;

        }
        else isReady = true;
      //  Debug.Log(energy);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == 2 && photonView.ViewID == (int)(((object[])photonEvent.CustomData)[0]))
        {

            animator.SetTrigger("SpecialAttack");
        }
    }

}