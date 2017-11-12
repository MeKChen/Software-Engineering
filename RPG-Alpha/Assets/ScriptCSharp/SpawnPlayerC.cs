using UnityEngine;
using System.Collections;

public class SpawnPlayerC : MonoBehaviour {
	
	public GameObject player;
	private Transform mainCam;
	public static bool onLoadGame;
	
	void Start(){
	
		GameObject currentPlayer = GameObject.FindWithTag ("Player");
		if(currentPlayer){
			string spawnPointName = currentPlayer.GetComponent<StatusC>().spawnPointName;
			GameObject spawnPoint = GameObject.Find(spawnPointName);
			if(spawnPoint && !onLoadGame){
				currentPlayer.transform.root.position = spawnPoint.transform.position;
				currentPlayer.transform.root.rotation = spawnPoint.transform.rotation;
			}

			PlayerPrefs.SetFloat("PlayerX", currentPlayer.transform.position.x);
			PlayerPrefs.SetFloat("PlayerY", currentPlayer.transform.position.y);
			PlayerPrefs.SetFloat("PlayerZ", currentPlayer.transform.position.z);

			onLoadGame = false;
			GameObject oldCam = currentPlayer.GetComponent<AttackTriggerC>().Maincam.gameObject;
			if(!oldCam){
				return;
			}
			GameObject[] cam = GameObject.FindGameObjectsWithTag("MainCamera"); 
			foreach(GameObject cam2 in cam) { 
				if(cam2 != oldCam){
					Destroy(cam2.gameObject);
				}
			}
			return;
		}

		GameObject spawnPlayer = Instantiate(player, transform.position , transform.rotation) as GameObject;
		mainCam = GameObject.FindWithTag ("MainCamera").transform;
		ARPGcameraC checkCam = mainCam.GetComponent<ARPGcameraC>();

		if(mainCam && checkCam){
			mainCam.GetComponent<ARPGcameraC>().target = spawnPlayer.transform;
		}

		PlayerPrefs.SetFloat("PlayerX", spawnPlayer.transform.position.x);
		PlayerPrefs.SetFloat("PlayerY", spawnPlayer.transform.position.y);
		PlayerPrefs.SetFloat("PlayerZ", spawnPlayer.transform.position.z);
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
}
