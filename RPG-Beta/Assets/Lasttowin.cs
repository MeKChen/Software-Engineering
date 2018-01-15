using UnityEngine;
using System.Collections;

public class Lasttowin : MonoBehaviour {
    public string Scenename;
    public GameObject SmallEmery;
    public GameObject Splider;


    public GameObject player;
    //public GameObject OverBag;
    // Use this for initialization
    void Start()
    {
        // OverBag.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SmallEmery = GameObject.Find("FieldGoblinC");
        Splider = GameObject.Find("SpiderC");
        if (SmallEmery == null && Splider == null)
        {
            Destroy(player);
          player. GetComponent<UiMasterC>().DestroyAllUi();
            Application.LoadLevel(Scenename);
            //OverBag.gameObject.SetActive(true);
        }

    }
}
