using UnityEngine;
using System.Collections;

public class ButtonScripts : MonoBehaviour {
	void Start () {
	
	}
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
