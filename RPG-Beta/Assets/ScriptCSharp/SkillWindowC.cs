using UnityEngine;
using System.Collections;

public class SkillWindowC : MonoBehaviour {
	public GameObject database;
	
	public int[] skill = new int[3];
	public int[] skillListSlot = new int[9];

	[System.Serializable]
	public class LearnSkillLV {
		public int level = 1;
		public int skillId = 1;
	}
	public LearnSkillLV[] learnSkill = new LearnSkillLV[2];
	
	private bool menu = false;
	private bool shortcutPage = true;
	private bool skillListPage = false;
	private int skillSelect = 0;

	public GUISkin skin1;
	public Rect windowRect = new Rect (360 ,80 ,360 ,185);
	private Rect originalRect;
	//private Vector2 selectedPos = new Vector2(27 , 97);
	public GUIStyle textStyle;
	public GUIStyle textStyle2;
	private bool showSkillLearned = false;
	private string showSkillName = "";
	public int pageMultiply = 8;
	private int page = 0;
	public bool autoAssignSkill = false;
	public bool useLegacyUi = false;
	
	void Start(){
		originalRect = windowRect;
		if(autoAssignSkill){
			AssignAllSkill();
		}
		
	}
	
	void Update(){
		if(Input.GetKeyDown("k") && useLegacyUi) {
			OnOffMenu();
		}
	}
	
	void OnOffMenu (){
		//Freeze Time Scale to 0 if Window is Showing
		if(!menu && Time.timeScale != 0.0f){
			menu = true;
			skillListPage = false;
			shortcutPage = true;
			Time.timeScale = 0.0f;
			//Screen.lockCursor = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}else if(menu){
			menu = false;
			Time.timeScale = 1.0f;
			//Screen.lockCursor = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void OnGUI(){
		//SkillDataC dataItem = database.GetComponent<SkillDataC>();
		if(showSkillLearned){
			GUI.Label (new Rect (Screen.width /2 -50, 85, 400, 50), "You Learned  " + showSkillName , textStyle2);
		}
		if(menu && shortcutPage){
			windowRect = GUI.Window (3, windowRect, SkillShortcut, "Skill");
		}
		//---------------Skill List----------------------------
		if(menu && skillListPage){
			windowRect = GUI.Window (3, windowRect, AllSkill, "Skill");
		}
		
	}
	void SkillShortcut(int windowID){
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		windowRect.width = 360;
		windowRect.height = 185;
		//Close Window Button
		if (GUI.Button (new Rect (310,2,30,30), "X")) {
			OnOffMenu();
		}
		
		//Skill Shortcut
		if (GUI.Button (new Rect (30,45,80,80), dataSkill.skill[skill[0]].icon)) {
			skillSelect = 0;
			skillListPage = true;
			shortcutPage = false;
		}
		GUI.Label (new Rect (70, 145, 20, 20), "1");
		if (GUI.Button (new Rect (130,45,80,80), dataSkill.skill[skill[1]].icon)) {
			skillSelect = 1;
			skillListPage = true;
			shortcutPage = false;
		}
		GUI.Label (new Rect (170, 145, 20, 20), "2");
		if (GUI.Button (new Rect (230,45,80,80), dataSkill.skill[skill[2]].icon)) {
			skillSelect = 2;
			skillListPage = true;
			shortcutPage = false;
		}
		GUI.Label (new Rect (270, 145, 20, 20), "3");
		
		GUI.DragWindow (new Rect (0,0,10000,10000));
		
	}
	
	void AllSkill(int windowID){
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		windowRect.width = 300;
		windowRect.height = 555;
		//Close Window Button
		if (GUI.Button (new Rect (260,2,30,30), "X")) {
			OnOffMenu();
		}
		if (GUI.Button (new Rect (30,60,50,50), new GUIContent (dataSkill.skill[skillListSlot[0 + page]].icon, dataSkill.skill[skillListSlot[0 + page]].description ))) {
			AssignSkill(skillSelect , 0 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 75, 140, 40), dataSkill.skill[skillListSlot[0 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 75, 140, 40), "MP : " + dataSkill.skill[skillListSlot[0 + page]].manaCost , textStyle); //Show Skill's MP Cost
		//-----------------------------
		
		if (GUI.Button (new Rect (30,120,50,50), new GUIContent (dataSkill.skill[skillListSlot[1 + page]].icon, dataSkill.skill[skillListSlot[1 + page]].description ))) {
			AssignSkill(skillSelect , 1 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 135, 140, 40), dataSkill.skill[skillListSlot[1 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 135, 140, 40), "MP : " + dataSkill.skill[skillListSlot[1 + page]].manaCost , textStyle); //Show Skill's MP Cost
		//-----------------------------
		
		if (GUI.Button (new Rect (30,180,50,50), new GUIContent (dataSkill.skill[skillListSlot[2 + page]].icon, dataSkill.skill[skillListSlot[2 + page]].description ))) {
			AssignSkill(skillSelect , 2 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 195, 140, 40), dataSkill.skill[skillListSlot[2 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 195, 140, 40), "MP : " + dataSkill.skill[skillListSlot[2 + page]].manaCost , textStyle); //Show Skill's MP Cost
		//-----------------------------
		
		if (GUI.Button (new Rect (30,240,50,50), new GUIContent (dataSkill.skill[skillListSlot[3 + page]].icon, dataSkill.skill[skillListSlot[3 + page]].description ))) {
			AssignSkill(skillSelect , 3 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 255, 140, 40), dataSkill.skill[skillListSlot[3 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 255, 140, 40), "MP : " + dataSkill.skill[skillListSlot[3 + page]].manaCost , textStyle); //Show Skill's MP Cost
		//-----------------------------
		
		if (GUI.Button (new Rect (30,300,50,50), new GUIContent (dataSkill.skill[skillListSlot[4 + page]].icon, dataSkill.skill[skillListSlot[4 + page]].description ))) {
			AssignSkill(skillSelect , 4 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 315, 140, 40), dataSkill.skill[skillListSlot[4 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 315, 140, 40), "MP : " + dataSkill.skill[skillListSlot[4 + page]].manaCost , textStyle); //Show Skill's MP Cost
		//-----------------------------
		
		if (GUI.Button (new Rect (30,360,50,50), new GUIContent (dataSkill.skill[skillListSlot[5 + page]].icon, dataSkill.skill[skillListSlot[5 + page]].description ))) {
			AssignSkill(skillSelect , 5 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 375, 140, 40), dataSkill.skill[skillListSlot[5 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 375, 140, 40), "MP : " + dataSkill.skill[skillListSlot[5 + page]].manaCost , textStyle); //Show Skill's MP Cost
		//-----------------------------
		
		if (GUI.Button (new Rect (30,420,50,50), new GUIContent (dataSkill.skill[skillListSlot[6 + page]].icon, dataSkill.skill[skillListSlot[6 + page]].description ))) {
			AssignSkill(skillSelect , 6 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 435, 140, 40), dataSkill.skill[skillListSlot[6 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 435, 140, 40), "MP : " + dataSkill.skill[skillListSlot[6 + page]].manaCost , textStyle); //Show Skill's MP Cost
		//-----------------------------
		
		if (GUI.Button (new Rect (30,480,50,50), new GUIContent (dataSkill.skill[skillListSlot[7 + page]].icon, dataSkill.skill[skillListSlot[7 + page]].description ))) {
			AssignSkill(skillSelect , 7 + page);
			shortcutPage = true;
			skillListPage = false;
		}
		GUI.Label (new Rect (95, 495, 140, 40), dataSkill.skill[skillListSlot[7 + page]].skillName , textStyle); //Show Skill's Name
		GUI.Label (new Rect (220, 495, 140, 40), "MP : " + dataSkill.skill[skillListSlot[7 + page]].manaCost , textStyle); //Show Skill's MP Cost
		
		if (GUI.Button (new Rect (220,514,25,30), "1")) {
			page = 0;
		}
		if (GUI.Button (new Rect (250,514,25,30), "2")) {
			page = pageMultiply;
		}
		
		GUI.Box (new Rect (20,20,240,26), GUI.tooltip);
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}

	//-----------------------
	
	public void AssignSkill(int id , int sk){
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		GetComponent<AttackTriggerC>().skill[id].manaCost = dataSkill.skill[skillListSlot[sk]].manaCost;
		GetComponent<AttackTriggerC>().skill[id].skillPrefab = dataSkill.skill[skillListSlot[sk]].skillPrefab;
		GetComponent<AttackTriggerC>().skill[id].skillAnimation = dataSkill.skill[skillListSlot[sk]].skillAnimation;

		GetComponent<AttackTriggerC>().skill[id].icon = dataSkill.skill[skillListSlot[sk]].icon;
		GetComponent<AttackTriggerC>().skill[id].sendMsg = dataSkill.skill[skillListSlot[sk]].sendMsg;
		GetComponent<AttackTriggerC>().skill[id].castEffect = dataSkill.skill[skillListSlot[sk]].castEffect;

		GetComponent<AttackTriggerC>().skill[id].castTime = dataSkill.skill[skillListSlot[sk]].castTime;
		GetComponent<AttackTriggerC>().skill[id].skillDelay = dataSkill.skill[skillListSlot[sk]].skillDelay;
		GetComponent<AttackTriggerC>().skill[id].whileAttack = dataSkill.skill[skillListSlot[sk]].whileAttack;

		skill[id] = skillListSlot[sk];
		print(sk);
	}

	public void AssignSkillByID(int slot , int skillId){
		//Use With Canvas UI
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		GetComponent<AttackTriggerC>().skill[slot].manaCost = dataSkill.skill[skillId].manaCost;
		GetComponent<AttackTriggerC>().skill[slot].skillPrefab = dataSkill.skill[skillId].skillPrefab;
		GetComponent<AttackTriggerC>().skill[slot].skillAnimation = dataSkill.skill[skillId].skillAnimation;
		
		GetComponent<AttackTriggerC>().skill[slot].icon = dataSkill.skill[skillId].icon;
		GetComponent<AttackTriggerC>().skill[slot].sendMsg = dataSkill.skill[skillId].sendMsg;
		GetComponent<AttackTriggerC>().skill[slot].castEffect = dataSkill.skill[skillId].castEffect;
		
		GetComponent<AttackTriggerC>().skill[slot].castTime = dataSkill.skill[skillId].castTime;
		GetComponent<AttackTriggerC>().skill[slot].skillDelay = dataSkill.skill[skillId].skillDelay;
		GetComponent<AttackTriggerC>().skill[slot].whileAttack = dataSkill.skill[skillId].whileAttack;
		
		skill[slot] = skillId;
	}
	
	public void AssignAllSkill(){
		int n = 0;
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		while(n < skill.Length){
			GetComponent<AttackTriggerC>().skill[n].manaCost = dataSkill.skill[skill[n]].manaCost;
			GetComponent<AttackTriggerC>().skill[n].skillPrefab = dataSkill.skill[skill[n]].skillPrefab;
			GetComponent<AttackTriggerC>().skill[n].skillAnimation = dataSkill.skill[skill[n]].skillAnimation;

			GetComponent<AttackTriggerC>().skill[n].icon = dataSkill.skill[skill[n]].icon;
			GetComponent<AttackTriggerC>().skill[n].sendMsg = dataSkill.skill[skill[n]].sendMsg;
			GetComponent<AttackTriggerC>().skill[n].castEffect = dataSkill.skill[skill[n]].castEffect;

			GetComponent<AttackTriggerC>().skill[n].castTime = dataSkill.skill[skill[n]].castTime;
			GetComponent<AttackTriggerC>().skill[n].skillDelay = dataSkill.skill[skill[n]].skillDelay;
			GetComponent<AttackTriggerC>().skill[n].whileAttack = dataSkill.skill[skill[n]].whileAttack;
			n++;
		}
		
	}

	public void LearnSkillByLevel(int lv){
		int c = 0;
		while(c < learnSkill.Length){
			if(lv >= learnSkill[c].level){
				AddSkill(learnSkill[c].skillId);
			}
			c++;
		}
		
	}
	public void AddSkill(int id){
		bool geta= false;
		int pt = 0;
		while(pt < skillListSlot.Length && !geta){
			if(skillListSlot[pt] == id){
				// Check if you already have this skill.
				geta = true;
			}else if(skillListSlot[pt] == 0){
				// Add Skill to empty slot.
				skillListSlot[pt] = id;
				StartCoroutine(ShowLearnedSkill(id));
				geta = true;
			}else{
				pt++;
			}
			
		}
		
	}

	IEnumerator ShowLearnedSkill(int id){
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		showSkillLearned = true;
		showSkillName = dataSkill.skill[id].skillName;
		yield return new WaitForSeconds(10.5f);
		showSkillLearned = false;
		
	}
	
	void ResetPosition(){
		//Reset GUI Position when it out of Screen.
		if(windowRect.x >= Screen.width -30 || windowRect.y >= Screen.height -30 || windowRect.x <= -70 || windowRect.y <= -70 ){
			windowRect = originalRect;
		}
	}

	public bool HaveSkill(int id){
		bool have = false;
		for(int a = 0; a < skillListSlot.Length; a++){
			if(skillListSlot[a] == id){
				have = true;
			}
		}
		return have;
	}
}