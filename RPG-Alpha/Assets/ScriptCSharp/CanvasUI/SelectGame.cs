using UnityEngine;
using System.Collections;

public class SelectGame : MonoBehaviour {

    public void Level1()
    {
        Application.LoadLevel(1);
    }
    public void Level2()
    {
        Application.LoadLevel(2);
    }
    public void Level3() {
        Application.LoadLevel(3);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            Application.LoadLevel(0);
        }
    }
}
