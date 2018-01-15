using UnityEngine;
using System.Collections;

public class applica : MonoBehaviour {
    public string Scenename;
    public GameObject SmallEmery;
    public GameObject Splider;

    //public GameObject OverBag;
	// Use this for initialization
	void Start () {
       // OverBag.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        SmallEmery = GameObject.Find("FieldGoblinC");
        Splider = GameObject.Find("SpiderC");
        if (SmallEmery == null && Splider == null)
        {
            Application.LoadLevel(Scenename);
            //OverBag.gameObject.SetActive(true);
        }
	
	}
}
