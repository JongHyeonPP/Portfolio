using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using ItemSpace;
// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	public float walkSpeed = 0.15f;                 // Default walk speed.
	public float runSpeed = 0.8f;                   // Default run speed.
	public float sprintSpeed = 1.5f;                // Default sprint speed.
	public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.       // Default horizontal inertial force when jumping.

	private float aimWalkSpeed = 2f;
	private float speed, speedSeeker;               // Moving speed.                           // Animator variable related to jumping.
	public string jumpButton = "Jump";
	public float jumpHeight = 1.5f;
	private bool jump;
	public float jumpIntertialForce = 10f;
	public Animator ani;
	private bool haveMotion = false;
	private bool rolling = false;
	
	public GameObject pistol;
	public GameObject sword;
	public GameObject curweapon = null;

	[SerializeField]private RayShoot rayShoot;
	public Image[] weaponImage = new Image[2];
	//public GameObject curweapon;
	
	
	// Start is always called after any Awake functions.
	
	void Start()
	{
		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
		rayShoot = GetComponent<RayShoot>();
		speedSeeker = runSpeed;
	}

	// Update is used to set features regardless the active behaviour.
	void Update()
	{
		if (!jump && Input.GetButtonDown(jumpButton) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
		{
			jump = true;
		}
		
		if(AimBehaviourBasic.aim)
		{
		if (Input.GetKey(KeyCode.W))
		{
			transform.Translate(Vector3.forward*Time.deltaTime*aimWalkSpeed);
		}

		if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(Vector3.forward *-1* Time.deltaTime*aimWalkSpeed);
		}

		if (Input.GetKey(KeyCode.A))
		{
			transform.Translate(Vector3.right*-1 * Time.deltaTime*aimWalkSpeed);
		}

		if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector3.right * Time.deltaTime*aimWalkSpeed);
		}
		}


		if (Input.GetKeyDown(KeyCode.Alpha2) && !haveMotion)
		{
			if(curweapon==pistol)
				curweapon.SetActive(false);
			curweapon = sword;
			ani.SetBool("HavePistol", false);
			ani.SetBool("HaveSword", true);
			StartCoroutine(On(sword));
			rayShoot.enabled = false;
			weaponImage[0].enabled = false;
			weaponImage[1].enabled = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)&&!haveMotion)
		{
			if (curweapon == sword)
				curweapon.SetActive(false);
			curweapon = pistol;
			ani.SetBool("HaveSword", false);
			ani.SetBool("HavePistol", true);
			StartCoroutine(On(pistol));
			rayShoot.enabled = true;
			weaponImage[0].enabled = true;
			weaponImage[1].enabled = false;
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if(curweapon!=null)
			curweapon.SetActive(false);
			curweapon = null;
			ani.SetBool("HaveSword", false);
			ani.SetBool("HavePistol", false);
			Off(curweapon);
			rayShoot.enabled = false; 
			weaponImage[0].enabled = false;
			weaponImage[1].enabled = false;
		}

		if (Input.GetKeyDown(KeyCode.Space)&&!rolling)
		{
			if (!AimBehaviourBasic.aim)
			{
				ani.SetTrigger("Roll");
				StartCoroutine(Roll());
			}
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if(!haveMotion)
			{
				ani.SetTrigger("Reload");
				StartCoroutine(Reload());
			}
		}
		
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		MovementManagement(behaviourManager.GetH, behaviourManager.GetV);
	}
	

	// Execute the idle and walk/run jump movements.


	// Deal with the basic player movement
	void MovementManagement(float horizontal, float vertical)
	{
		// On ground, obey gravity.

		// Avoid takeoff when reached a slope end.


		// Call function that deals with player orientation.
		Rotating(horizontal, vertical);

		// Set proper speed.
		if (rolling)
		{
			speed = 3f;
			transform.Translate(Vector3.forward * speed * Time.deltaTime * 3);
			behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
			return;
		}
		Vector2 dir = new Vector2(horizontal, vertical);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
		// This is for PC only, gamepads control speed via analog stick.
		speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		speed *= speedSeeker;
		if (behaviourManager.IsSprinting())
		{
			speed = sprintSpeed;
		}
		transform.Translate(Vector3.forward * speed * Time.deltaTime*3);

		behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
	}


	

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}

	// Collision detection.
	private void OnCollisionStay(Collision collision)
	{
		// Slide on vertical obstacles
		if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
		{
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
		GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
	}
	
public void FixedUpdate()
	{
		if (curweapon == pistol && !AimBehaviourBasic.aim)
			return;
	if(Input.GetMouseButton(0)&&!Input.GetKey(KeyCode.LeftShift))
        {
			if (!haveMotion&&(curweapon==sword||curweapon==pistol))
			{
				
				if (speed == 0&&curweapon==sword)
					QuickRotating();
					StartCoroutine(Attack());

			}
		}
		
	}

	void QuickRotating()
	{
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Always rotates the player according to the camera horizontal rotation in aim mode.
		Quaternion targetRotation =  Quaternion.Euler(0, behaviourManager.GetCamScript.GetH, 0);

		// Rotate entire player to face camera.
		behaviourManager.SetLastDirection(forward);
		transform.rotation = targetRotation;

	}
	IEnumerator Attack()
    {
		ani.SetTrigger("Attack");
		haveMotion = true;
		sword.GetComponent<SwordAttack>().canAttack = true;
        yield return new WaitForSeconds(1.3f);
		sword.GetComponent<SwordAttack>().canAttack = false;
		haveMotion = false;
    }
	IEnumerator Reload()
	{
		haveMotion = true;
		yield return new WaitForSeconds(2.5f);
		haveMotion = false;
	}
	IEnumerator On(GameObject weapon)
	{
		yield return new WaitForSeconds(0.2f);
		if (weapon != null)
			weapon.SetActive(true);
		haveMotion = true;
			yield return new WaitForSeconds(1f);
		haveMotion = false;

	}
	void Off(GameObject weapon)
	{
		if (weapon != null)
			weapon.SetActive(false);

	}
	IEnumerator Roll()
	{
		rolling = haveMotion = true;
		speed = 2;
		yield return new WaitForSeconds(0.3f);
		speed = 1;
		yield return new WaitForSeconds(0.3f);
		speed = 0;
		rolling = haveMotion = false;

	}
    

}
