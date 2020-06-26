using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[Header("Horizontal Movement")]
	public int runSpeed = 2;

	Animator animator;

	[Header("Vertical Movement")]
	public float jumpSpeed = 15f;

	[Header("Components")]
	public Rigidbody2D rb;
	public LayerMask groundLayer;

	[Header("Collision")]
	public bool onGround = false;
	public bool isLanding = false;
	public float groundLength = 0.6f;
	public float landLength = 0.9f;

	// Start is called before the first frame update
	void Start()
	{
		animator = gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{	
		onGround = Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer);
		isLanding = Physics2D.Raycast(transform.position, Vector2.down, landLength, groundLayer);

		// Jump
		if (Input.GetButtonDown("Jump") && onGround) 
		{
			Jump();
		}

		// Landing
		if (isLanding && !onGround)
		{
			Debug.Log("landed");
			animator.SetBool("jump", false);
			animator.SetBool("land", true);
		}

		// Landed
		if (onGround && isLanding) 
		{
			Debug.Log("landed");
			animator.SetBool("land", false);
		}
		
		// Move Right
		if (Input.GetAxisRaw("Horizontal") == 1)
		{
			MoveRight();
		}
		// Move Left
		else if (Input.GetAxisRaw("Horizontal") == -1)
		{
			MoveLeft();
		}
		// Idle
		else
		{
			animator.SetBool("run", false);
		}
	}

	void Jump(){
		animator.SetBool("jump", true);
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
	}

	void MoveRight()
	{
		Move(MoveDirection.Right);	
	}

	void MoveLeft()
	{
		Move(MoveDirection.Left);
	}

	void Move(MoveDirection direction)
	{
		int factor = 1;

		switch (direction) 
		{
			case MoveDirection.Left:
				factor = -1;
				break;
			case MoveDirection.Right:
				factor = 1;
				break;
		}
		
		animator.SetBool("run", true);
		gameObject.transform.position += (factor * transform.right) * Time.deltaTime * runSpeed;
		var playerScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3(factor * Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);
	}

	void Land()
	{
		animator.SetBool("jump", false);
		animator.SetBool("land", true);
	}

	enum MoveDirection 
	{
		Left,
		Right
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLength);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position + (Vector3.right * 0.1f), transform.position + (Vector3.right * 0.1f) + (Vector3.down * landLength));
	}
}
