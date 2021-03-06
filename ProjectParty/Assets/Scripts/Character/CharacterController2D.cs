using Photon.Pun;
using UnityEngine;

//     Handles movement form playerMovement.cs 
//     Uses Rigidbody2d to move Character (which is synchronized by Photon on all devices) 
public class CharacterController2D : MonoBehaviourPunCallback
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[SerializeField] public Transform target;


	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    private Vector3 velocity = Vector3.zero;
    

	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private bool climb;

    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		
	}
    private void Start()
    {
		animator = target.parent.GetComponent<Animator>();
	}

    private void FixedUpdate()
	{
		if (photonView.IsMine)
		{
			m_Grounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
					m_Grounded = true;
			}
		}
		if (Mathf.Abs(m_Rigidbody2D.velocity.x) >= 1) animator.SetBool("Walking", true);
		else animator.SetBool("Walking", false);
	}

	public void Climb(float moveUp)
    {
		if (!climb) return;

		Vector2 force = transform.up * (m_Rigidbody2D.mass*9.81f);

		m_Rigidbody2D.AddForce(force);

		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x,moveUp*10f);
		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);


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
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);


			

            // If the input is moving the player right and the player is facing left...
            if (target.position.x - transform.position.x > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (target.position.x - transform.position.x < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Ladder")
		{
			
			climb = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Ladder") climb = false;
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
}
