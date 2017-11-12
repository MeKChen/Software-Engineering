using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarCanvasC : MonoBehaviour {
	public Image hpBar;
	public Image mpBar;
	public Image expBar;
	public Text hpText;
	public Text mpText;
	public Text lvText;
	public GameObject player;
      private float t;

    public GameObject tishi;
	void Start(){
        tishi = GameObject.Find("tishixinxi");
		DontDestroyOnLoad(transform.gameObject);
		if(!player){
			player = GameObject.FindWithTag("Player");
		}
	}
	
	void Update(){
		if(!player){
			Destroy(gameObject);
			return;
		}
		StatusC stat = player.GetComponent<StatusC>();
		
		int maxHp = stat.maxHealth;
		float hp = stat.health;
		int maxMp = stat.maxMana;
		float mp = stat.mana;
		int exp = stat.exp;
		float maxExp = stat.maxExp;
		float curHp = hp/maxHp;
		float curMp = mp/maxMp;
		float curExp = exp/maxExp;
        if (expBar.fillAmount == 0)
        {
            tishi.GetComponent<Text>().text = "";
          
            t += (float)Time.deltaTime;
            if (t >= 2f)
            {
                tishi.GetComponent<Text>().text = null;
            }
        }

		//HP
		if(curHp > hpBar.fillAmount){
			hpBar.fillAmount += 1 / 1 * Time.unscaledDeltaTime;
			if(hpBar.fillAmount > curHp){
				hpBar.fillAmount = curHp;
                expBar.fillAmount = curHp;
            }
		}	
		if(curHp < hpBar.fillAmount){
			hpBar.fillAmount -= 1 / 1 * Time.unscaledDeltaTime;
			if(hpBar.fillAmount < curHp){
                expBar.fillAmount = (curHp - 0.5f);
                hpBar.fillAmount = curHp;
			}
		}
		
		//MP
		if(curMp > mpBar.fillAmount){
			mpBar.fillAmount += 1 / 1 * Time.unscaledDeltaTime;
			if(mpBar.fillAmount > curMp){
				mpBar.fillAmount = curMp;
			}
		}	
		if(curMp < mpBar.fillAmount){
			mpBar.fillAmount -= 1 / 1 * Time.unscaledDeltaTime;
			if(mpBar.fillAmount < curMp){
				mpBar.fillAmount = curMp;
			}
		}
	}
}