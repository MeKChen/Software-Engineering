using UnityEngine;
using System.Collections;

[RequireComponent (typeof (StatusC))]
[RequireComponent (typeof (UiMasterC))]
[RequireComponent (typeof (PlayerInputControllerC))]
[RequireComponent (typeof (CharacterMotorC))]

public class AttackTriggerC : MonoBehaviour {
	public GameObject mainModel;
	public Transform attackPoint;
	public Transform cameraZoomPoint;
	public Transform attackPrefab;
	public bool useMecanim = false;
	
	public whileAtk whileAttack = whileAtk.MeleeFwd;

	public AimType aimingType = AimType.Normal;
	
	private bool atkDelay = false;
	public bool freeze = false;
	public int skillIconSize = 80;
	
	public float attackSpeed = 0.15f;
	private float nextFire = 0.0f;
	public float atkDelay1 = 0.1f;

	public AnimationClip[] attackCombo = new AnimationClip[3];
	public float attackAnimationSpeed = 1.0f;

	private AnimationClip hurt;
	
	private bool meleefwd = false;
	[HideInInspector]
	public bool isCasting = false;
	
	private int c = 0;
	private int conCombo = 0;
	
	public Transform Maincam;
	public GameObject MaincamPrefab;
	public GameObject attackPointPrefab;
	
	private int str = 0;
	private int matk = 0;
	
	public Texture2D aimIcon;
	public int aimIconSize = 40;

	[HideInInspector]
	public bool flinch = false;
	private int skillEquip  = 0;
	private Vector3 knock = Vector3.zero;

	//----------Sounds-------------
	[System.Serializable]
	public class AtkSound {
		public AudioClip[] attackComboVoice = new AudioClip[3];
		public AudioClip magicCastVoice;
		public AudioClip hurtVoice;
	}
	public AtkSound sound;

	[HideInInspector]
	public GameObject pet;
	private GameObject castEff;

	public Texture2D braveIcon;
	public Texture2D barrierIcon;
	public Texture2D faithIcon;
	public Texture2D magicBarrierIcon;
	
	void Awake(){
		if(!mainModel){
			mainModel = this.gameObject;
		}
		GetComponent<StatusC>().mainModel = mainModel;
		GetComponent<StatusC>().useMecanim = useMecanim;
		gameObject.tag = "Player";

		GameObject[] cam = GameObject.FindGameObjectsWithTag("MainCamera"); 
		foreach(GameObject cam2 in cam) { 
			if(cam2){
				Destroy(cam2.gameObject);
			}
		}
		GameObject newCam = GameObject.FindWithTag ("MainCamera");
		newCam = Instantiate(MaincamPrefab, transform.position , transform.rotation) as GameObject;
		Maincam = newCam.transform;
		// Set Target to ARPG Camera
    	if(!cameraZoomPoint || aimingType == AimType.Normal){
    		Maincam.GetComponent<ARPGcameraC>().target = this.transform;
    	}else{
    		Maincam.GetComponent<ARPGcameraC>().target = cameraZoomPoint;
    	}
    	Maincam.GetComponent<ARPGcameraC>().targetBody = this.transform;

		str = GetComponent<StatusC>().addAtk;
		matk = GetComponent<StatusC>().addMatk;
		int animationSize = attackCombo.Length;
		int a = 0;
		if(animationSize > 0 && !useMecanim){
			while(a < animationSize && attackCombo[a]){
				mainModel.GetComponent<Animation>()[attackCombo[a].name].layer = 15;
				a++;
			}
		}

		if(!attackPoint){
			if(!attackPointPrefab){
				print("Please assign Attack Point");
				freeze = true;
				return;
			}
			GameObject newAtkPoint = Instantiate(attackPointPrefab, transform.position , transform.rotation) as GameObject;
			newAtkPoint.transform.parent = this.transform;
			attackPoint = newAtkPoint.transform;	
		}

		if (!useMecanim){
			hurt = GetComponent<PlayerAnimationC> ().hurt;
		}
		if(aimingType == AimType.Raycast){//Auto Lock On for Raycast Mode
			Maincam.GetComponent<ARPGcameraC>().lockOn = true;
		}
	}
	
	
	void Update(){
		StatusC stat = GetComponent<StatusC>();
		if(freeze || atkDelay || Time.timeScale == 0.0f || stat.freeze || GlobalConditionC.freezeAll || GlobalConditionC.freezePlayer){
			return;
		}
		CharacterController controller = GetComponent<CharacterController>();
		if (flinch){
			controller.Move(knock * 6* Time.deltaTime);
			return;
		}
		
		if (meleefwd){
			Vector3 lui = transform.TransformDirection(Vector3.forward);
			controller.Move(lui * 5 * Time.deltaTime);
		}
		if(aimingType == AimType.Raycast){
			Aiming();
		}else{
			attackPoint.transform.rotation = Maincam.GetComponent<ARPGcameraC>().aim;
		}
		//Normal Trigger
		if (Input.GetButton("Fire1") && Time.time > nextFire && !isCasting) {
			if(Time.time > (nextFire + 0.5f)){
				c = 0;
			}
			//Attack Combo
			if(attackCombo.Length >= 1){
				conCombo++;
				StartCoroutine(AttackCombo());

			}
		}
	}

	void OnGUI(){
		if(aimingType == AimType.Normal){
			GUI.DrawTexture ( new Rect(Screen.width/2 - 16,Screen.height/2 - 90,aimIconSize,aimIconSize), aimIcon);
		}
		if(aimingType == AimType.Raycast){
			GUI.DrawTexture ( new Rect(Screen.width/2 - 20,Screen.height/2 - 20,40,40), aimIcon);
		}
		
		StatusC stat = GetComponent<StatusC>();
		//Show Buffs Icon
		if(stat.brave){
			GUI.DrawTexture( new Rect(30,200,60,60), braveIcon);
		}
		if(stat.barrier){
			GUI.DrawTexture( new Rect(30,260,60,60), barrierIcon);
		}
		if(stat.faith){
			GUI.DrawTexture( new Rect(30,320,60,60), faithIcon);
		}
		if(stat.mbarrier){
			GUI.DrawTexture( new Rect(30,380,60,60), magicBarrierIcon);
		}
	}


	IEnumerator AttackCombo(){
		if(c >= attackCombo.Length){
			c = 0;
		}
		float wait = 0.0f;
		if(attackCombo[c]){
			str = GetComponent<StatusC>().addAtk;
			matk = GetComponent<StatusC>().addMatk;
			Transform bulletShootout;
			isCasting = true;
			
			if(whileAttack == whileAtk.MeleeFwd){
				GetComponent<CharacterMotorC>().canControl = false;
			
				StartCoroutine(MeleeDash());
			}
			// If Immobile
			if(whileAttack == whileAtk.Immobile){
				GetComponent<CharacterMotorC>().canControl = false;
			}

			if(sound.attackComboVoice.Length > c && sound.attackComboVoice[c]){
				GetComponent<AudioSource>().PlayOneShot(sound.attackComboVoice[c]);
			}
			
			while(conCombo > 0){
				if(!useMecanim){
					//For Legacy Animation
					mainModel.GetComponent<Animation>().PlayQueued(attackCombo[c].name, QueueMode.PlayNow).speed = attackAnimationSpeed;
					wait = mainModel.GetComponent<Animation>()[attackCombo[c].name].length;
				}
				
				yield return new WaitForSeconds(atkDelay1);
				c++;
				
				nextFire = Time.time + attackSpeed;
				bulletShootout = Instantiate(attackPrefab, attackPoint.transform.position , attackPoint.transform.rotation) as Transform;
				conCombo -= 1;
				
				if(c >= attackCombo.Length){
					c = 0;
					atkDelay = true;
					yield return new WaitForSeconds(wait);
					atkDelay = false;
				}else{
					yield return new WaitForSeconds(attackSpeed);
				}
				
			}
			
			isCasting = false;
			GetComponent<CharacterMotorC>().canControl = true;
		} else {
			print ("Please assign attack animation in Attack Combo");
		}

	}

	
	IEnumerator MeleeDash(){
		meleefwd = true;
		yield return new WaitForSeconds(0.2f);
		meleefwd = false;
	}
	
	public void Flinch(Vector3 dir){
		if(sound.hurtVoice && GetComponent<StatusC>().health >= 1){
			GetComponent<AudioSource>().PlayOneShot(sound.hurtVoice);
		}
		if(GlobalConditionC.freezePlayer){
			return;
		}
		knock = dir;
		GetComponent<CharacterMotorC>().canControl = false;
		StartCoroutine(KnockBack());
		if(!useMecanim && hurt){
			mainModel.GetComponent<Animation>().PlayQueued(hurt.name, QueueMode.PlayNow);
		}
		GetComponent<CharacterMotorC>().canControl = true;
	}
	
	IEnumerator KnockBack (){
		flinch = true;
		yield return new WaitForSeconds(0.2f);
		flinch = false;
	}

	public void WhileAttackSet(int watk){
		if(watk == 2) {
			whileAttack = whileAtk.WalkFree;
		}else if (watk == 1) {
			whileAttack = whileAtk.Immobile;
		}else {
			whileAttack = whileAtk.MeleeFwd;
		}
	}
	
	void Aiming(){
		Ray ray = Maincam.GetComponent<Camera>().ViewportPointToRay (new Vector3(0.5f,0.5f,0.0f));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)){
			attackPoint.transform.LookAt(hit.point);
		}else{
			attackPoint.transform.rotation = Maincam.transform.rotation;
		}
}
	
			
}
public enum whileAtk{
	MeleeFwd = 0,
	Immobile = 1,
	WalkFree = 2
}

public enum AimType{
	Normal = 0,
	Raycast = 1
}
