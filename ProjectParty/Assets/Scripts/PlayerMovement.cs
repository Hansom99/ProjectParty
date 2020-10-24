using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks
{

	public CharacterController2D controller;

	public Animator animator;
	public GameObject shape;
	public Transform arm;
	public Transform armBone;

	//public GameObject[] weaponPrefabs;
	//Weapon[] weapons;
	public int selectedWeapon = 1;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	bool movefast = false;
	public bool attack = false;

	public int Ammo { get { return 0; } }

    private void Awake()
	{
		
		
		//List<Weapon> tmp = new List<Weapon>();
		//foreach (GameObject c in weaponPrefabs) { tmp.Add(c.GetComponent<Weapon>()); }
		//weapons = tmp.ToArray();
		
	}

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
		if (Input.GetButtonDown("Fire3")) movefast = true;
		else if (Input.GetButtonUp("Fire3")) movefast = false;
		animator.SetBool("runningFast", movefast);
		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}
        if (Input.GetButtonDown("Fire1"))
        {
			attack = true;
			Debug.Log("attack");
		}
		else if (Input.GetButtonUp("Fire1"))
        {
			animator.SetTrigger("attack");
			
			attack = false;
		}
        if (Input.GetButton("Reload"))
        {
			//weapons[selectedWeapon].reload();

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
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("jab")) horizontalMove = 0;

		if (movefast) horizontalMove *= 4;
		//Send to other Clients new Position
		//photonView.RPC("updatePosition", RpcTarget.Others, transform.position, horizontalMove * Time.fixedDeltaTime,crouch,jump);
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);

		jump = false;


		//animator.SetBool("jump", jump);
		
		//Debug.Log(animator.get);
		//if (attack) weapons[selectedWeapon].attack();
		//animator.SetBool("Attack", attack);
		//Debug.Log(animator.GetFloat("speed"));


	}
	void selectWeapon()
    {
		//if (!weapons[selectedWeapon].isGun) arm.parent = armBone;
    }
	




	[PunRPC]
	void updatePosition(Vector3 pos,float move,bool crouch,bool jump)
	{
		transform.position = pos;
		//this.move = true;
		
		
		controller.Move(move, crouch, jump);
	}

	[PunRPC]
	void showShot(Vector3 endpoint)
    {
		//weapons[selectedWeapon].showShot(endpoint);
    }
    
}
