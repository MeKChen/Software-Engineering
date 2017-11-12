using UnityEngine;
using System.Collections;

public class CheckPointC : MonoBehaviour {
	
	private GameObject player;
	
	void  OnTriggerEnter ( Collider other  ){
		if (other.gameObject.tag == "Player") {
			player = other.gameObject;
			SaveData();
		}
	}
	
	void  SaveData (){
		PlayerPrefs.SetInt("PreviousSave", 10);
		PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
		PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
		PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
		print("Saved");
	}
}