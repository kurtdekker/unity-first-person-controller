using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (BoxCollider))]

public class PlayerController : MonoBehaviour {
	
	public float walkSpeed = 6;
	public float runSpeed = 10;
	public float strafeSpeed = 5;
	public float gravity = 20;
	public float jumpHeight = 2;
	public bool canJump = true;
	private bool isRunning = false;
    private bool isGrounded = false;

    public bool IsRunning
    {
        get { return isRunning; }
    }

	void Awake () {
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}
	
	void FixedUpdate () {
		// get correct speed
		float forwardAndBackSpeed = walkSpeed;

        // if running, set run speed
		if (isRunning) {
			forwardAndBackSpeed = runSpeed;
		}

		// calculate how fast it should be moving
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") * strafeSpeed, 0, Input.GetAxis("Vertical") * forwardAndBackSpeed);
		targetVelocity = transform.TransformDirection(targetVelocity);
		
		// apply a force that attempts to reach our target velocity
		Vector3 velocity = rigidbody.velocity;
		Vector3 velocityChange = (targetVelocity - velocity);
		velocityChange.y = 0;
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
		
		// jump
		if (canJump && isGrounded && Input.GetButton("Jump")) {
			rigidbody.velocity = new Vector3(velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), velocity.z);
			isGrounded = false;
		}
		
		// apply gravity
		rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
	}

	void Update() {
        // check if the player is touching a surface below them
        checkGrounded();

        // check if the player is running
		if (isGrounded && Input.GetButtonDown("Sprint")) {
			isRunning = true;
		}
		
        // check if the player stops running
		if (Input.GetButtonUp("Sprint")) {
			isRunning = false;
		}
	}
	
	void checkGrounded() {
        /* ==============
         * REMEMBER
         * ==============
         * If you change the size of the prefab, you may have
         * to change the length of the ray to ensure it hits
         * the ground.
         * 
         * All obstacles/walls/floors must have rigidbodies
         * attached to them. If not, Unity physics may get
         * confused and the player can jump really high
         * when in a corner between 2 walls for example.
         */
        float rayLength = 0.7f;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        //Debug.DrawRay(ray.origin, ray.direction * rayLength);
        // if there is something directly below the player
        if (Physics.Raycast(ray, out hit, rayLength)) {
            isGrounded = true;
        }
	}


}
