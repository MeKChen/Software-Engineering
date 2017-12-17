﻿using UnityEngine;
using System.Collections;

public class QuestStatC : MonoBehaviour {
	
	public GameObject questDataBase;
	
	public int[] questProgress = new int[20];
	public int[] questSlot = new int[5];

	private bool  menu = false;
	
	
	void  Start (){
		QuestDataC quest = questDataBase.GetComponent<QuestDataC>();
		if(questProgress .Length < quest.questData.Length){
			questProgress = new int[quest.questData.Length];
		}
		
	}
	
	void  Update (){
		if (Input.GetKeyDown("q")) {
			OnOffMenu();
		}
		
		
	}
	
	public bool AddQuest ( int id  ){
		bool  full = false;
		bool  geta = false;
		
		int pt= 0;
		while(pt < questSlot.Length && !geta){
			if(questSlot[pt] == id){
				print("You Already Accept this Quest");
				geta = true;
				
			}else if(questSlot[pt] == 0){
				questSlot[pt] = id;
				geta = true;
			}else{
				pt++;
				if(pt >= questSlot.Length){
					full = true;
					print("Full");
				}
			}
			
		}
		
		return full;
		
	}
	
	public void  SortQuest (){
		int pt= 0;
		int nextp= 0;
		bool  clearr = false;
		while(pt < questSlot.Length){
			if(questSlot[pt] == 0){
				nextp = pt + 1;
				while(nextp < questSlot.Length && !clearr){
					if(questSlot[nextp] > 0){
						questSlot[pt] = questSlot[nextp];
						questSlot[nextp] = 0;
						clearr = true;
					}else{
						nextp++;
					}
					
				}
				clearr = false;
				pt++;
			}else{
				pt++;
			}
			
		}
		
	}
	
	void  OnGUI (){
		QuestDataC data = questDataBase.GetComponent<QuestDataC>();
		if(menu){
			GUI.Box ( new Rect(260,140,280,385), "Quest Log");
			if (GUI.Button ( new Rect(490,142,30,30), "X")) {
				OnOffMenu();
			}
			
			if(questSlot[0] > 0){
				//任务名
				GUI.Label ( new Rect(275, 185, 280, 40), data.questData[questSlot[0]].questName);
				//任务进度
				GUI.Label ( new Rect(275, 210, 280, 40), data.questData[questSlot[0]].description + " (" + questProgress[questSlot[0]].ToString() + " / " + data.questData[questSlot[0]].finishProgress + ")");
				//放弃任务
				if (GUI.Button ( new Rect(450,195,64,32), "Cancel")) {
					questProgress[questSlot[0]] = 0;
					questSlot[0] = 0;
					SortQuest();
				}
			}
			//-----------------------------------------
			if(questSlot[1] > 0){
				//任务名
				GUI.Label ( new Rect(275, 245, 280, 40), data.questData[questSlot[1]].questName);
                //任务进度
                GUI.Label ( new Rect(275, 270, 280, 40), data.questData[questSlot[1]].description + " (" + questProgress[questSlot[1]].ToString() + " / " + data.questData[questSlot[1]].finishProgress + ")");
                //放弃任务
                if (GUI.Button ( new Rect(450,255,64,32), "Cancel")) {
					questProgress[questSlot[1]] = 0;
					questSlot[1] = 0;
					SortQuest();
				}
			}
			//-----------------------------------------
			if(questSlot[2] > 0){
                //任务名
                GUI.Label ( new Rect(275, 305, 280, 40), data.questData[questSlot[2]].questName);
                //任务进度
                GUI.Label ( new Rect(275, 330, 280, 40), data.questData[questSlot[2]].description + " (" + questProgress[questSlot[2]].ToString() + " / " + data.questData[questSlot[2]].finishProgress + ")");
                //放弃任务
                if (GUI.Button ( new Rect(450,315,64,32), "Cancel")) {
					questProgress[questSlot[2]] = 0;
					questSlot[2] = 0;
					SortQuest();
				}
			}
			//-----------------------------------------
			if(questSlot[3] > 0){
                //任务名
                GUI.Label ( new Rect(275, 365, 280, 40), data.questData[questSlot[3]].questName);
                //任务进度
                GUI.Label ( new Rect(275, 390, 280, 40), data.questData[questSlot[3]].description + " (" + questProgress[questSlot[3]].ToString() + " / " + data.questData[questSlot[3]].finishProgress + ")");
                //放弃任务
                if (GUI.Button ( new Rect(450,375,64,32), "Cancel")) {
					questProgress[questSlot[3]] = 0;
					questSlot[3] = 0;
					SortQuest();
				}
			}
			//-----------------------------------------
			if(questSlot[4] > 0){
                //任务名
                GUI.Label ( new Rect(275, 425, 280, 40), data.questData[questSlot[4]].questName);
                //任务进度
                GUI.Label ( new Rect(275, 450, 280, 40), data.questData[questSlot[4]].description + " (" + questProgress[questSlot[4]].ToString() + " / " + data.questData[questSlot[4]].finishProgress + ")");
                //放弃任务
                if (GUI.Button ( new Rect(450,435,64,32), "Cancel")) {
					questProgress[questSlot[4]] = 0;
					questSlot[4] = 0;
					SortQuest();
				}
			}
		}
		
	}
	
	void OnOffMenu(){
		if(!menu && Time.timeScale != 0.0f){
			menu = true;
			Time.timeScale = 0.0f;
			//Screen.lockCursor = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}else if(menu){
			menu = false;
			Time.timeScale = 1.0f;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
	
	public bool  Progress ( int id  ){
		bool haveQuest = false;
		for(int n= 0; n < questSlot.Length ; n++){
			if(questSlot[n] == id && id != 0){
				QuestDataC data = questDataBase.GetComponent<QuestDataC>();
				if(questProgress[id] < data.questData[questSlot[n]].finishProgress){
					questProgress[id] += 1;
					haveQuest = true;
				}
				print("Quest Slot =" + n);
			}
			
		}
		return haveQuest;
		
		
	}
	//-----------------------------------------------
	
	public bool CheckQuestSlot ( int id  ){
		bool  exist = false;
		for(int n= 0; n < questSlot.Length ; n++){
			if(questSlot[n] == id && id != 0){
				exist = true;
			}
			
		}
		return exist;
		
	}
	
	public int CheckQuestProgress ( int id  ){
		int qProgress = 0;
		for(int n= 0; n < questSlot.Length ; n++){
			if(questSlot[n] == id && id != 0){
				qProgress = questProgress[id];
			}
			
		}
		return qProgress;
		
	}
	
	public void  Clear ( int id  ){
		for(int n= 0; n < questSlot.Length ; n++){
			if(questSlot[n] == id && id != 0){
				questProgress[id] += 10;
				questSlot[n] = 0;
				SortQuest();
				print("Quest Slot =" + n);
			}
			
		}
		
	}
	
}