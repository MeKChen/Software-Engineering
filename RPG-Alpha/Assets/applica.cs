using UnityEngine;
using System.Collections;

public class applica : MonoBehaviour {
    public string Scenename;
    public GameObject SmallEmery;
    public GameObject Splider;

	void Start () {
       // OverBag.gameObject.SetActive(false);
	}
	void Update ()
    {
        SmallEmery = GameObject.Find("FieldGoblinC");
        Splider = GameObject.Find("SpiderC");
        if (SmallEmery == null && Splider == null)
        {
            Application.LoadLevel(Scenename);
        }
	
	}
}
