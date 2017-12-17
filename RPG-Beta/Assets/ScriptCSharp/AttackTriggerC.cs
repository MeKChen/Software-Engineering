using UnityEngine;
using System.Collections;

[RequireComponent (typeof (StatusC))]
[RequireComponent (typeof (UiMasterC))]
[RequireComponent (typeof (PlayerInputControllerC))]
[RequireComponent (typeof (CharacterMotorC))]
[RequireComponent (typeof (InventoryC))]
[RequireComponent (typeof (QuestStatC))]
[RequireComponent (typeof (SkillWindowC))]

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
	public SkillSetting[] skill = new SkillSetting[3];

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

	//声音
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

	//buffs
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
		GameObject minimap = GameObject.FindWithTag("Minimap");
			if(minimap){
				GameObject mapcam = minimap.GetComponent<MinimapOnOffC>().minimapCam;
				mapcam.GetComponent<MinimapCameraC>().target = this.transform;
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
		if (Input.GetButton("Fire1") && Time.time > nextFire && !isCasting) {
			if(Time.time > (nextFire + 0.5f)){
				c = 0;
			}
			//连招
			if(attackCombo.Length >= 1){
				conCombo++;
				StartCoroutine(AttackCombo());

			}
		}
		//魔法
		if (Input.GetButtonDown("Fire2") && Time.time > nextFire && !isCasting && skill[skillEquip].skillPrefab && !stat.silence) {
			StartCoroutine(MagicSkill(skillEquip));
		}
		if(Input.GetKeyDown("1") && !isCasting && skill[0].skillPrefab){
			skillEquip = 0;
		}
		if(Input.GetKeyDown("2") && !isCasting && skill[1].skillPrefab){
			skillEquip = 1;
		}
		if(Input.GetKeyDown("3") && !isCasting && skill[2].skillPrefab){
			skillEquip = 2;
		}
		
	}

	void OnGUI(){
		if(aimingType == AimType.Normal){
			GUI.DrawTexture ( new Rect(Screen.width/2 - 16,Screen.height/2 - 90,aimIconSize,aimIconSize), aimIcon);
		}
		if(aimingType == AimType.Raycast){
			GUI.DrawTexture ( new Rect(Screen.width/2 - 20,Screen.height/2 - 20,40,40), aimIcon);
		}
		
		
		if(skill[skillEquip].skillPrefab && skill[skillEquip].icon){
			GUI.DrawTexture ( new Rect(Screen.width -skillIconSize - 28,Screen.height - skillIconSize - 20,skillIconSize,skillIconSize), skill[skillEquip].icon);
		}
		if(skill[0].skillPrefab && skill[0].icon){
			GUI.DrawTexture ( new Rect(Screen.width -skillIconSize -50,Screen.height - skillIconSize -50,skillIconSize /2,skillIconSize /2), skill[0].icon);
		}
		if(skill[1].skillPrefab && skill[1].icon){
			GUI.DrawTexture ( new Rect(Screen.width -skillIconSize -10,Screen.height - skillIconSize -60,skillIconSize /2,skillIconSize /2), skill[1].icon);
		}
		if(skill[2].skillPrefab && skill[2].icon){
			GUI.DrawTexture ( new Rect(Screen.width -skillIconSize +30 ,Screen.height - skillIconSize -50,skillIconSize /2,skillIconSize /2), skill[2].icon);
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
			if(whileAttack == whileAtk.Immobile){
				GetComponent<CharacterMotorC>().canControl = false;
			}

			if(sound.attackComboVoice.Length > c && sound.attackComboVoice[c]){
				GetComponent<AudioSource>().PlayOneShot(sound.attackComboVoice[c]);
			}
			
			while(conCombo > 0){
				if(!useMecanim){
					mainModel.GetComponent<Animation>().PlayQueued(attackCombo[c].name, QueueMode.PlayNow).speed = attackAnimationSpeed;
					wait = mainModel.GetComponent<Animation>()[attackCombo[c].name].length;
				}else{
					GetComponent<PlayerMecanimAnimationC>().AttackAnimation(attackCombo[c].name);
					float clip = GetComponent<PlayerMecanimAnimationC>().animator.GetCurrentAnimatorClipInfo(0).Length;
					wait = clip -0.3f;
				}
				
				yield return new WaitForSeconds(atkDelay1);
				c++;
				
				nextFire = Time.time + attackSpeed;
				bulletShootout = Instantiate(attackPrefab, attackPoint.transform.position , attackPoint.transform.rotation) as Transform;
				bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
				conCombo -= 1;
				if(GetComponent<StatusC>().hiddenStatus.drainTouch > 0){
					bulletShootout.GetComponent<BulletStatusC>().drainHp += GetComponent<StatusC>().hiddenStatus.drainTouch;
				}
				
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
	
	IEnumerator MagicSkill(int skillID){
		c = 0;
		int cost = skill[skillID].manaCost;
		if(GetComponent<StatusC>().hiddenStatus.mpReduce > 0){
			int per = 100 - GetComponent<StatusC>().hiddenStatus.mpReduce;
			if(per < 0){
				per = 0;
			}
			cost *= per;
			cost /= 100;
		}
		if(GetComponent<StatusC>().mana < cost){
			yield return false;
		}
		if(skill[skillID].sendMsg != ""){
			SendMessage(skill[skillID].sendMsg , SendMessageOptions.DontRequireReceiver);
		}

		GetComponent<StatusC>().mana -= cost;

		if(skill[skillID].skillAnimation){
			str = GetComponent<StatusC>().addAtk;
			matk = GetComponent<StatusC>().addMatk;
			
			if(!GetComponent<StatusC>().silence){
				if(sound.magicCastVoice){
					GetComponent<AudioSource>().clip = sound.magicCastVoice;
					GetComponent<AudioSource>().Play();
				}
				isCasting = true;
				if(skill[skillID].whileAttack == whileAtk.MeleeFwd){
					GetComponent<CharacterMotorC>().canControl = false;
					meleefwd = true;
				}
				if(skill[skillID].whileAttack == whileAtk.Immobile){
					GetComponent<CharacterMotorC>().canControl = false;
				}

				if(!useMecanim){
					mainModel.GetComponent<Animation>()[skill[skillID].skillAnimation.name].layer = 16;
					mainModel.GetComponent<Animation>().PlayQueued(skill[skillID].skillAnimation.name , QueueMode.PlayNow);
				}else{
					GetComponent<PlayerMecanimAnimationC>().AttackAnimation(skill[skillID].skillAnimation.name);
				}
				if(skill[skillID].castEffect){
					castEff = Instantiate(skill[skillID].castEffect , transform.position , transform.rotation) as GameObject;
					castEff.transform.parent = this.transform;
				}
				nextFire = Time.time + skill[skillID].skillDelay;
				Maincam.GetComponent<ARPGcameraC>().lockOn = true;

				yield return new WaitForSeconds(skill[skillID].castTime);
				if(aimingType == AimType.Normal){
					Maincam.GetComponent<ARPGcameraC>().lockOn = false;
				}
				if(castEff){
					Destroy(castEff);
				}
				Transform bulletShootout = Instantiate(skill[skillID].skillPrefab, attackPoint.transform.position , attackPoint.transform.rotation) as Transform;
				bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
				yield return new WaitForSeconds(skill[skillID].skillDelay);
				isCasting = false;
				meleefwd = false;
				GetComponent<CharacterMotorC>().canControl = true;
			}


		}else{
			print("Please assign skill animation in Skill Animation");
		}

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
