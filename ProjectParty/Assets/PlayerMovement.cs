using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;

	Animator animator;
	public GameObject shape;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;

    private void Start()
    {
		animator = shape.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("jump", jump);
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
			animator.SetBool("attack", true);
			Debug.Log("attack");
		}
		else animator.SetBool("attack", false);

	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
		animator.SetBool("jump", jump);
		if (horizontalMove != 0) animator.SetBool("Running",true);
		else animator.SetBool("Running", false);
		//Debug.Log(animator.GetFloat("speed"));
		//animator.SetBool("attack", false);

	}

}
