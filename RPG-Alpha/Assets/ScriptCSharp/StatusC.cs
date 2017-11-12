using UnityEngine;
using System.Collections;

public class StatusC : MonoBehaviour {

	public string characterName = "";
	public int characterId = 0;
	public int level = 1;
	public int atk = 0;
	public int def = 0;
	public int matk = 0;
	public int mdef = 0;
	public int exp = 0;
	public int maxExp = 100;
	public int maxHealth = 100;
	public int health = 100;
	public int maxMana = 100;
	public int mana = 100;
	public int statusPoint = 0;
	public int skillPoint = 0;
	private bool dead = false;
	public bool immortal = false;
	
	[HideInInspector]
	public GameObject mainModel;
	
	[HideInInspector]
		public int addAtk = 0;
	[HideInInspector]
		public int addDef = 0;
	[HideInInspector]
		public int addMatk = 0;
	[HideInInspector]
		public int addMdef = 0;

	public Transform deathBody;
	
	[HideInInspector]
		public string spawnPointName = ""; 
	
	//States
	[HideInInspector]
		public int buffAtk = 0;
	[HideInInspector]
		public int buffDef = 0;
	[HideInInspector]
		public int buffMatk = 0;
	[HideInInspector]
		public int buffMdef = 0;
	[HideInInspector]
		public int weaponAtk = 0;
	[HideInInspector]
		public int weaponMatk = 0;
	[HideInInspector]
		public bool poison = false;
	[HideInInspector]
		public bool silence = false;
	[HideInInspector]
		public bool web = false;
	[HideInInspector]
		public bool stun = false;
	[HideInInspector]
		public bool freeze = false; 
	[HideInInspector]
		public bool dodge = false;
	[HideInInspector]
		public bool brave = false; 
	[HideInInspector]
		public bool barrier = false;
	[HideInInspector]
		public bool mbarrier = false;
	[HideInInspector]
		public bool faith = false; 
	

	public GameObject poisonEffect;
	public GameObject silenceEffect;
	public GameObject stunEffect;
	public GameObject webbedUpEffect;
	
	public AnimationClip stunAnimation;
	public AnimationClip webbedUpAnimation;
	[System.Serializable]
	public class elem{
		public string elementName = "";
		public int effective = 100;
	}
	public elem[] elementEffective = new elem[5];
	public Resist statusResist;
	[HideInInspector]
	public Resist eqResist;
	[HideInInspector]
	public Resist totalResist;
	[HideInInspector]
	public HiddenStat hiddenStatus;
	[HideInInspector]
	public bool useMecanim = false;
	public string sendMsgWhenDead = "";

	void Start(){
		CalculateStatus();
	}
	
	public string OnDamage(int amount , int element){	
		if (!dead) {
			if(dodge){
				return "Evaded";
			}
			if(immortal){
				return "Invulnerable";
			}
			if(hiddenStatus.autoGuard > 0){
				int ran = Random.Range(0 , 100);
				if(ran <= hiddenStatus.autoGuard){
					return "Guard";
				}
			}
			amount -= def;
			amount -= addDef;
			amount -= buffDef;
			amount *= elementEffective [element].effective;
			amount /= 100;
		
			if (amount < 1) {
					amount = 1;
			}
		
			health -= amount;
		
			if (health <= 0) {
					health = 0;
					enabled = false;
					dead = true;
					Death ();
			}

		}
		return amount.ToString();
	}

	public string OnMagicDamage(int amount , int element){
		if(!dead){
			if(dodge){
				return "Evaded";
			}
			if(immortal){
				return "Invulnerable";
			}
			if(hiddenStatus.autoGuard > 0){
				int ran = Random.Range(0 , 100);
				if(ran <= hiddenStatus.autoGuard){
					return "Guard";
				}
			}
			amount -= mdef;
			amount -= addMdef;
			amount -= buffMdef;
			amount *= elementEffective[element].effective;
			amount /= 100;
		
			if(amount < 1){
				amount = 1;
			}
		
			health -= amount;
		
			if (health <= 0){
				health = 0;
				enabled = false;
				dead = true;
				Death();
			}
		}
		return amount.ToString();
	}
	
	public void Heal(int hp , int mp){
		health += hp;
		if (health >= maxHealth){
			health = maxHealth;
		}
		
		mana += mp;
		if (mana >= maxMana){
			mana = maxMana;
		}
	}
	
	
	public void Death(){
		if(sendMsgWhenDead != ""){
			SendMessage(sendMsgWhenDead , SendMessageOptions.DontRequireReceiver);
		}
		if(gameObject.tag == "Player"){
			SaveData();
			if(GetComponent<UiMasterC>()){
				GetComponent<UiMasterC>().DestroyAllUi();
			}
		}
		Destroy(gameObject);
		if(deathBody){
			Instantiate(deathBody, transform.position , transform.rotation);
		}else{
			print("This Object didn't assign the Death Body");
		}
	}

	public void gainEXP(int gain){
		exp += gain;
		if(exp >= maxExp){
			int remain = exp - maxExp;
			LevelUp(remain);
		}
	}
	
	public void LevelUp(int remainingEXP){
		exp = 0;
		exp += remainingEXP;
		level++;
		statusPoint += 5;
		skillPoint++;
		maxExp = 125 * maxExp  / 100;
		maxHealth += 20;
		maxMana += 10;
		health = maxHealth;
		mana = maxMana;
		gainEXP(0);
	}
	
	void SaveData(){
		PlayerPrefs.SetInt("PreviousSave", 10);
		PlayerPrefs.SetString("TempName", characterName);
		PlayerPrefs.SetInt("TempID", characterId);
		PlayerPrefs.SetInt("TempPlayerLevel", level);
		PlayerPrefs.SetInt("TempPlayerATK", atk);
		PlayerPrefs.SetInt("TempPlayerDEF", def);
		PlayerPrefs.SetInt("TempPlayerMATK", matk);
		PlayerPrefs.SetInt("TempPlayerMDEF", mdef);
		PlayerPrefs.SetInt("TempPlayerEXP", exp);
		PlayerPrefs.SetInt("TempPlayerMaxEXP", maxExp);
		PlayerPrefs.SetInt("TempPlayerMaxHP", maxHealth);
		PlayerPrefs.SetInt("TempPlayerMaxMP", maxMana);
		PlayerPrefs.SetInt("TempPlayerSTP", statusPoint);
		PlayerPrefs.SetInt("TempPlayerSKP", skillPoint);
	}
	
	public void CalculateStatus(){
		addAtk = 0;
		addAtk += atk + buffAtk + weaponAtk;
		addMatk = 0;
		addMatk += matk + buffMatk + weaponMatk;
		if (health >= maxHealth){
			health = maxHealth;
		}
		if (mana >= maxMana){
			mana = maxMana;
		}
		totalResist.poisonResist = statusResist.poisonResist + eqResist.poisonResist;
		totalResist.silenceResist = statusResist.silenceResist + eqResist.silenceResist;
		totalResist.stunResist = statusResist.stunResist + eqResist.stunResist;
		totalResist.webResist = statusResist.webResist + eqResist.webResist;
	}

	//States
	public IEnumerator OnPoison(int hurtTime){
		int amount = 0;
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!poison){
			int chance= 100;
			chance -= totalResist.poisonResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
					poison = true;
					amount = maxHealth * 2 / 100; 
				}
			
			}
			while(poison && hurtTime > 0){
				if(poisonEffect){ 
					eff = Instantiate(poisonEffect, transform.position, poisonEffect.transform.rotation) as GameObject;
					eff.transform.parent = transform;
				}
				yield return new WaitForSeconds(0.7f); 
				health -= amount;
			
				if (health <= 1){
					health = 1;
				}
				if(eff){
					Destroy(eff.gameObject);
				}
				hurtTime--;
				if(hurtTime <= 0){
					poison = false;
				}
			}
		}
	}

	
	public IEnumerator OnSilence(float dur){
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!silence){
			int chance= 100;
			chance -= totalResist.silenceResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
						silence = true;
					if(silenceEffect){
						eff = Instantiate(silenceEffect, transform.position, transform.rotation) as GameObject;
						eff.transform.parent = transform;
					}
						yield return new WaitForSeconds(dur);
						if(eff){ 
							Destroy(eff.gameObject);
						}
						silence = false;
				}
				
			}

		}
	}

	public IEnumerator OnWebbedUp(float dur){
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!web){
			int chance= 100;
			chance -= totalResist.webResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
					web = true;
					freeze = true; 
					if(webbedUpEffect){
						eff = Instantiate(webbedUpEffect, transform.position, transform.rotation) as GameObject;
						eff.transform.parent = transform;
					}
					if(webbedUpAnimation){
						if(useMecanim){
						}else{
							mainModel.GetComponent<Animation>()[webbedUpAnimation.name].layer = 25;
							mainModel.GetComponent<Animation>().Play(webbedUpAnimation.name);
						}
					}
					yield return new WaitForSeconds(dur);
					if(eff){ 
						Destroy(eff.gameObject);
					}
					if(webbedUpAnimation && !useMecanim){
						mainModel.GetComponent<Animation>().Stop(webbedUpAnimation.name);
					}
					freeze = false;
					web = false;
				}
				
			}

		}
	}

	public IEnumerator OnStun(float dur){
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!stun){
			int chance= 100;
			chance -= totalResist.stunResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
					stun = true;
					freeze = true;
					if(stunEffect){
						eff = Instantiate(stunEffect, transform.position, stunEffect.transform.rotation) as GameObject;
						eff.transform.parent = transform;
					}
					if(stunAnimation){
						if(useMecanim){
						}else{
							mainModel.GetComponent<Animation>()[stunAnimation.name].layer = 25;
							mainModel.GetComponent<Animation>().Play(stunAnimation.name);
						}
					}
					yield return new WaitForSeconds(dur);
					if(eff){ 
						Destroy(eff.gameObject);
					}
					if(stunAnimation && !useMecanim){
						mainModel.GetComponent<Animation>().Stop(stunAnimation.name);
					}
					freeze = false; 
					stun = false;
				}
				
			}

		}
		
	}

	public void ApplyAbnormalStat(int statId , float dur){
		if(GlobalConditionC.freezePlayer){
			return;
		}
		if(statId == 0){
			OnPoison(Mathf.FloorToInt(dur));
			StartCoroutine(OnPoison(Mathf.FloorToInt(dur)));
		}
		if(statId == 1){
			//OnSilence(dur);
			StartCoroutine(OnSilence(dur));
		}
		if(statId == 2){
			//OnStun(dur);
			StartCoroutine(OnStun(dur));
		}
		if(statId == 3){
			//OnWebbedUp(dur);
			StartCoroutine(OnWebbedUp(dur));
		}
		
		
	}
	
	public IEnumerator OnBarrier (int amount , float dur){
		//增加防御
		if(!barrier){
			barrier = true;
			buffDef = 0;
			buffDef += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffDef = 0;
			barrier = false;
			CalculateStatus();
		}
		
	}

	public IEnumerator OnMagicBarrier(int amount , float dur){
		//增加魔抗
		if(!mbarrier){
			mbarrier = true;
			buffMdef = 0;
			buffMdef += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffMdef = 0;
			mbarrier = false;
			CalculateStatus();
		}

	}

	public IEnumerator OnBrave(int amount , float dur){
		//增加攻击
		if(!brave){
			brave = true;
			buffAtk = 0;
			buffAtk += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffAtk = 0;
			brave = false;
			CalculateStatus();
		}
		
	}
	
	public IEnumerator OnFaith (int amount , float dur){
		//增加法攻
		if(!faith){
			faith = true;
			buffMatk = 0;
			buffMatk += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffMatk = 0;
			faith = false;
			CalculateStatus();
		}
		
	}

	public void ApplyBuff(int statId , float dur , int amount){
		if(statId == 1){
			//增加防御
			StartCoroutine(OnBarrier(amount , dur));
		}
		if(statId == 2){
			//增加魔抗
			StartCoroutine(OnMagicBarrier(amount , dur));
		}
		if(statId == 3){
			//增加攻击
			StartCoroutine(OnBrave(amount , dur));
		}
		if(statId == 4){
			//增加魔攻
			StartCoroutine(OnFaith(amount , dur));
		}
	}
}

[System.Serializable]
public class Resist{
	public int poisonResist = 0;
	public int silenceResist = 0;
	public int webResist = 0;
	public int stunResist = 0;
}

[System.Serializable]
public class HiddenStat{
	public bool doubleJump = false;
	public int drainTouch = 0;
	public int autoGuard = 0;
	public int mpReduce = 0;
}
