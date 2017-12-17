using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterMotorC))]

public class PlayerInputControllerC : MonoBehaviour {
	
	private GameObject mainModel;
	public float walkSpeed = 6.0f;
	public float sprintSpeed = 12.0f;
	public bool canSprint = true;
	private bool sprint = false;
	private bool recover = false;
	private float staminaRecover = 1.4f;
	private float useStamina = 0.04f;
	[HideInInspector]
		public bool  dodging = false;
	
	public Texture2D staminaGauge;
	public Texture2D staminaBorder;
	
	public float maxStamina = 100.0f;
	public float stamina = 100.0f;
	
	private float lastTime = 0.0f;
	private float recoverStamina = 0.0f;
	private Vector3 dir = Vector3.forward;

	public bool doubleJump = false;
	private bool airJump = false;
	private bool airMove = false;
	
	private bool useMecanim = true;

	[System.Serializable]
	public class DodgeSetting{
		public bool  canDodgeRoll = false;
		public int staminaUse = 25;
		
		public AnimationClip dodgeForward;
		public AnimationClip dodgeLeft;
		public AnimationClip dodgeRight;
		public AnimationClip dodgeBack;
	}
	public DodgeSetting dodgeRollSetting;
	
	private CharacterMotorC motor;
	private CharacterController controller;
    // 初始化
    void Start(){
		motor = GetComponent<CharacterMotorC>();
		controller = GetComponent<CharacterController>();
		stamina = maxStamina;
		if(!mainModel){
			mainModel = GetComponent<StatusC>().mainModel;
		}
		useMecanim = GetComponent<AttackTriggerC>().useMecanim;
	}
    // 每帧更新一次
    void Update(){
		StatusC stat = GetComponent<StatusC>();
		if(recover && !sprint && !dodging){
			if(recoverStamina >= staminaRecover){
				StaminaRecovery();
			}else{
				recoverStamina += Time.deltaTime;
			}
		}
		
		if(stat.freeze || GlobalConditionC.freezeAll || GlobalConditionC.freezePlayer){
			motor.inputMoveDirection = new Vector3(0,0,0);
			return;
		}
		if(Time.timeScale == 0.0f){
			return;
		}
		if (dodging){
			Vector3 fwd = transform.TransformDirection(dir);
			controller.Move(fwd * 8 * Time.deltaTime);
			return;
		}
		
		if(dodgeRollSetting.canDodgeRoll){
            //往前躲闪
            if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0 && (controller.collisionFlags & CollisionFlags.Below) != 0 && Input.GetAxis("Horizontal") == 0){
				if(Input.GetButtonDown ("Vertical") && (Time.time - lastTime) < 0.4f && Input.GetButtonDown ("Vertical") && (Time.time - lastTime) > 0.1f && Input.GetAxis("Vertical") > 0.03f){
					lastTime = Time.time;
					dir = Vector3.forward;
					StartCoroutine(DodgeRoll(dodgeRollSetting.dodgeForward));
				}else
					lastTime = Time.time;
			}
            //向后躲闪
            if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0 && (controller.collisionFlags & CollisionFlags.Below) != 0 && Input.GetAxis("Horizontal") == 0){
				if(Input.GetButtonDown ("Vertical") && (Time.time - lastTime) < 0.4f && Input.GetButtonDown ("Vertical") && (Time.time - lastTime) > 0.1f && Input.GetAxis("Vertical") < -0.03f){
					lastTime = Time.time;
					dir = Vector3.back;
					StartCoroutine(DodgeRoll(dodgeRollSetting.dodgeBack));
				}else
					lastTime = Time.time;
			}
            //向左躲闪
            if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0 && (controller.collisionFlags & CollisionFlags.Below) != 0 && !Input.GetButton("Vertical")){
				if(Input.GetButtonDown ("Horizontal") && (Time.time - lastTime) < 0.3f && Input.GetButtonDown ("Horizontal") && (Time.time - lastTime) > 0.15f && Input.GetAxis("Horizontal") < -0.03f){
					lastTime = Time.time;
					dir = Vector3.left;
					StartCoroutine(DodgeRoll(dodgeRollSetting.dodgeLeft));
				}else
					lastTime = Time.time;
			}
            //向右躲闪
            if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0 && (controller.collisionFlags & CollisionFlags.Below) != 0 && !Input.GetButton("Vertical")){
				if(Input.GetButtonDown ("Horizontal") && (Time.time - lastTime) < 0.3f && Input.GetButtonDown ("Horizontal") && (Time.time - lastTime) > 0.15f && Input.GetAxis("Horizontal") > 0.03f){
					lastTime = Time.time;
					dir = Vector3.right;
					StartCoroutine(DodgeRoll(dodgeRollSetting.dodgeRight));
				}else
					lastTime = Time.time;
			}
		}

        //取消冲刺
        if (sprint && Input.GetAxis("Vertical") < 0.02f || sprint && stamina <= 0 || sprint && Input.GetButtonDown("Fire1") || sprint && Input.GetKeyUp(KeyCode.LeftShift)){
			sprint = false;
			recover = true;
			motor.movement.maxForwardSpeed = walkSpeed;
			motor.movement.maxSidewaysSpeed = walkSpeed;
			recoverStamina = 0.0f;
		}

		if(airJump){
			Vector3 aj = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal") , 2 , Input.GetAxis("Vertical")));
			controller.Move(aj * 4 * Time.deltaTime);
			return;
		}

		if(Input.GetButtonDown("Jump") && !motor.grounded && doubleJump || Input.GetButtonDown("Jump") && !motor.grounded && stat.hiddenStatus.doubleJump){
			StartCoroutine(DoubleJumping());
		}
		
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		if (directionVector != Vector3.zero) {
			float directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			directionLength = Mathf.Min(1, directionLength);
			
			directionLength = directionLength * directionLength;
			directionVector = directionVector * directionLength;
		}
		

		motor.inputMoveDirection = transform.rotation * directionVector;
		motor.inputJump = Input.GetButton("Jump");
		
		if (sprint){
			motor.movement.maxForwardSpeed = sprintSpeed;
			motor.movement.maxSidewaysSpeed = sprintSpeed;
			return;
		}

		if(Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && (controller.collisionFlags & CollisionFlags.Below) != 0 && canSprint && stamina > 0){
			sprint = true;
			StartCoroutine(Dasher());
		}
	}
	
	void OnGUI(){
		if(sprint || recover || dodging){
			float staminaPercent = stamina * 100 / maxStamina *3;
			GUI.DrawTexture ( new Rect((Screen.width /2) -150,Screen.height - 120, staminaPercent ,10), staminaGauge);
			GUI.DrawTexture ( new Rect((Screen.width /2) -153,Screen.height - 123, 306 ,16), staminaBorder);
		}
		
	}

	IEnumerator DoubleJumping(){
		if(!airMove){
			airMove = true;
			airJump = true;
			if(!useMecanim){
				GetComponent<PlayerAnimationC>().DoubleJumpAnimation();
			}
			motor.freezeGravity = true;
			yield return new WaitForSeconds(0.25f);
			motor.freezeGravity = false;
			airJump = false;
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit col){
		CharacterController controller = GetComponent<CharacterController>();
		if(airMove && (controller.collisionFlags & CollisionFlags.Below) != 0){
			airMove = false;
			motor.freezeGravity = false;
		}
	}
	
	IEnumerator Dasher(){
		while (sprint){
			yield return new WaitForSeconds(useStamina);
			if(stamina > 0){
				stamina -= 1;
			}else{
				stamina = 0;
			}
		}
	}
	
	void StaminaRecovery(){
		stamina += 1;
		if(stamina >= maxStamina){
			stamina = maxStamina;
			recoverStamina = 0.0f;
			recover = false;
		}else{
			recoverStamina = staminaRecover - 0.02f;
		}
	}
	
	IEnumerator DodgeRoll(AnimationClip anim){
		if(stamina < 25 || dodging || !motor.canControl){
			yield return false;
		}
		if(!useMecanim){
			mainModel.GetComponent<Animation>()[anim.name].layer = 18;
			mainModel.GetComponent<Animation>().PlayQueued(anim.name, QueueMode.PlayNow);
		}else{
			GetComponent<PlayerMecanimAnimationC>().AttackAnimation(anim.name);
		}
		
		dodging = true;
		stamina -= dodgeRollSetting.staminaUse;
		GetComponent<StatusC>().dodge = true;
		motor.canControl = false;
		yield return new WaitForSeconds(0.5f);
		GetComponent<StatusC>().dodge = false;
		recover = true;
		motor.canControl = true;
		dodging = false;
		recoverStamina = 0.0f;
		
	}
		
}