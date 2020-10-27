using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.PlayerLoop;

public class CharacterController2D : MonoBehaviourPunCallback
{
	[SerializeField] bool playGround = false;
	public Animator animator;

	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = 0.6f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody m_Rigidbody;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	
	public Transform target;
	Camera camera;
    [SerializeField] private LayerMask mouseAimLayer;

    private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		camera = Camera.main;

        if (photonView.IsMine || playGround)
        {
			//target = new GameObject().transform;
			//rig.GetComponent<MultiAimConstraint>().
        }
	}


	private void FixedUpdate()
	{
		if (photonView.IsMine || playGround)
		{
			m_Grounded = false;

			//Get mouse pos on mousePointLayer




			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			RaycastHit[] colliders = Physics.RaycastAll(m_GroundCheck.position, -transform.up, 2 * k_GroundedRadius);
			//Debug.DrawRay(m_GroundCheck.position, -transform.up*k_GroundedRadius);
			//Debug.Log(colliders.Length);
			for (int i = 0; i < colliders.Length; i++)
			{
			
				if (colliders[i].collider.gameObject.layer != gameObject.layer)
					m_Grounded = true;
			}
		}
  //      else
  //      {
		//	m_Rigidbody2D.position = Vector3.MoveTowards(m_Rigidbody2D.position, networkPosition, Time.fixedDeltaTime);
		//	m_Rigidbody2D.rotation += (m_Rigidbody2D.rotation - networkRotation) * Time.fixedDeltaTime * 100.0f;
		//}
	}



	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;
			}

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (target.position.x > transform.position.x && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (target.position.x < transform.position.x && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			animator.SetTrigger("jump");
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody.AddForce(new Vector2(0f, m_JumpForce));
			
		}

		if (move != 0)
		{
			// set Backward
			animator.SetBool("isMoving", true);
		}
		else
		{
			// backward flase;
			animator.SetBool("isMoving", false);
		}
		animator.SetBool("grounded", m_Grounded);
		animator.SetBool("kneel", crouch);
		

	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Quaternion rot = Quaternion.identity;
		if (m_FacingRight) rot.y = 0;
		else rot.y = 180;
		transform.localRotation = rot;
		photonView.RPC("RPCFlip", RpcTarget.Others);
	}
	[PunRPC]
	void RPCFlip()
	{ // Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	//{
	//    if (stream.IsWriting)
	//    {
	//        stream.SendNext(m_Rigidbody2D.position);
	//        stream.SendNext(m_Rigidbody2D.rotation);
	//        stream.SendNext(m_Rigidbody2D.velocity);
	//    }
	//    else
	//    {
	//        networkPosition = (Vector3)stream.ReceiveNext();
	//        networkRotation = (float)stream.ReceiveNext();
	//        m_Rigidbody2D.velocity = (Vector3)stream.ReceiveNext();

	//        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
	//        networkPosition += (Vector3)(m_Rigidbody2D.velocity * lag);
	//    }
	//}

	void Update()
	{

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit, Mathf.Infinity, mouseAimLayer))
        {
			target.position = hit.point;
        }

		Debug.Log(Input.mousePosition);
		//Debug.Log(target.position);
		//Debug.DrawLine(transform.position, target.position);

		

		//Debug.Log(target.position);
	}


}
