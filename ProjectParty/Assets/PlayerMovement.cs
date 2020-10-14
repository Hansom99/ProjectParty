using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks {

	public CharacterController2D controller;

	//Animator animator;
	//public GameObject shape;



	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	bool move = false;

	Vector3 targetPos;
	Vector3 zero = Vector3.zero;
	float smooth = 0.9f;
    private float speed = 1;

    private void Start()
    {
		//animator = shape.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

		if (!photonView.IsMine) return;

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			//animator.SetBool("jump", jump);
			
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}
        if (Input.GetMouseButton(0))
        {
			//animator.SetBool("attack", true);
			Debug.Log("attack");
		}
		//else animator.SetBool("attack", false);

	}

	void FixedUpdate ()
	{
		if (!photonView.IsMine) {
			//transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref zero, smooth);
			
			return;
		} 
		// Move this character
		

		//Send to other Clients new Position
		photonView.RPC("updatePosition", RpcTarget.Others, transform.position, horizontalMove * Time.fixedDeltaTime,crouch,jump);
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);

		jump = false;


		//animator.SetBool("jump", jump);
		//if (horizontalMove != 0) animator.SetBool("Running",true);
		//else animator.SetBool("Running", false);
		//Debug.Log(animator.GetFloat("speed"));
		//animator.SetBool("attack", false);

	}


	[PunRPC]
	void updatePosition(Vector3 pos,float move,bool crouch,bool jump)
	{
		transform.position = pos;
		//this.move = true;
		
		
		controller.Move(move, crouch, jump);
	}

}
