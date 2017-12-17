using UnityEngine;
using System.Collections;

public class QuestClientC : MonoBehaviour {
	
	public int questId = 1;
	public GameObject questData;
	public Texture2D button;
	public Texture2D textWindow;
	[HideInInspector]
	public bool enter = false;
	private bool  showGui = false;
	private bool  showError = false;
	[HideInInspector]
	public int s = 0;
	
	private GameObject player;
	
	public TextDialogue[] talkText = new TextDialogue[3];
	public TextDialogue[] ongoingQuestText = new TextDialogue[1];
	public TextDialogue[] finishQuestText = new TextDialogue[1];
	public TextDialogue[] alreadyFinishText = new TextDialogue[1];
	private string errorLog = "Quest Full...";
	
	public GUIStyle textStyle;
	private bool acceptQuest = false;
	public bool trigger = true;
	public string showText = "";
	private bool thisActive = false;
	private bool questFinish = false;
	public string sendMsgWhenTakeQuest = "";
	
	void Update (){
		if(Input.GetKeyDown("e") && enter && thisActive && !showError){
			SetDialogue();
		}
	}
	
	public void SetDialogue (){
		DialogueC dl = GetComponent<DialogueC>();
		if(!player){
			player = GameObject.FindWithTag("Player");
		}
		dl.player = player;
		dl.enter = true;
		
		int ongoing = player.GetComponent<QuestStatC>().CheckQuestProgress(questId);
		int finish = questData.GetComponent<QuestDataC>().questData[questId].finishProgress;
		int qprogress = player.GetComponent<QuestStatC>().questProgress[questId];
		if(qprogress >= finish + 9){
			dl.message = alreadyFinishText;
			dl.sendMessageWhenDone = "";
			dl.NextPage();
			print("Already Clear");
			return;
		}
		if(acceptQuest){
			if(ongoing >= finish){
				dl.message = finishQuestText;
				dl.sendMessageWhenDone = "FinishQuest";
				dl.NextPage();
			}else{
				dl.message = ongoingQuestText;
				dl.sendMessageWhenDone = "";
				dl.NextPage();
			}
		}else{
			dl.message = talkText;
			dl.sendMessageWhenDone = "TakeQuest";
			dl.NextPage();
		}
	}
	
	public void TakeQuest(){
		showGui = false;
		StartCoroutine(AcceptQuest());
		CloseTalk();	
	}
	
	public void FinishQuest(){
		showGui = false;
		questData.GetComponent<QuestDataC>().QuestClear(questId , player);
		player.GetComponent<QuestStatC>().Clear(questId);
		print("Clear");
		questFinish = true;
		CloseTalk();
	}
	
	public IEnumerator AcceptQuest(){
		bool full = player.GetComponent<QuestStatC>().AddQuest(questId);
		if(full){
			showError = true;
			yield return new WaitForSeconds(1);
			showError = false;
		}else{
			acceptQuest = player.GetComponent<QuestStatC>().CheckQuestSlot(questId);
			if(sendMsgWhenTakeQuest != ""){
				SendMessage(sendMsgWhenTakeQuest);
			}
		}
	}
	
	public void CheckQuestCondition(){
		QuestDataC quest = questData.GetComponent<QuestDataC>();
		int progress = player.GetComponent<QuestStatC>().CheckQuestProgress(questId);
		
		if(progress >= quest.questData[questId].finishProgress){
			quest.QuestClear(questId , player);
		}
	}
	
	void OnGUI(){
		if(!player){
			return;
		}
		if(enter && !showGui && !showError && !GlobalConditionC.interacting && !GlobalConditionC.freezeAll){
			GUI.DrawTexture( new Rect(Screen.width / 2 - 130, Screen.height - 120, 260, 80), button);
		}
		
		if(showError){
			GUI.DrawTexture( new Rect(80, Screen.height - 255, 615, 220), textWindow);
			GUI.Label ( new Rect(125, Screen.height - 220, 500, 200), errorLog , textStyle);
			if (GUI.Button ( new Rect(590,Screen.height - 100,80,30), "OK")) {
				showError = false;
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(!trigger){
			return;
		}
		if(other.tag == "Player"){
			s = 0;
			player = other.gameObject;
			acceptQuest = player.GetComponent<QuestStatC>().CheckQuestSlot(questId);
			enter = true;
			thisActive = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		if(!trigger){
			return;
		}
		if(other.tag == "Player"){
			s = 0;
			enter = false;
			CloseTalk();
		}
		thisActive = false;
		showError = false;
	}
	
	void CloseTalk(){
		showGui = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		s = 0;
	}
	
	public bool ActivateQuest(GameObject p){
		player = p;
		acceptQuest = player.GetComponent<QuestStatC>().CheckQuestSlot(questId);
		thisActive = false;
		trigger = false;
		SetDialogue();
		return questFinish;
	}
}