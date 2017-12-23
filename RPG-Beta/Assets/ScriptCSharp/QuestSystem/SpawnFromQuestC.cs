﻿using UnityEngine;
using System.Collections;

public class SpawnFromQuestC : MonoBehaviour {
	
	public int questId = 1;
	public GameObject spawnObject;
	public int progressAbove = 0;	//Will spawn your spawnPrefab if your progression of the quest greater than this.
	public int progressBelow = 9999;	//Will spawn your spawnPrefab if your progression of the quest lower than this.
	private GameObject player;
	
	public enum SpawmQType{
		Instantiate = 0,
		Active = 1,
		Deactivate = 2,
		Destroy = 3
	}
	
	public SpawmQType by = SpawmQType.Instantiate;
	
	void Start(){
		CheckCondition();
	}

	public void CheckCondition(){
		if(by == SpawmQType.Instantiate){
			Spawn();
		}
		if(by == SpawmQType.Active){
			Activate();
		}
		if(by == SpawmQType.Deactivate){
			Deactivate();
		}
		if(by == SpawmQType.Destroy){
			DeleteObj();
		}
	}
	
	void Spawn(){
		player = GameObject.FindWithTag("Player");
		if(player){
			QuestStatC qstat = player.GetComponent<QuestStatC>();
			if(qstat){
				bool  letSpawn = player.GetComponent<QuestStatC>().CheckQuestSlot(questId);
				int checkProgress = player.GetComponent<QuestStatC>().CheckQuestProgress(questId);
				
				if(letSpawn && checkProgress >= progressAbove && checkProgress < progressBelow){
					GameObject m = Instantiate(spawnObject , transform.position , transform.rotation) as GameObject;
					m.name = spawnObject.name;
				}
			}
		}
	}
	
	void Activate(){
		player = GameObject.FindWithTag("Player");
		if(player){
			QuestStatC qstat = player.GetComponent<QuestStatC>();
			if(qstat){
				bool  letSpawn = player.GetComponent<QuestStatC>().CheckQuestSlot(questId);
				int checkProgress = player.GetComponent<QuestStatC>().CheckQuestProgress(questId);
				
				if(letSpawn && checkProgress >= progressAbove && checkProgress < progressBelow){
					spawnObject.SetActive(true);
				}
			}
		}
	}
	
	void Deactivate(){
		player = GameObject.FindWithTag("Player");
		if(player){
			QuestStatC qstat = player.GetComponent<QuestStatC>();
			if(qstat){
				bool  letSpawn = player.GetComponent<QuestStatC>().CheckQuestSlot(questId);
				int checkProgress = player.GetComponent<QuestStatC>().CheckQuestProgress(questId);
				
				if(letSpawn && checkProgress >= progressAbove && checkProgress < progressBelow){
					spawnObject.SetActive(false);
				}
			}
		}
	}
	
	void DeleteObj(){
		player = GameObject.FindWithTag("Player");
		if(player){
			QuestStatC qstat = player.GetComponent<QuestStatC>();
			if(qstat){
				bool  letSpawn = player.GetComponent<QuestStatC>().CheckQuestSlot(questId);
				int checkProgress = player.GetComponent<QuestStatC>().CheckQuestProgress(questId);
				
				if(letSpawn && checkProgress >= progressAbove && checkProgress < progressBelow){
					Destroy(spawnObject);
				}
			}
		}
	}
	
}