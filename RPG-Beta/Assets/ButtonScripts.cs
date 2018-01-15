using UnityEngine;
using System.Collections;

public class ButtonScripts : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartGame()
    {
        Application.LoadLevel(1);
    }
    public void AuitGame()
    {
        Application.Quit();
    }
}
